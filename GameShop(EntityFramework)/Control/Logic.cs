using GameShop_EntityFramework_.Control;
using GameShop_EntityFramework_.View;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameShop_EntityFramework_.Model
{
    public class Logic
    {
        //Метод, определяющий, выбрана ли одна строчка с ДатаГридов. Вызывается обеими ДатаГридами
        public void RowSelected(Form1 form1, bool mode)
        {
            DataGridView dataGrid;
            //Если событие вызвал главный ДатаГрид, то работа будет происходить с ним
            if (mode)
            {
                dataGrid = form1.Controls["dataGridView1"] as DataGridView;
                //Кнопка удаления неактивна, ведь удаление можно произвести только после поиска
                form1.Controls["button2"].Enabled = false;
            }
            //Если событие вызвал второй ДатаГрид
            else
                dataGrid = form1.Controls["dataGridView2"] as DataGridView;

            //Если выбрана строчка и выбрана только одна, то её можно изменить
            if (dataGrid.SelectedRows.Count > 0 &&
                 dataGrid.SelectedRows.Count <= 1)
            {
                form1.Controls["label3"].Text = "Изменить";
                //Если предыдущее условие выполняется для второго датагрида, то строчку можно удалить
                if (!mode)
                    form1.Controls["button2"].Enabled = true;

                //При выборе строк в ДатаГриде выбранные строки записываются в спец. массив. Поскольку строчка будет только одна, то она будет первой в массиве
                DataGridViewRow selectedRow = dataGrid.SelectedRows[0];

                //Запись данных в элементы формы для дальнейшего изменения
                form1.Controls["textBox1"].Text = selectedRow.Cells[2].Value.ToString();
                form1.Controls["textBox2"].Text = selectedRow.Cells[3].Value.ToString();
                (form1.Controls["numericUpDown1"] as NumericUpDown).Value = Convert.ToDecimal(selectedRow.Cells[4].Value);

                if (Convert.ToBoolean(selectedRow.Cells[5].Value))
                    (form1.Controls["comboBox1"] as ComboBox).SelectedIndex = 1;
                else
                    (form1.Controls["comboBox1"] as ComboBox).SelectedIndex = 0;

                (form1.Controls["comboBox2"] as ComboBox).SelectedIndex = Convert.ToInt32(selectedRow.Cells[1].Value) - 1;
                (form1.Controls["dateTimePicker1"] as DateTimePicker).Value = Convert.ToDateTime(selectedRow.Cells[6].Value);
            }
            //Иначе можно только добавить строчку
            else
            {
                form1.Controls["label3"].Text = "Добавить";

                //Возврат элементов формы в начальное состояние
                form1.Controls["textBox1"].Text = string.Empty;
                form1.Controls["textBox2"].Text = string.Empty;
                (form1.Controls["numericUpDown1"] as NumericUpDown).Value = 0;
                (form1.Controls["comboBox1"] as ComboBox).SelectedIndex = 0;
                (form1.Controls["comboBox2"] as ComboBox).SelectedIndex = 0;
                (form1.Controls["dateTimePicker1"] as DateTimePicker).Value = DateTime.Today;
            }
        }

        //Метод сохранения изменений. Вызывается кнопкой сохранения изменений
        public void SaveChanges(Form1 form1)
        {
            //Если установлен режим изменения
            if (form1.Controls["label3"].Text == "Изменить")
            {
                //Получение индекса текущей(выбранной) строки
                int index = (form1.Controls["dataGridView1"] as DataGridView).CurrentCell.RowIndex;

                //Изменение записи в ДБСете по индексу (можно и без Local)
                Communication.db.Games.Local.ToList()[index].Game_Name = form1.Controls["textBox1"].Text;
                Communication.db.Games.Local.ToList()[index].Game_Studio = form1.Controls["textBox2"].Text;
                Communication.db.Games.Local.ToList()[index].Game_SoldAmount = Convert.ToInt32((form1.Controls["numericUpDown1"] as NumericUpDown).Value);
                Communication.db.Games.Local.ToList()[index].Game_IsMultiplayer = Convert.ToBoolean((form1.Controls["comboBox1"] as ComboBox).SelectedIndex);
                Communication.db.Games.Local.ToList()[index].Game_StyleId = (form1.Controls["comboBox2"] as ComboBox).SelectedIndex + 1;
                Communication.db.Games.Local.ToList()[index].Game_ReleaseDate = (form1.Controls["dateTimePicker1"] as DateTimePicker).Value;

                form1.Controls["dataGridView2"].Refresh();
                Communication.isChange = true;
            }
            //Если установлен режим добавления
            else
            {
                //Добавление новой записи с данными из элементов формы
                Communication.db.Games.Local.Add(new Game
                {
                    Game_Name = form1.Controls["textBox1"].Text,
                    Game_Studio = form1.Controls["textBox2"].Text,
                    Game_SoldAmount = Convert.ToInt32((form1.Controls["numericUpDown1"] as NumericUpDown).Value),
                    Game_IsMultiplayer = Convert.ToBoolean((form1.Controls["comboBox1"] as ComboBox).SelectedIndex),
                    Game_StyleId = (form1.Controls["comboBox2"] as ComboBox).SelectedIndex + 1,
                    Game_ReleaseDate = (form1.Controls["dateTimePicker1"] as DateTimePicker).Value
                });
            }

            //Local получает коллекцию DbSet, которая содержит в себе все строки с БД + изменённые строки (удалённые, добавленные и т. д.)
            (form1.Controls["dataGridView1"] as DataGridView).DataSource = Communication.db.Games.Local.ToList();
            /*Сохранение изменений на уровне БД. Добавление записей не может быть откатываемым (при закрытии формы предлагается отмена изменений),
              поскольку записи выдаётся айди по-умолчанию, а настоящий присваивается только после закрепления изменений. Айди по-умолчанию нарушает
              работу с записями на уровне C#*/
            Communication.db.SaveChanges();
            form1.Controls["dataGridView1"].Refresh();
            form1.Controls["dataGridView1"].Update();
        }

        //Метод создания формы поиска. Вызывается пунктами меню "Поиск"
        public void FindForm(UInt16 mode) => new FormFind(mode).ShowDialog();

        //Метод поиска игр. Вызывается кнопкой поиска с формы поиска
        public void Find(UInt16 mode, FormFind formFind)
        {
            //В зависимости от режима производится поиск по какому-либо критерию
            switch (mode)
            {
                case 1:
                    {
                        /*Коллекцию DbSet при некоторых запросах Linq нужно брать, как перечисляемую коллекцию, иначе будет ошибка "Linq to Entity не знает этой команды".
                         (а именно взаимодействие с элементами управления формы). Если нет возможности представить коллекцию, как перечисляемую, то работу с элементами
                         формы нужно вынести за предел запроса Linq, как указано в закомментированном примере ниже*/
                        Communication.found_games =
                            Communication.db.Games.Local.AsEnumerable().Where(x => x.Game_Name.ToLower().Contains(formFind.Controls["textBox1"].Text.ToLower())).ToList();
                        //string test = formFind.Controls["textBox1"].Text.ToLower();
                        //Communication.found_games =
                        //    Communication.db.Games.Where(x => x.Game_Name.ToLower().Contains(test)).ToList();
                        break;
                    }
                case 2:
                    {
                        Communication.found_games =
                            Communication.db.Games.Local.AsEnumerable().Where(x => x.Game_Studio.ToLower().Contains(formFind.Controls["textBox2"].Text.ToLower())).ToList();
                        break;
                    }
                case 3:
                    {
                        Communication.found_games =
                            Communication.db.Games.Local.AsEnumerable().Where(x => x.Game_Name.ToLower().Contains(formFind.Controls["textBox1"].Text.ToLower()) && 
                                                                                x.Game_Studio.ToLower().Contains(formFind.Controls["textBox2"].Text.ToLower())).ToList();
                        break;
                    }
                case 4:
                    {
                        Communication.found_games =
                            Communication.db.Games.Local.AsEnumerable().Where(x => x.Game_StyleId == (formFind.Controls["comboBox1"] as ComboBox).SelectedIndex + 1).ToList();
                        break;
                    }
                case 5:
                    {
                        Communication.found_games =
                            Communication.db.Games.Local.AsEnumerable().Where(x => x.Game_ReleaseDate.Year == (formFind.Controls["numericUpDown1"] as NumericUpDown).Value).ToList();
                        break;
                    }
            }
            formFind.Dispose();
            //Обновление ДатаГрида поиска
            Communication.dataGrid.DataSource = Communication.found_games;
            Communication.dataGrid.Update();
            Communication.dataGrid.Refresh();
        }

        //Перегрузка метода поиска, вызываемая пунктами меню "Поиск по критерию", которые не требуют дополнительной формы
        public void Find(UInt16 mode)
        {
            //В зависимости от режима...
            switch (mode)
            {
                case 6:
                    {
                        Communication.found_games =
                            Communication.db.Games.Local.AsEnumerable().Where(x => !x.Game_IsMultiplayer).ToList();
                        break;
                    }
                case 7:
                    {
                        Communication.found_games =
                            Communication.db.Games.Local.AsEnumerable().Where(x => x.Game_IsMultiplayer).ToList();
                        break;
                    }
                case 8:
                    {
                        Communication.found_games =
                            Communication.db.Games.Local.AsEnumerable().Where(x => x.Game_SoldAmount == 
                                Communication.db.Games.Local.AsEnumerable().Max(y => y.Game_SoldAmount)).ToList();
                        break;
                    }
                case 9:
                    {
                        Communication.found_games =
                            Communication.db.Games.Local.AsEnumerable().Where(x => x.Game_SoldAmount ==
                                Communication.db.Games.Local.AsEnumerable().Min(y => y.Game_SoldAmount)).ToList();
                        break;
                    }
                case 10:
                    {
                        List<Game> topGames =
                            Communication.db.Games.Local.AsEnumerable().OrderBy(x => x.Game_SoldAmount).ToList();
                        topGames.Reverse();

                        Communication.found_games = topGames.Take(3).ToList();
                        break;
                    }
                case 11:
                    {
                        List<Game> topGames =
                            Communication.db.Games.Local.AsEnumerable().OrderBy(x => x.Game_SoldAmount).ToList();

                        Communication.found_games = topGames.Take(3).ToList();
                        break;
                    }
            }
            Communication.dataGrid.DataSource = Communication.found_games;
            Communication.dataGrid.Update();
            Communication.dataGrid.Refresh();
        }

        //Метод, вызываемый событием закрытия формы
        public void MainFormClosing()
        {
            //Если изменений не было (тех, которые можно отменить), то программа просто закрывается. В другом случае есть выбор отмены изменений
            if (Communication.isChange && 
                MessageBox.Show("Сохранить изменения в БД?", "Сохранение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                Communication.db.SaveChanges();
        }

        //Метод удаления записи, вызываемый кнопкой удаления
        public void Delete(DataGridView dataGrid, int id)
        {
            if (MessageBox.Show("Вы точно хотите удалить игру?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                //Нельзя изменять коллекцию, пока она привязана к ДатаГриду. Поэтому сначала происходит отвязка, модификация списка и привязка заново.
                Communication.dataGrid.DataSource = null;
                Communication.found_games.Remove(Communication.found_games.First(x => x.Game_Id == id));
                Communication.dataGrid.DataSource = Communication.found_games;

                dataGrid.DataSource = null;
                Communication.db.Games.Remove(Communication.db.Games.First(x => x.Game_Id == id));
                dataGrid.DataSource = Communication.db.Games.Local.ToList();
                
                dataGrid.Refresh();
                Communication.dataGrid.Refresh();

                Communication.isChange = true;
            }
        }
    }
}
