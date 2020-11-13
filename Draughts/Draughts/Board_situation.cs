using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts
{
    class Board_situation
    {

        public List<BasePawn> player1;//list of pawns controlled by player 1
        public List<BasePawn> player2;//list of pawns controlled by player 2
        public int[,] board = new int[8, 8];//array of 0's for empty field, numbers equal to 1 modulo 3 for player one pawn, numbers equal to 2 modulo 3 for player two pawn
        private int iplayer1;//number of pawns controlled by player 1
        private int iplayer2;//number of pawns controlled by player 1
        public int active_side = 1;
        public string to_kill;
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
        public bool check_promotion(int x, int y)
        {
            bool promoted = false;
            if (board[x, y] % 3 == 1)
            {
                if (y == 7)
                {
                    promoted = true;
                    player1[board[x, y] / 3] = player1[board[x, y] / 3].promotion();
                }
            }
            if (board[x, y] % 3 == 2)
            {
                if (y == 0)
                {
                    promoted = true;
                    player2[board[x, y] / 3] = player2[board[x, y] / 3].promotion();
                }
            }
            return promoted;
        }
        //get index of the pawn
        public int get_index(int x, int y)
        {
            return board[x, y];
        }
        
        //execute a move
        public int move(int x1, int y1, int x2, int y2)
        {
            int res = 0;
            if (board[x1, y1] == 0 || board[x2, y2] != 0)
            {
                return 0;//you can't move a pawn that isn't there
            }
            if (board[x1, y1] % 3 == 1)
            {
                res =  player1[board[x1, y1] / 3].move(x2, y2);
                if (res > 0)
                {
                    board[x2, y2] = board[x1, y1];
                    board[x1, y1] = 0;
                }
                return res;
            }
            else
            {
                res = player2[board[x1, y1] / 3].move(x2, y2);
                if (res > 0)
                {
                    board[x2, y2] = board[x1, y1];
                    board[x1, y1] = 0;
                }
                return res;
            }
        }

        public void remove(int x, int y)
        {
            if (board[x, y] % 3 == 1)
            {                
                iplayer1--;
            }
            else
            {
                iplayer2--;
            }
            to_kill = string.Format("{0}-{1}", x, y);
            board[x, y] = 0;            
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
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)//set empty fields
                {
                    board[i, j] = 0;
                }
            }            
            iplayer1 = 12;
            iplayer2 = 12;
        }

    }
}
