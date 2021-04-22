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

            //Установка источника данных для главного ДатаГрида и ДатаГрида для отображения найденных игр
            //dataGridView1.DataSource = Communication.db.Games.ToList();

            //Для использования локальной коллекции ДБСета в качестве источника данных для ДатаГрида нужно сначала прогрузить коллекцию
            Communication.db.Games.Load();
            dataGridView1.DataSource = Communication.db.Games.Local.ToList();
            dataGridView2.DataSource = Communication.found_games;
            //Передача "по ссылке" в класс коммуникации второго ДатаГрида(поиска)
            Communication.dataGrid = dataGridView2;

            //Запись в комбобокс стилей игр
            foreach (var item in Communication.db.Styles)
                this.comboBox2.Items.Add(item.Style_Name);

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
        }

        //Событие изменения выделения в главном ДатаГриде
        private void dataGridView1_SelectionChanged(object sender, EventArgs e) => logic.RowSelected(this, true);

        //Нажатие на кнопку сохранения изменений
        private void button1_Click(object sender, EventArgs e) => logic.SaveChanges(this);

        //Поиск по названию игры
        private void GameToolStripMenuItem_Click(object sender, EventArgs e) => logic.FindForm(1);

        //Поиск по названию студии
        private void StudioToolStripMenuItem_Click(object sender, EventArgs e) => logic.FindForm(2);

        //Поиск по названию игры и студии
        private void GameAndStudioToolStripMenuItem_Click(object sender, EventArgs e) => logic.FindForm(3);

        //Поиск игры по стилю
        private void ByStyleToolStripMenuItem_Click(object sender, EventArgs e) => logic.FindForm(4);

        //Поиск игры по году релиза
        private void ByReleaseYearToolStripMenuItem_Click(object sender, EventArgs e) => logic.FindForm(5);

        //Событие закрытия формы
        private void Form1_FormClosing(object sender, FormClosingEventArgs e) => logic.MainFormClosing();

        //Событие изменения выделения в ДатаГриде найденных игр
        private void dataGridView2_SelectionChanged(object sender, EventArgs e) => logic.RowSelected(this, false);

        //Нажатие на кнопку удаления
        private void button2_Click(object sender, EventArgs e) => logic.Delete(dataGridView1, (int)dataGridView2.CurrentRow.Cells[0].Value);

        //Все однопользовательские игры
        private void AllSingleplayerToolStripMenuItem_Click(object sender, EventArgs e) => logic.Find(6);

        //Все многопользовательские игры
        private void AllMultiplayerToolStripMenuItem_Click(object sender, EventArgs e) => logic.Find(7);

        //Игра с макс. числом продаж
        private void MaxSoldToolStripMenuItem_Click(object sender, EventArgs e) => logic.Find(8);

        //Игра с мин. числом продаж
        private void MinSoldToolStripMenuItem_Click(object sender, EventArgs e) => logic.Find(9);

        //Топ-3 игр по числу продаж
        private void Top3BestToolStripMenuItem_Click(object sender, EventArgs e) => logic.Find(10);

        //Топ-3 худших игр по числу продаж
        private void Top3WorstToolStripMenuItem_Click(object sender, EventArgs e) => logic.Find(11);
    }
}
