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
    public partial class Endgame : Form
    {
        Board parent;
        public Endgame(Board b, bool victory)
        {
            parent = b;
            InitializeComponent();
            if (victory)
            {
                this.BackColor = Color.Green;
                this.label1.Text = "Zwycięstwo!\n(Fajnie, nie?)";
            }
            else
            {
                this.BackColor = Color.Red;
                this.label1.Text = "Przegrana...\n(Może następnym razem będzie lepiej..?)";
            }
        }
        public Endgame(Board b, bool victory, string text)
        {
            parent = b;
            InitializeComponent();
            if (victory)
            {
                this.BackColor = Color.Green;
                this.label1.Text = text;
            }
            else
            {
                this.BackColor = Color.Red;
                this.label1.Text = text;
            }
        }

        private void Endgame_FormClosed(object sender, FormClosedEventArgs e)
        {
            parent.Dispose();
            parent.parent.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            parent.parent.Show();
            parent.Dispose();
            this.Dispose();
        }
    }
}
