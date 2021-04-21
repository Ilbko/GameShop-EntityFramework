using GameShop_EntityFramework_.Control;
using GameShop_EntityFramework_.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
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
        //List<Game> games;
        public Form1()
        {
            InitializeComponent();
            //using (GameModel model = new GameModel())
            //{
            //    //dataGridView1.DataSource = model.Games.ToList();
            //    dataGridView1.DataSource = games = model.Games.ToList();

            //    foreach(var item in model.Styles)
            //        comboBox2.Items.Add(item.Style_Name);
            //}

            dataGridView1.DataSource = Communication.db.Games.ToList();
            dataGridView2.DataSource = Communication.found_games;
            Communication.dataGrid = dataGridView2;

            foreach (var item in Communication.db.Styles)
                this.comboBox2.Items.Add(item.Style_Name);
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e) => logic.RowSelected(this, true);

        private void button1_Click(object sender, EventArgs e) => logic.SaveChanges(this);

        private void GameToolStripMenuItem_Click(object sender, EventArgs e) => logic.FindForm(1);

        private void StudioToolStripMenuItem_Click(object sender, EventArgs e) => logic.FindForm(2);

        private void GameAndStudioToolStripMenuItem_Click(object sender, EventArgs e) => logic.FindForm(3);

        private void ByStyleToolStripMenuItem_Click(object sender, EventArgs e) => logic.FindForm(4);
       
        private void ByReleaseYearToolStripMenuItem_Click(object sender, EventArgs e) => logic.FindForm(5);

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) => logic.MainFormClosing();

        private void dataGridView2_SelectionChanged(object sender, EventArgs e) => logic.RowSelected(this, false);

        private void button2_Click(object sender, EventArgs e) => logic.Delete(dataGridView1, (int)dataGridView2.CurrentRow.Cells[0].Value);
    }
}
