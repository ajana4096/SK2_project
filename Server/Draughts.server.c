#include <sys/types.h>
#include <sys/wait.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <unistd.h>
#include <netdb.h>
#include <signal.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>
#include <pthread.h>

#include "MoveCheck.c"
#define SERVER_PORT 1234
#define QUEUE_SIZE 0

pthread_mutex_t lock_queue = PTHREAD_MUTEX_INITIALIZER;

int queue[QUEUE_SIZE] = {0};
int server_socket_descriptor;
int awaiting_player=0;
struct thread_data_t
{
    int connection_socket_descriptor1;
    int connection_socket_descriptor2;
};

void close_server()
{
    close(server_socket_descriptor);
    for (int i = 0; i < QUEUE_SIZE; i++)
    {
        if (queue[i] != 0 && queue[i] >= 0)
        {
            close(queue[i]);
        }
    }
    exit(0);
}

void *ThreadBehavior(void *t_data)
{
    char input[10];
    int size;
    struct thread_data_t *th_data = (struct thread_data_t *)t_data;

    while (1)
    {
        while(size<6)
        {
            size = read(th_data->connection_socket_descriptor1, input, 7);
        }
        if (strncmp(input, "endconn", 6) == 0)
        {
            strncpy(input,"winner1",7);
            write(th_data->connection_socket_descriptor2,input,7);
            break;
            break;
        }
        else 
        {
            if (strncmp(input, "mov", 3) == 0)
            {                    
                int x1 = input[3] - 48;
                int y1 = input[4] - 48;
                int x2 = input[5] - 48;
                int y2 = input[6] - 48;
                int move_evaluation = correct_move(x1,y1,x2,y2);
                switch(move_evaluation)
                {
                    case 0:
                        printf("Wykryto probe oszustwa. Gracz 2 zdyskwalifikowany.");                        
                        strncpy(input,"winner2",7);                        
                        write(th_data->connection_socket_descriptor2,input,7);

                        strncpy(input,"looser2",7);                        
                        write(th_data->connection_socket_descriptor1,input,7);
                        break;
                    case 1:
                        write(th_data->connection_socket_descriptor2,input,7);
                        break;
                    case 2:
                        printf("Gracz 2 zwyciezyl.");                        
                        strncpy(input,"winner3",7);                        
                        write(th_data->connection_socket_descriptor1,input,7);

                        strncpy(input,"looser3",7);                        
                        write(th_data->connection_socket_descriptor2,input,7);                        
                        break;
                }
            }
            else
            {
                printf("Wykryto probe oszustwa. Gracz 1 zdyskwalifikowany.");
                strncpy(input,"winner2",7);
                write(th_data->connection_socket_descriptor2,input,7);
                        
                strncpy(input,"looser2",7);                        
                write(th_data->connection_socket_descriptor1,input,7);
                break;
            }
        }
        printf("Komenda dla gracza 1: %s\n",input);
        size=0;
        while(size<6)
        {
            size = read(th_data->connection_socket_descriptor2, input, 7);  
        }
        if (strncmp(input, "endconn", 6) == 0)
        {
            strncpy(input,"winner1",7);
            write(th_data->connection_socket_descriptor1,input,7);
            break;
        }
        else 
        {
            if (strncmp(input, "mov", 3) == 0)
            {                    
                int x1 = input[3] - 48;
                int y1 = input[4] - 48;
                int x2 = input[5] - 48;
                int y2 = input[6] - 48;
                int move_evaluation = correct_move(x1,y1,x2,y2);
                switch(move_evaluation)
                {
                    case 0:
                        printf("Wykryto probe oszustwa. Gracz 2 zdyskwalifikowany.");                        
                        strncpy(input,"winner2",7);                        
                        write(th_data->connection_socket_descriptor1,input,7);

                        strncpy(input,"looser2",7);                        
                        write(th_data->connection_socket_descriptor2,input,7);
                        break;
                    case 1:
                        write(th_data->connection_socket_descriptor1,input,7);
                        break;
                    case 2:
                        printf("Gracz 2 zwyciezyl.");                        
                        strncpy(input,"winner3",7);                        
                        write(th_data->connection_socket_descriptor2,input,7);

                        strncpy(input,"looser3",7);                        
                        write(th_data->connection_socket_descriptor1,input,7);
                        break;
                        break;
                }
            }
            else
            {
                printf("Wykryto probe oszustwa. Gracz 2 zdyskwalifikowany.");
                strncpy(input,"winner2",7);
                write(th_data->connection_socket_descriptor1,input,7);
                        
                strncpy(input,"looser2",7);                        
                write(th_data->connection_socket_descriptor2,input,7);
                break;
            }
            
        }
        printf("Komenda dla gracza 2: %s\n",input);
        size=0;
    }
    printf("Klient sie rozlaczyl.\n");
    pthread_detach(pthread_self());
    close(th_data->connection_socket_descriptor1);
    close(th_data->connection_socket_descriptor2);
    for (int i = 0; i < QUEUE_SIZE; i++)
    {
        if (queue[i] == th_data->connection_socket_descriptor1)
        {
            pthread_mutex_lock(&lock_queue);
            queue[i] = 0;
            pthread_mutex_unlock(&lock_queue);
        }
        if (queue[i] == th_data->connection_socket_descriptor2)
        {
            pthread_mutex_lock(&lock_queue);
            queue[i] = 0;
            pthread_mutex_unlock(&lock_queue);
        }
    }
    free(t_data);
    pthread_exit(NULL);
}

