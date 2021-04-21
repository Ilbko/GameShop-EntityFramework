using GameShop_EntityFramework_.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameShop_EntityFramework_.Control
{
    internal static class Communication
    {
        public static List<Game> found_games = new List<Game>();
        public static GameModel db = new GameModel();
        public static DataGridView dataGrid;
        public static bool isChange = false;
    }
}
