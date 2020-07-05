using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _3_match
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Game game = new Game() { MenuForm = this };
            game.Show();
            this.Hide();
        }

        private void Menu_Load(object sender, EventArgs e)
        {
        }
    }
}
