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
    public partial class Settings : Form
    {
        Menu parent;
        Options o;
        public Settings(Menu m, Options in_options)
        {
            parent = m;
            o = in_options;
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            this.BackgroundImage = Properties.Resources.background;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            if (o.singlemode == true)
            {
                this.button1.Text = "Gra lokalnie";
                this.textBox1.Hide();
                this.textBox2.Hide();
                this.textBox3.Hide();
                this.label2.Hide();
                this.label3.Hide();
                this.label4.Hide();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void Settings_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            parent.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (o.singlemode == false)
            {
                o.singlemode = true;
                this.button1.Text = "Gra lokalnie";
                this.textBox1.Hide();
                this.textBox2.Hide();
                this.textBox3.Hide();
                this.label2.Hide();
                this.label3.Hide();
                this.label4.Hide();
            }
            else
            {
                o.singlemode = false;
                this.button1.Text = "Gra poprzez sieć";                
                this.textBox1.Show();
                this.textBox2.Show();
                this.textBox3.Show();
                this.label2.Show();
                this.label3.Show();
                this.label4.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            parent.Show();
            this.Dispose();
        }

        private void Settings_FormClosed(object sender, FormClosedEventArgs e)
        {
            parent.Show();
        }
    }
}
