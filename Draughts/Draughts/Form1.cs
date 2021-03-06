﻿using System;
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
    public partial class Menu : Form
    {
        Options o = new Options();
        public Menu()
        {
            this.BackgroundImage = Properties.Resources.background;
            this.BackgroundImageLayout = ImageLayout.Stretch;           
            InitializeComponent();
        }
        //Start game
        private void button1_Click(object sender, EventArgs e)
        {            
            Board b = new Board(this, o);
            ToggleWait(true);
            this.Hide();
            b.Show();            
        }
        public void ToggleWait(bool show)
        {
            if(show)
            {
                this.label1.Show();
            }
            else
            {
                this.label1.Hide();
            }
        }
        //connection to the server
        private void button2_Click(object sender, EventArgs e)
        {
            Settings s = new Settings(this, o);
            this.Hide();
            s.Show();
        }
        //Exit
        private void button3_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}
