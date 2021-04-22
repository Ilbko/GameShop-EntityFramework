using GameShop_EntityFramework_.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameShop_EntityFramework_.Control
{
    //Класс для общения между окном поиска и главным окном
    internal static class Communication
    {
        //Список найденных игр по поиску
        public static List<Game> found_games = new List<Game>();
        //Модель EntityFramework
        public static GameModel db = new GameModel();
        //Второй ДатаГрид, предназначенный для отображения найденных игр
        public static DataGridView dataGrid;
        //Было ли изменение в БД (при любом действии, кроме добавления, можно откатиться при закрытии окна)
        public static bool isChange = false;
    }
}
