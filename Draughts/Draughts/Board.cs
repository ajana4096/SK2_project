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
        Menu parent;
        public Board(Menu main)
        {
            parent = main;
            InitializeComponent();
        }

        private void Board_Load(object sender, EventArgs e)
        {
            this.BackgroundImage = Properties.Resources.background;
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }
        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            parent.Show();
        }
    }
}
