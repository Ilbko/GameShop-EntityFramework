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
        public void RowSelected(Form1 form1, bool mode)
        {
            DataGridView dataGrid;
            if (mode)
            {
                dataGrid = form1.Controls["dataGridView1"] as DataGridView;
                form1.Controls["button2"].Enabled = false;
            }
            else
                dataGrid = form1.Controls["dataGridView2"] as DataGridView;

            if (dataGrid.SelectedRows.Count > 0 &&
                 dataGrid.SelectedRows.Count <= 1)
            {
                form1.Controls["label3"].Text = "Изменить";
                if (!mode)
                    form1.Controls["button2"].Enabled = true;

                DataGridViewRow selectedRow = dataGrid.SelectedRows[0];

                //form1.Controls["label4"].Visible = true;
                //form1.Controls["label5"].Visible = true;
                //form1.Controls["label6"].Visible = true;
                //form1.Controls["label7"].Visible = true;
                //form1.Controls["label8"].Visible = true;
                //form1.Controls["label9"].Visible = true;
                //form1.Controls["button1"].Visible = true;

                //form1.Controls["textBox1"].Visible = true;
                //form1.Controls["textBox2"].Visible = true;
                //form1.Controls["numericUpDown1"].Visible = true;
                //form1.Controls["comboBox1"].Visible = true;
                //form1.Controls["comboBox2"].Visible = true;
                //form1.Controls["dateTimePicker1"].Visible = true;

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
            else
            {
                //form1.Controls["label4"].Visible = false;
                //form1.Controls["label5"].Visible = false;
                //form1.Controls["label6"].Visible = false;
                //form1.Controls["label7"].Visible = false;
                //form1.Controls["label8"].Visible = false;
                //form1.Controls["label9"].Visible = false;
                //form1.Controls["button1"].Visible = false;
                form1.Controls["label3"].Text = "Добавить";

                form1.Controls["textBox1"].Text = string.Empty;
                form1.Controls["textBox2"].Text = string.Empty;
                (form1.Controls["numericUpDown1"] as NumericUpDown).Value = 0;
                (form1.Controls["comboBox1"] as ComboBox).SelectedIndex = 0;
                (form1.Controls["comboBox2"] as ComboBox).SelectedIndex = 0;
                (form1.Controls["dateTimePicker1"] as DateTimePicker).Value = DateTime.Today;
            }
        }

        public void SaveChanges(Form1 form1)
        {
            if (form1.Controls["label3"].Text == "Изменить")
            {
                int index = (form1.Controls["dataGridView1"] as DataGridView).CurrentCell.RowIndex;

                //games[index].Game_Name = form1.Controls["textBox1"].Text;
                //games[index].Game_Studio = form1.Controls["textBox2"].Text;
                //games[index].Game_SoldAmount = Convert.ToInt32((form1.Controls["numericUpDown1"] as NumericUpDown).Value);
                //games[index].Game_IsMultiplayer = Convert.ToBoolean((form1.Controls["comboBox1"] as ComboBox).SelectedIndex);
                //games[index].Game_StyleId = (form1.Controls["comboBox2"] as ComboBox).SelectedIndex + 1;
                //games[index].Game_ReleaseDate = (form1.Controls["dateTimePicker1"] as DateTimePicker).Value;
                Communication.db.Games.ToList()[index].Game_Name = form1.Controls["textBox1"].Text;
                Communication.db.Games.ToList()[index].Game_Studio = form1.Controls["textBox2"].Text;
                Communication.db.Games.ToList()[index].Game_SoldAmount = Convert.ToInt32((form1.Controls["numericUpDown1"] as NumericUpDown).Value);
                Communication.db.Games.ToList()[index].Game_IsMultiplayer = Convert.ToBoolean((form1.Controls["comboBox1"] as ComboBox).SelectedIndex);
                Communication.db.Games.ToList()[index].Game_StyleId = (form1.Controls["comboBox2"] as ComboBox).SelectedIndex + 1;
                Communication.db.Games.ToList()[index].Game_ReleaseDate = (form1.Controls["dateTimePicker1"] as DateTimePicker).Value;

                form1.Controls["dataGridView2"].Refresh();
                Communication.isChange = true;
            }
            else
            {
                Communication.db.Games.Add(new Game
                {
                    Game_Name = form1.Controls["textBox1"].Text,
                    Game_Studio = form1.Controls["textBox2"].Text,
                    Game_SoldAmount = Convert.ToInt32((form1.Controls["numericUpDown1"] as NumericUpDown).Value),
                    Game_IsMultiplayer = Convert.ToBoolean((form1.Controls["comboBox1"] as ComboBox).SelectedIndex),
                    Game_StyleId = (form1.Controls["comboBox2"] as ComboBox).SelectedIndex + 1,
                    Game_ReleaseDate = (form1.Controls["dateTimePicker1"] as DateTimePicker).Value
                });
            }

            (form1.Controls["dataGridView1"] as DataGridView).DataSource = Communication.db.Games.Local.ToList();
            Communication.db.SaveChanges();
            form1.Controls["dataGridView1"].Refresh();
            form1.Controls["dataGridView1"].Update();
        }

        public void FindForm(UInt16 mode) => new FormFind(mode).ShowDialog();

        public void Find(UInt16 mode, FormFind formFind)
        {
            switch (mode)
            {
                case 1:
                    {
                        //string test = formFind.Controls["textBox1"].Text.ToLower();
                        Communication.found_games =
                            Communication.db.Games.Local.AsEnumerable().Where(x => x.Game_Name.ToLower().Contains(formFind.Controls["textBox1"].Text.ToLower())).ToList(); 
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
            Communication.dataGrid.DataSource = Communication.found_games;
            Communication.dataGrid.Update();
            Communication.dataGrid.Refresh();
        }

        public void Find(UInt16 mode)
        {
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

        public void MainFormClosing()
        {
            if (Communication.isChange && 
                MessageBox.Show("Сохранить изменения в БД?", "Сохранение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                Communication.db.SaveChanges();
        }

        public void Delete(DataGridView dataGrid, int id)
        {
            if (MessageBox.Show("Вы точно хотите удалить игру?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
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
