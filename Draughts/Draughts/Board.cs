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
    public partial class Board : Form
    {
        int size = 500;

        Menu parent;
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
            state = new Board_situation();
            state.new_game();
            Board_setup();
        }
        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            parent.Show();
        }
        private void Board_setup()
        {
        }
    }
}