void handleConnection(int connection_socket_descriptor)
{
    int create_result = 0;
    pthread_mutex_lock(&lock_queue);
    for (int i = 0; i < QUEUE_SIZE; i++)
    {
        if (queue[i] == 0)
        {
            queue[i] = connection_socket_descriptor;
            break;
        }
    }

 
    if(awaiting_player==0)
    {
        awaiting_player=connection_socket_descriptor;
        pthread_mutex_unlock(&lock_queue);
    }
    else 
    {
        pthread_t thread1;

        struct thread_data_t *t_data = (struct thread_data_t *)malloc(sizeof(struct thread_data_t));
        t_data->connection_socket_descriptor1 = connection_socket_descriptor;
        t_data->connection_socket_descriptor2 = awaiting_player;
        awaiting_player=0;
        pthread_mutex_unlock(&lock_queue);
        create_result = pthread_create(&thread1, NULL, ThreadBehavior, (void *)t_data);
        if (create_result)
        {
            printf("Blad przy probie utworzenia watku, kod b1edu: %d\n", create_result);
            exit(-1);
        }
    }
}

int main(int argc, char *argv[])
{
    int server_socket_descriptor;
    int connection_socket_descriptor;
    int bind_result;
    int listen_result;
    char reuse_addr_val = 1;
    struct sockaddr_in server_address;

    //inicjalizacja gniazda serwera

    signal(SIGINT, close_server);

    memset(&server_address, 0, sizeof(struct sockaddr));
    server_address.sin_family = AF_INET;
    server_address.sin_addr.s_addr = htonl(INADDR_ANY);
    server_address.sin_port = htons(SERVER_PORT);

    server_socket_descriptor = socket(AF_INET, SOCK_STREAM, 0);
    if (server_socket_descriptor < 0)
    {
        fprintf(stderr, "%s: Blad przy probie utworzenia gniazda..\n", argv[0]);
        exit(1);
    }
    setsockopt(server_socket_descriptor, SOL_SOCKET, SO_REUSEADDR, (char *)&reuse_addr_val, sizeof(reuse_addr_val));

    bind_result = bind(server_socket_descriptor, (struct sockaddr *)&server_address, sizeof(struct sockaddr));
    if (bind_result < 0)
    {
        fprintf(stderr, "%s: Blad przy probie dowiazania adresu IP i numeru portu do gniazda.\n", argv[0]);
        exit(1);
    }

    listen_result = listen(server_socket_descriptor, QUEUE_SIZE);
    if (listen_result < 0)
    {
        fprintf(stderr, "%s: Blad przy probie ustawienia wielkosci kolejki.\n", argv[0]);
        exit(1);
    }

    while (1)
    {
        connection_socket_descriptor = accept(server_socket_descriptor, NULL, NULL);
        if (connection_socket_descriptor < 0)
        {
            fprintf(stderr, "%s: Blad przy probie utworzenia gniazda dla polaczenia.\n", argv[0]);
            exit(1);
        }
        printf("Dolaczyl nowy klient.\n");
        handleConnection(connection_socket_descriptor);
    }

    close(server_socket_descriptor);
    return (0);
}