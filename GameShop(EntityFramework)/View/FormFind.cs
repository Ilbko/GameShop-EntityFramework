using GameShop_EntityFramework_.Control;
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

namespace GameShop_EntityFramework_.View
{
    //Форма поиска игр
    public partial class FormFind : Form
    {
        Logic logic = new Logic();
        //Режим поиска (по названию игры, по названию студии...)
        UInt16 mode;
        public FormFind()
        {
            InitializeComponent();
        }

        public FormFind(UInt16 mode)
        {
            InitializeComponent();
            this.mode = mode;

            //В зависимости от режима становятся активными соответствующие поля окна
            switch (mode)
            {
                case 1:
                    {
                        this.label2.ForeColor = Color.Aquamarine;
                        this.textBox1.Enabled = true;
                        break;
                    }
                case 2:
                    {
                        this.label3.ForeColor = Color.Aquamarine;
                        this.textBox2.Enabled = true;
                        break;
                    }
                case 3:
                    {
                        this.label2.ForeColor = Color.Aquamarine;
                        this.textBox1.Enabled = true;
                        this.label3.ForeColor = Color.Aquamarine;
                        this.textBox2.Enabled = true;
                        break;
                    }
                case 4:
                    {
                        //using (GameModel model = new GameModel())
                        //{
                        //    foreach (var item in model.Styles)
                        //    {
                        //        this.comboBox1.Items.Add(item.Style_Name);
                        //    }
                        //}
                        foreach (var item in Communication.db.Styles)
                        {
                            this.comboBox1.Items.Add(item.Style_Name);
                        }

                        this.comboBox1.SelectedIndex = 0;
                        this.label4.ForeColor = Color.Aquamarine;
                        this.comboBox1.Enabled = true;
                        break;
                    }
                case 5:
                    {
                        this.numericUpDown1.Value = this.numericUpDown1.Maximum = DateTime.Now.Year;

                        this.label5.ForeColor = Color.Aquamarine;
                        this.numericUpDown1.Enabled = true;
                        break;
                    }
            }
        }

        //Событие нажатия на кнопку поиска
        private void button1_Click(object sender, EventArgs e) => logic.Find(mode, this);
    }
}
