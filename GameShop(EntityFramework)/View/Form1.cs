using GameShop_EntityFramework_.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameShop_EntityFramework_
{
    public partial class Form1 : Form
    {
        Logic logic = new Logic();
        List<Game> games;
        public Form1()
        {
            InitializeComponent();
            using (GameModel model = new GameModel())
            {
                //dataGridView1.DataSource = model.Games.ToList();
                dataGridView1.DataSource = games = model.Games.ToList();

                foreach(var item in model.Styles)
                    comboBox2.Items.Add(item.Style_Name);
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e) => logic.RowSelected(this);

        private void button1_Click(object sender, EventArgs e) => logic.SaveChanges(this, games);

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
