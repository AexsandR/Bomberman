using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman.model
{
    static public class Setting
    {
        /*
         * константы для игры
        */
        public const int CellSize = 39;
        public const int CellSection = 30;
        public const int TimeBeforeFire = 3;
        public const int TimeFire = 1;
        public const int Tick = 16;
        public const int Bottom = 461;
        public const int Right = 761;
        public const int SizeSmallHitBoxPlayer = 10;
        public const int SizeSmallHitBoxEnemy = 20;
        private static char[][] map = {
            /*
             * 0 - игрок
             * # - стена
             * @ - кирпичи
             * 1 - бомбы
             * E - выход с уровня
             * = - помеченная област, чтоб не появлялись мобы и кирпичи
             */
            "###############################".ToCharArray(),
            "#0==                          #".ToCharArray(),
            "#=# # # # # # # # # # # # # # #".ToCharArray(),
            "#=                            #".ToCharArray(),
            "# # # # # # # # # # # # # # # #".ToCharArray(),
            "#                             #".ToCharArray(),
            "# # # # # # # # # # # # # # # #".ToCharArray(),
            "#                             #".ToCharArray(),
            "# # # # # # # # # # # # # # # #".ToCharArray(),
            "#                             #".ToCharArray(),
            "# # # # # # # # # # # # # # # #".ToCharArray(),
            "#                             #".ToCharArray(),
            "###############################".ToCharArray()
        };
        /// <summary>
        /// метод который копирует поле карты
        /// </summary>
        /// <param name="array"></param>
        public static void CopyMapPole(out char[][] array)
        {
            array = new char[map.Length][];
            for(int i = 0; i < map.Length; i++)
            {
                array[i] = new char[map[i].Length];
                for (int j = 0; j < map[i].Length; j++)
                    array[i][j] = map[i][j];

            }
        }

    }
}
