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
        public Settings(Menu m)
        {
            parent = m;
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            this.BackgroundImage = Properties.Resources.background;
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void Settings_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            parent.Show();
        }
    }
}
