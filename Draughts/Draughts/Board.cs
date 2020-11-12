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
    class DraughtsButton : Button
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
        DraughtsButton highlighted;
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
                    state.player2.Add(new Pawn(-1, 7 - i, 7 - j, state));//add the pawn to the board state keeper pawn list
                    state.board[7 - i, 7 - j] = pawn_number + 1;//add the pawn id the board tab
                    this.Controls.Add(Create_pawn(false, pawn_number + 1, 7 - i, 7 - j));//draw the pawn
                    pawn_number += 3;
                }
            }
            //add buttons for empty fields that the pawns can access
            for (int i = 0; i < 8; i += 2)
            {
                state.board[i, 3] = 0;
                this.Controls.Add(Create_pawn(false, 0, i, 3));

                state.board[i, 4] = 0;
                this.Controls.Add(Create_pawn(false, 0, 7 - i, 4));
            }
        }
        private DraughtsButton Create_pawn(bool king, int id, int x, int y)
        {
            DraughtsButton pawn = new DraughtsButton();
            pawn.id = id;
            pawn.Name = string.Format("{0}-{1}", x, y);
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
            int result = 0;
            DraughtsButton field = (DraughtsButton)sender;
            switch (phase)
            {
                case 0:     //choose the pawn
                    if (state.get_square(field.x, field.y) == state.active_side)
                    {
                        field.FlatStyle = FlatStyle.Flat;
                        field.FlatAppearance.BorderColor = Color.DarkGreen;
                        field.FlatAppearance.BorderSize = 2;
                        highlighted = field;
                        phase = 1;
                    }
                    break;
                case 1: //make the move or change the pawn
                    if (state.get_square(field.x, field.y) == state.active_side) //change the pawn
                    {
                        highlighted.FlatAppearance.BorderSize = 0;
                        highlighted.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        field.FlatStyle = FlatStyle.Flat;
                        field.FlatAppearance.BorderColor = Color.DarkGreen;
                        field.FlatAppearance.BorderSize = 2;
                        highlighted = field;
                    }
                    else
                    {
                        if (state.get_square(field.x, field.y) == 0)
                        {
                            result = state.move(highlighted.x, highlighted.y, field.x, field.y);
                            switch(result)
                            {
                                //0 - incorrect move, no action neccessary, so no case
                                case 1://end turn;
                                    field.id = highlighted.id;
                                    field.Image = highlighted.Image;
                                    highlighted.Image = Properties.Resources.field;
                                    highlighted.id = 0;
                                    highlighted.FlatAppearance.BorderSize = 0;
                                    highlighted.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                                    phase = 0;
                                    result = state.check_if_win();
                                    if (result == state.active_side)
                                    {

                                    }
                                        state.active_side = 3 - state.active_side;                                    
                                    break;
                                case 2://remove captured pawn and end turn
                                    field.id = highlighted.id;
                                    field.Image = highlighted.Image;
                                    highlighted.Image = Properties.Resources.field;
                                    highlighted.id = 0;
                                    highlighted.FlatAppearance.BorderSize = 0;
                                    ((DraughtsButton)this.Controls.Find(state.to_kill, false)[0]).Image = Properties.Resources.field;
                                    highlighted.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                                    phase = 0;
                                    state.active_side = 3 - state.active_side;
                                    break;
                                case 3://further captures possible
                                    field.id = highlighted.id;
                                    field.Image = highlighted.Image;
                                    field.FlatAppearance.BorderColor = Color.DarkGreen;
                                    field.FlatAppearance.BorderSize = 2;
                                    highlighted.Image = Properties.Resources.field;
                                    ((DraughtsButton)this.Controls.Find(state.to_kill, false)[0]).Image = Properties.Resources.field;
                                    highlighted.id = 0;
                                    highlighted.FlatAppearance.BorderSize = 0;
                                    highlighted.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                                    highlighted = field;
                                    phase = 2;
                                    break;
                            }
                        }
                    }
                    break;
                case 2:
                    result = state.move(highlighted.x, highlighted.y, field.x, field.y);
                    switch (result)
                    {
                        //0,1 - incorrect move, no action neccessary, so no case
                        case 2://remove captured pawn and end turn
                            field.id = highlighted.id;
                            field.Image = highlighted.Image;
                            highlighted.Image = Properties.Resources.field;
                            highlighted.id = 0;
                            highlighted.FlatAppearance.BorderSize = 0;
                            highlighted.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                            phase = 0;
                            state.active_side = 3 - state.active_side;
                            ((DraughtsButton)this.Controls.Find(state.to_kill, false)[0]).Image = Properties.Resources.field;
                            break;
                        case 3://further captures possible
                            field.id = highlighted.id;
                            field.Image = highlighted.Image;
                            field.FlatAppearance.BorderColor = Color.DarkGreen;
                            field.FlatAppearance.BorderSize = 2;
                            highlighted.Image = Properties.Resources.field;
                            ((DraughtsButton)this.Controls.Find(state.to_kill, false)[0]).Image = Properties.Resources.field;
                            highlighted.id = 0;
                            highlighted.FlatAppearance.BorderSize = 0;
                            highlighted.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                            highlighted = field;
                            phase = 2;
                            break;
                    }
                    break;
            }
        }
    }
}
