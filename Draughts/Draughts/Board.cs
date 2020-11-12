using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Draughts
{
    class DraugthsButton : Button
    {
        public int x;
        public int y;
        public int id;
    }
    public partial class Board : Form
    {
        int square_size = 64;
        int phase = 0;
        Menu parent;
        DraugthsButton highlighted;
        Options o;
        Board_situation state;
        public Board(Menu main, Options in_options)
        {
            parent = main;
            o = in_options;
            InitializeComponent();
        }

        private void Board_Load(object sender, EventArgs e)
        {
            this.BackgroundImage = Properties.Resources.background;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            state = new Board_situation();
            state.new_game();
            Board_setup();
        }
        //on closing of the game window, the program should return to the main menu and if the game if on-line send info to the server
        private void Board_closed(object sender, FormClosedEventArgs e)
        {
            //to do
            parent.Show();
        }
        private void Board_setup()
        {
            int pawn_number = 1;
            for (int j = 0; j < 3; j++)//set pawns on the board
            {
                for (int i = (j + 1) % 2; i < 8; i += 2)
                {
                    //side 1, white by default
                    state.player1.Add(new Pawn(1, i, j, state));//add the pawn to the board state keeper pawn list
                    state.board[i, j] = pawn_number;//add the pawn id the board tab
                    this.Controls.Add(Create_pawn(false, pawn_number, i, j));//draw the pawn

                    //side 2, black by default
                    state.player1.Add(new Pawn(-1, 7 - i, 7 - j, state));//add the pawn to the board state keeper pawn list
                    state.board[7 - i, 7 - j] = pawn_number + 1;//add the pawn id the board tab
                    this.Controls.Add(Create_pawn(false, pawn_number + 1, 7 - i, 7 - j));//draw the pawn
                    pawn_number += 3;
                }
            }
            //add buttons for empty fields that the pawns can access
            for (int i = 0; i < 8; i += 2)
            {
                state.player1.Add(new Pawn(1, i, 3, state));
                state.board[i, 3] = 0;
                this.Controls.Add(Create_pawn(false, 0, i, 3));

                state.player1.Add(new Pawn(-1, 7 - i, 4, state));
                state.board[i, 4] = 0;
                this.Controls.Add(Create_pawn(false, 0, 7 - i, 4));
            }
        }
        private DraugthsButton Create_pawn(bool king, int id, int x, int y)
        {
            DraugthsButton pawn = new DraugthsButton();
            pawn.id = id;
            pawn.x = x;
            pawn.y = y;
            pawn.Click += new EventHandler(this.ClickHandler);
            if (king)
            {
                switch (id % 3)
                {
                    case 0:
                        pawn.Image = Properties.Resources.field;
                        break;
                    case 1:
                        pawn.Image = Properties.Resources.white_king;
                        break;
                    case 2:
                        pawn.Image = Properties.Resources.black_king;
                        break;
                }
            }
            else
            {
                switch (id % 3)
                {
                    case 0:
                        pawn.Image = Properties.Resources.field;
                        break;
                    case 1:
                        pawn.Image = Properties.Resources.white_pawn;
                        break;
                    case 2:
                        pawn.Image = Properties.Resources.black_pawn;
                        break;
                }
            }
            pawn.Size = new System.Drawing.Size(square_size, square_size);
            pawn.Location = new System.Drawing.Point(x * square_size, y * square_size);
            pawn.TabStop = false;
            pawn.FlatStyle = FlatStyle.Flat;
            pawn.FlatAppearance.BorderSize = 0;
            return pawn;
        }
        void ClickHandler(object sender, EventArgs e)
        {
           
        }
    }
}
