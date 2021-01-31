using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
namespace Draughts
{
    public class WebComm
    {
        Options o;
        Socket sender;
        public int StartClient()
        {
            byte[] bytes = new byte[8];
            int player = 0;
            try
            {
                IPAddress ipAddress = IPAddress.Parse(o.ip_addres);
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 1234);

                // Create a TCP/IP  socket.    
                sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the server. Catch any errors.    
                try
                {
                    // Connect to server
                    sender.Connect(remoteEP);

                    // Receive the information whether you're player one or two 
                    int bytesRec = sender.Receive(bytes);
                    player = bytes[7]-48;
                }
                catch (Exception e)
                {
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return player;
        }
        public void CloseConn()
        {
            try
            {
                //send end of connection message to the server
                byte[] bytes = Encoding.ASCII.GetBytes("endconn!");
                sender.Send(bytes);
                // Release the socket.    
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
            }
            catch (Exception e)
            {
            }
        }
        public void SendMove(Queue<int> move_list, bool jump)
        {
            if(move_list.Count==0)
            {
                return;
            }
            byte[] msg;
            int x1, y1, x2, y2;
            // Encode the data string into a byte array.    
            if (jump)
            {
                int jump_count = move_list.Count / 2 - 2;
                x1 = move_list.Dequeue();
                y1 = move_list.Dequeue();
                x2 = move_list.Dequeue();
                y2 = move_list.Dequeue();
                msg = Encoding.ASCII.GetBytes(string.Format("jmp{0}{1}{2}{3}{4}", jump_count, x1,y1,x2,y2));
                x1 = x2;
                y1 = y2;
                // Send the data through the socket.    
                sender.Send(msg);
                for (int i = 1; i <= jump_count; i++)
                {
                    x2 = move_list.Dequeue();
                    y2 = move_list.Dequeue();
                    msg = Encoding.ASCII.GetBytes(string.Format("jmp{0}{1}{2}{3}{4}", jump_count-i, x1, y1, x2, y2));
                    // Send the data through the socket.    
                    sender.Send(msg);
                    x1 = x2;
                    y1 = y2;
                }
                // Send the data through the socket.    
                sender.Send(msg);
            }
            else
            {
                x1 = move_list.Dequeue();
                y1 = move_list.Dequeue();
                x2 = move_list.Dequeue();
                y2 = move_list.Dequeue();
                msg = Encoding.ASCII.GetBytes(string.Format("move{0}{1}{2}{3}",x1,y1,x2,y2));
                // Send the data through the socket.    
                 sender.Send(msg);
            }
        }
        public Queue<int> GetMove()
        {
            Queue<int> res = new Queue<int>();
            try
            {
                byte[] bytes = new byte[8];
                sender.Receive(bytes);
                string result = System.Text.Encoding.ASCII.GetString(bytes);
                switch (result.Substring(0, 3))
                {
                    case "mov":
                        res.Enqueue(result[4] - 48);
                        res.Enqueue(result[5] - 48);
                        res.Enqueue(result[6] - 48);
                        res.Enqueue(result[7] - 48);
                        break;
                    case "jmp":
                        res.Enqueue(result[4] - 48);
                        res.Enqueue(result[5] - 48);
                        res.Enqueue(result[6] - 48);
                        res.Enqueue(result[7] - 48);
                        for (int i = 0; i < result[3] - 48; i++)
                        {
                            sender.Receive(bytes);
                            result = System.Text.Encoding.ASCII.GetString(bytes);
                            switch (result.Substring(0, 3))
                            {
                                case "win":
                                    res.Clear();
                                    res.Enqueue(1);
                                    res.Enqueue(result[7] - 48);
                                    break;
                                case "loo":
                                    break;
                                case "jmp":
                                    res.Enqueue(result[6] - 48);
                                    res.Enqueue(result[7] - 48);
                                    break;
                            }
                        }
                        break;
                    case "loo":
                        res.Enqueue(-1);
                        res.Enqueue(result[7] - 48);
                        break;
                    case "win":
                        res.Enqueue(1);
                        res.Enqueue(result[7] - 48);
                        break;
                }
            }
            catch (Exception e)
            {

            }
            return res;
        }
        public WebComm(Options o_in)
        {
            o = o_in;
        }
    }
}