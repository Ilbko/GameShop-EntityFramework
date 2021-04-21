using GameShop_EntityFramework_.Control;
using GameShop_EntityFramework_.View;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameShop_EntityFramework_.Model
{
    public class Logic
    {
        public void RowSelected(Form1 form1)
        {
            if ((form1.Controls["dataGridView1"] as DataGridView).SelectedRows.Count > 0 &&
                 (form1.Controls["dataGridView1"] as DataGridView).SelectedRows.Count <= 1)
            {
                DataGridViewRow selectedRow = (form1.Controls["dataGridView1"] as DataGridView).SelectedRows[0];

                form1.Controls["label4"].Visible = true;
                form1.Controls["label5"].Visible = true;
                form1.Controls["label6"].Visible = true;
                form1.Controls["label7"].Visible = true;
                form1.Controls["label8"].Visible = true;
                form1.Controls["label9"].Visible = true;
                form1.Controls["button1"].Visible = true;
                form1.Controls["label3"].ForeColor = Color.GreenYellow;

                form1.Controls["textBox1"].Visible = true;
                form1.Controls["textBox2"].Visible = true;
                form1.Controls["numericUpDown1"].Visible = true;
                form1.Controls["comboBox1"].Visible = true;
                form1.Controls["comboBox2"].Visible = true;
                form1.Controls["dateTimePicker1"].Visible = true;

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
                form1.Controls["label4"].Visible = false;
                form1.Controls["label5"].Visible = false;
                form1.Controls["label6"].Visible = false;
                form1.Controls["label7"].Visible = false;
                form1.Controls["label8"].Visible = false;
                form1.Controls["label9"].Visible = false;
                form1.Controls["button1"].Visible = false;
                form1.Controls["label3"].ForeColor = Color.LightGray;

                form1.Controls["textBox1"].Visible = false;
                form1.Controls["textBox2"].Visible = false;
                form1.Controls["numericUpDown1"].Visible = false;
                form1.Controls["comboBox1"].Visible = false;
                form1.Controls["comboBox2"].Visible = false;
                form1.Controls["dateTimePicker1"].Visible = false;
            }
        }

        public void SaveChanges(Form1 form1)
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

            form1.Controls["dataGridView1"].Refresh();

            Communication.isChange = true;
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
                            Communication.db.Games.AsEnumerable().Where(x => x.Game_Name.ToLower().Contains(formFind.Controls["textBox1"].Text.ToLower())).ToList(); 
                        //Communication.found_games =
                        //    Communication.db.Games.Where(x => x.Game_Name.ToLower().Contains(test)).ToList();
                        break;
                    }
                case 2:
                    {
                        Communication.found_games =
                            Communication.db.Games.AsEnumerable().Where(x => x.Game_Studio.ToLower().Contains(formFind.Controls["textBox2"].Text.ToLower())).ToList();
                        break;
                    }
                case 3:
                    {
                        Communication.found_games =
                            Communication.db.Games.AsEnumerable().Where(x => x.Game_Name.ToLower().Contains(formFind.Controls["textBox1"].Text.ToLower()) && 
                                                                                x.Game_Studio.ToLower().Contains(formFind.Controls["textBox2"].Text.ToLower())).ToList();
                        break;
                    }
                case 4:
                    {
                        Communication.found_games =
                            Communication.db.Games.AsEnumerable().Where(x => x.Game_StyleId == (formFind.Controls["comboBox1"] as ComboBox).SelectedIndex + 1).ToList();
                        break;
                    }
                case 5:
                    {
                        Communication.found_games =
                            Communication.db.Games.AsEnumerable().Where(x => x.Game_ReleaseDate.Year == (formFind.Controls["numericUpDown1"] as NumericUpDown).Value).ToList();
                        break;
                    }
            }
            formFind.Dispose();
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
    }
}
