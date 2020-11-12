using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts
{
    //base class not representing actual pawn type, but useful to cover any pawn type

    abstract class BasePawn
    {

        public int x;
        public int y;
        public Board_situation board;
        public int side;
        public int type;
        public abstract int move(int a, int b);        
        //verifies if there exists a viable jump move for the pawn
        public int jump()
        {
            if (x + 2 < 8 && y + 2 < 8)
            {
                if (board.get_square(x + 2, y + 2) == 0 && board.get_square(x + 1, y + 1) == 3 - side)
                {
                    return 1;
                }
            }
            if (x - 2 > -1 && y + 2 < 8)
            {
                if (board.get_square(x - 2, y + 2) == 0 && board.get_square(x - 1, y + 1) == 3 - side)
                {
                    return 1;
                }
            }
            if (x + 2 < 8 && y - 2 > -1)
            {
                if (board.get_square(x + 2, y - 2) == 0 && board.get_square(x + 1, y - 1) == 3 - side)
                {
                    return 1;
                }
            }
            if (x - 2 > -1 && y - 2 > -1)
            {
                if (board.get_square(x - 2, y - 2) == 0 && board.get_square(x - 1, y - 1) == 3 - side)
                {
                    return 1;
                }
            }
            return 0;
        }
        //finds first possible jump direction
        public int jump(int a, int b)
        {
            int res = 0;
            if (y + 2 == b)//verify if target field is in jump range part 1
            {
                if (x + 2 == a && board.get_square(x + 1, y + 1) == 3 - side)//verify if target field is in jump range part 2 and verify if field through which we jump if hostile
                {
                    board.remove(x + 1, y + 1); //removing captured pawn from board
                    res = 2;
                }
                else if (x - 2 == a && board.get_square(x - 1, y + 1) == 3 - side)//verify if target field is in jump range part 2 and verify if field through which we jump if hostile
                {
                    board.remove(x - 1, y + 1); //removing captured pawn from board
                    res = 2;
                }
            }
            else if (y - 2 == b)//verify if target field is in jump range part 1
            {
                if (x + 2 == a && board.get_square(x + 1, y - 1) == 3 - side)//verify if target field is in jump range part 2 and verify if field through which we jump if hostile
                {
                    board.remove(x + 1, y - 1); //removing captured pawn from board
                    res = 2;
                }
                else if (x - 2 == a && board.get_square(x - 1, y - 1) == 3 - side)//verify if target field is in jump range part 2 and verify if field through which we jump if hostile
                {
                    board.remove(x - 1, y - 1); //removing captured pawn from board
                    res = 2;
                }
            }
            if (res > 0) // if jump is successful, reposition the pawn and check if next jump is possible
            {
                x = a;
                y = b;
                res += jump();
            }
            return res;
        }
        //not implemented
        public abstract BasePawn promotion();
    }
    //starting pawn type 
    class Pawn : BasePawn
    {
        public int direction;
        //constructor taking direction which the pawn is facing, its x-y position and reference to object keeping information about board state
        public Pawn(int _direction, int _x, int _y, Board_situation _board)
        {
            type = 0;
            direction = _direction;
            x = _x;
            y = _y;
            board = _board;
            if (_direction == 1)
            {
                side = 1;
            }
            else
            {
                side = 2;
            }
        }

        //move: 0 - incorect, 1 - standard correct 2 - jump without possibility of continuation 3 - jump with possibility of continuation

        //execute move
        public override int move(int a, int b)
        {
            int res;
            //standard move
            if (y + direction == b && (x + 1 == a || x - 1 == a))
            {
                if (board.get_square(a, b) == 0) //verify if target square is empty
                {
                    res = 1;
                    x = a;
                    y = b;
                }
                else
                {
                    res = 0;
                }
            }
            else
            {
                //jump
                res = jump(a, b);
            }

            return res;
        }
        //upgrade pawn to the king
        public override BasePawn promotion()
        {
            return new King(side, x, y, board);
        }
        //execute jump and verify if further jumps are possible
    }
    //promoted pawn available after reaching the last row of the board
    class King : BasePawn
    {
        //constructor taking number of the player controlling the king, its x-y position and reference to object keeping information about board state
        public King(int _side, int _x, int _y, Board_situation _board)
        {
            type = 2;
            x = _x;
            y = _y;
            board = _board;
            side = _side;
        }
        //execute the move
        public override int move(int a, int b)
        {
            int res = 0;
            if (board.get_square(a, b) != 0)//if target square isn't empty, the move isn't possible
            {
                res = 0;
            }
            else if (x - a == y - b)//verify if the is in correct diagonal direction
            {
                res = 1;
                if (x > a)
                {
                    int j = y - 1;
                    for (int i = x - 1; i > a; i--)
                    {
                        if (board.get_square(i, j) == 3 - side)//detect hostile pawns in the path
                        {
                            if (i - 1 == a)//verify if the move ends right after capturing the hostile pawn
                            {
                                board.remove(i, j);//remove captured pawn
                                res = 2;
                            }
                            else
                            {
                                return 0;//otherwise the move is incorrect
                            }
                            break;
                        }
                        if (board.get_square(i, j) == side)//if your own pawn if in the path, move is incorrect
                        {
                            return 0;
                        }
                        j--;
                    }
                }
                else
                {
                    int j = y + 1;
                    for (int i = x + 1; i < a; i++)
                    {
                        if (board.get_square(i, j) == 3 - side)//detect hostile pawns in the path
                        {
                            if (i + 1 == a)//verify if the move ends right after capturing the hostile pawn
                            {
                                board.remove(i, j);//remove captured pawn
                                res = 2;
                            }
                            else
                            {
                                return 0;//otherwise the move is incorrect
                            }
                            break;
                        }
                        if (board.get_square(i, j) == side)//if your own pawn if in the path, move is incorrect
                        {
                            return 0;
                        }
                        j++;
                    }
                }
            }
            else if (x - a == b - y)
            {
                res = 1;
                if (x > a)
                {
                    int j = y + 1;
                    for (int i = x - 1; i > a; i--)
                    {
                        if (board.get_square(i, j) == 3 - side)//detect hostile pawns in the path
                        {
                            if (i - 1 == a)//verify if the move ends right after capturing the hostile pawn
                            {
                                board.remove(i, j);//remove captured pawn
                                res = 2;
                            }
                            else
                            {
                                return 0;//otherwise the move is incorrect
                            }
                            break;
                        }
                        if (board.get_square(i, j) == side)
                        {
                            return 0;//if your own pawn if in the path, move is incorrect
                        }
                        j++;
                    }
                }
                else
                {
                    int j = y - 1;
                    for (int i = x + 1; i < a; i++)
                    {
                        if (board.get_square(i, j) == 3 - side)//detect hostile pawns in the path
                        {
                            if (i + 1 == a)//verify if the move ends right after capturing the hostile pawn
                            {
                                board.remove(i, j);//remove captured pawn
                                res = 2;
                            }
                            else
                            {
                                return 0;//otherwise the move is incorrect
                            }
                            break;
                        }
                        if (board.get_square(i, j) == side)
                        {
                            return 0;//if your own pawn if in the path, move is incorrect
                        }
                        j--;
                    }
                }
            }
            if (res > 0)
            {
                x = a;
                y = b;
                res += jump();//verify if further jumps are possible
            }
            return res;
        }
        public override BasePawn promotion()
        {
            return this;
        }
    }
}
