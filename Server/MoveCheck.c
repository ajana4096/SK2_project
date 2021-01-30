//check if the pawn at given position is entitled to promotion
void set_board(int board[8][8], int player1[12], int player2[12])
{
    //board={0};
    int pawn_number = 1;
    for (int j = 0; j < 3; j++) //set pawns on the board
    {
        for (int i = (j + 1) % 2; i < 8; i += 2)
        {
            //side 1, white by default
            player1[pawn_number / 3] = 1; //add the pawn to the board state keeper pawn list
            board[i][j] = pawn_number;    //add the pawn id the board tab

            //side 2, black by default
            player2[pawn_number / 3] = -1;         //add the pawn to the board state keeper pawn list
            board[7 - i][7 - j] = pawn_number + 1; //add the pawn id the board tab

            pawn_number += 3;
        }
    }
}
int check_promotion(int x, int y, int board[8][8], int player1[12], int player2[12])
{
    int promoted = 0;
    if (board[x][y] % 3 == 1)
    {
        if (y == 7)
        {
            promoted = 1;
            player1[board[x][y] / 3] = 8;
        }
    }
    if (board[x][y] % 3 == 2)
    {
        if (y == 0)
        {
            promoted = 1;
            player2[board[x][y] / 3] = -8;
        }
    }
    return promoted;
}
//get index of the pawn
int get_index(int x, int y, int board[8][8])
{
    return board[x][y];
}
int get_square(int x, int y, int board[8][8])
{
    if (x < 0 || x >= 8 || y < 0 || y >= 8) //if the field asked about is out of the board return -1
    {
        return -1;
    }
    return board[x][y] % 3;
}
//execute a move

int king(int board[8][8], int x1, int y1, int x2, int y2, int side)
{
    if (x1 - x2 == y1 - y2) //verify if the is in correct diagonal direction
    {
        if (x1 > x2)
        {
            int j = y1 - 1;
            for (int i = x1 - 1; i > x2; i--)
            {
                if (get_square(i, j, board) == 3 - side) //detect hostile pawns in the path
                {
                    if (i - 1 == x2) //verify if the move ends right after capturing the hostile pawn
                    {
                        board[i][j] = 0; //remove captured pawn
                        return 2;
                    }
                    else
                    {
                        return 0; //otherwise the move is incorrect
                    }
                    break;
                }
                if (get_square(i, j, board) == side) //if your own pawn if in the path, move is incorrect
                {
                    return 0;
                }
                j--;
            }
            return 1;
        }
        else
        {
            int j = y1 + 1;
            for (int i = x1 + 1; i < x2; i++)
            {
                if (get_square(i, j, board) == 3 - side) //detect hostile pawns in the path
                {
                    if (i + 1 == x2) //verify if the move ends right after capturing the hostile pawn
                    {
                        board[i][j] = 0; //remove captured pawn
                        return 2;
                    }
                    else
                    {
                        return 0; //otherwise the move is incorrect
                    }
                    break;
                }
                if (get_square(i, j, board) == side) //if your own pawn if in the path, move is incorrect
                {
                    return 0;
                }
                j++;
            }
            return 1;
        }
    }
    else if (x1 - x2 == y2 - y1)
    {
        if (x1 > x2)
        {
            int j = y1 + 1;
            for (int i = x1 - 1; i > x2; i--)
            {
                if (get_square(i, j, board) == 3 - side) //detect hostile pawns in the path
                {
                    if (i - 1 == x2) //verify if the move ends right after capturing the hostile pawn
                    {
                        board[i][j] = 0; //remove captured pawn
                        return 2;
                    }
                    else
                    {
                        return 0; //otherwise the move is incorrect
                    }
                    break;
                }
                if (get_square(i, j, board) == side)
                {
                    return 0; //if your own pawn if in the path, move is incorrect
                }
                j++;
            }
            return 1;
        }
        else
        {
            int j = y1 - 1;
            for (int i = x1 + 1; i < x2; i++)
            {
                if (get_square(i, j, board) == 3 - side) //detect hostile pawns in the path
                {
                    if (i + 1 == x2) //verify if the move ends right after capturing the hostile pawn
                    {
                        board[i][j] = 0; //remove captured pawn
                        return 2;
                    }
                    else
                    {
                        return 0; //otherwise the move is incorrect
                    }
                    break;
                }
                if (get_square(i, j, board) == side)
                {
                    return 0; //if your own pawn if in the path, move is incorrect
                }
                j--;
            }
            return 1;
        }
    }
    return 0;
}
int move_pawn(int board[8][8], int x1, int y1, int x2, int y2, int side)
{
    if (side == 1)
    {
        if (y2 - y1 == 1 && (x2 - x1 == 1 || x2 - y1 == -1))
        {
            board[x2][y2] = board[x1][y1];
            board[x1][y1] = 0;
            return 1;
        }
        else
        {
            return 0;
        }
    }
    else
    {
        if (y2 - y1 == -1 && (x2 - x1 == 1 || x2 - y1 == -1))
        {
            board[x2][y2] = board[x1][y1];
            board[x1][y1] = 0;
            return 1;
        }
        else
        {
            return 0;
        }
    }
}
int move(int board[8][8], int player1[12], int player2[12], int x1, int y1, int x2, int y2, int side)
{
    if (get_square(x1, y1, board) != side || get_square(x2, y2, board) != 0)
    {
        return 0; //you can't move a pawn that isn't there or two an occupied square
    }
    int index = get_index(x1, y1, board);
    if (side == 1)
    {
        if (player1[index] == 1)
        {
            return move_pawn(board, x1, y1, x2, y2, side);
        }
        else
        {
            int res = king(board, x1, y1, x2, y2, side);
            if(res!=1)
            {
                res = 0;
            }
            return res;
        }
    }
    else
    {
        if (player2[index] == -1)
        {
            return move_pawn(board, x1, y1, x2, y2, side);
        }
        else
        {
            return king(board, x1, y1, x2, y2, side);
        }
    }
}
int jump_pawn(int board[8][8], int x1, int y1, int x2, int y2, int side)
{
    if (x2 - x1 == 2 && y2 - y1 == 2)
    {
        if (get_square(x1 + 1, y1 + 1, board) == 3 - side)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
    if (x2 - x1 == -2 && y2 - y1 == -2)
    {
        if (get_square(x1 - 1, y1 - 1, board) == 3 - side)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
    if (x2 - x1 == 2 && y2 - y1 == -2)
    {
        if (get_square(x1 + 1, y1 - 1, board) == 3 - side)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
    if (x2 - x1 == -2 && y2 - y1 == 2)
    {
        if (get_square(x1 - 1, y1 + 1, board) == 3 - side)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
    return 0;
}
int jump(int board[8][8], int player1[12], int player2[12], int x1, int y1, int x2, int y2, int side)
{
    if (get_square(x1, y1, board) != side || get_square(x2, y2, board) != 0)
    {
        return 0; //you can't move a pawn that isn't there or two an occupied square
    }
    int index = get_index(x1, y1, board);
    if (side == 1)
    {
        if (player1[index] == 1)
        {
            return jump_pawn(board, x1, y1, x2, y2, side);
        }
        else
        {

            return king(board, x1, y1, x2, y2, side)-1;
        }
    }
    else
    {
        if (player2[index] == -1)
        {
            return jump_pawn(board, x1, y1, x2, y2, side);
        }
        else
        {

            return king(board, x1, y1, x2, y2, side)-1;
        }
    }
}
