using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts
{
    class Board_situation
    {

        private List<BasePawn> player1;//list of pawns controlled by player 1
        private List<BasePawn> player2;//list of pawns controlled by player 2
        private int[,] board = new int[8, 8];//array of 0's for empty field, numbers equal to 1 modulo 3 for player one pawn, numbers equal to 2 modulo 3 for player two pawn
        private int iplayer1;//number of pawns controlled by player 1
        private int iplayer2;//number of pawns controlled by player 1

        //get info about status of the given field
        public int get_square(int x, int y)
        {
            if (x < 0 || x >= 8 || y < 0 || y >= 8)//if the field asked about is out of the board return -1
            {
                return -1;
            }
            return board[x, y] % 3;
        }
        //check if some of the pawns are entitled to promotion
        public void sprawdzenie(int x, int y)
        {
            if (board[x, y] % 3 == 1)
            {
                if (y == 7)
                {
                    //to do
                    player1[board[x, y] / 3] = player1[board[x, y] / 3].promotion();
                }
                return;
            }
            if (board[x, y] % 3 == 2)
            {
                if (y == 0)
                {
                    //to do
                    player2[board[x, y] / 3] = player2[board[x, y] / 3].promotion();
                }
                return;
            }
        }
        //get index of the pawn
        public int get_index(int x, int y)
        {
            return board[x, y];
        }
        
        //execute a move
        public int move(int x1, int y1, int x2, int y2)
        {
            if (board[x1, y1] == 0 || board[x2, y2] != 0)
            {
                return 0;//you can't move a pawn that isn't there
            }
            if (board[x1, y1] % 3 == 1)
            {
                return player1[board[x1, y1] / 3].move(x2, y2);
            }
            else
            {
                return player2[board[x1, y1] / 3].move(x2, y2);
            }
        }

        public void remove(int x, int y)
        {
            if (board[x, y] % 3 == 1)
            {
                player1[board[x, y] / 3].zbij();
                iplayer1--;
            }
            else
            {
                iplayer2--;
                player2[board[x, y] / 3].zbij();
            }
            board[x, y] = 0;
            //to do
        }

        public Board_situation()
        {
            board = new int[8, 8];
            player1 = new List<BasePawn>();
            player2 = new List<BasePawn>();
        }

        //check if one of the players have won
        public int check_if_win()
        {
            int res = 0;
            if (iplayer1 == 0)
            {
                res = 2;
            }
            if (iplayer2 == 0)
            {
                res = 1;
            }
            return res;
        }

        public void new_game()
        {
            int numer_pionka = 1;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)//set empty fields
                {
                    board[i, j] = 0;
                }
            }
            for (int j = 0; j < 4; j++)
            {
                for (int i = j % 2; i < 8; i += 2)
                {
                    player1.Add(new Pawn(1, i, j, this));
                    board[i, j] = numer_pionka;
                    //to do
                    player2.Add(new Pawn(-1, 7 - i, 7 - j, this));
                    board[7 - i, 7 - j] = numer_pionka + 1;
                    //to do
                    numer_pionka += 3;
                }
            }
            iplayer1 = 12;
            iplayer2 = 12;
        }

    }
}
