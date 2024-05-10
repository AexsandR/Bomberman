using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;


namespace Bomberman.model
{
    public class BoardGame
    {
        /*
         * # - неразрушаемая стена
         * @ - разрушаемая стена
         * 0 - Bomberman
         * 1 - враг
         * = - зона чтоб не появлись мобы и разрушаемые стены
         */

        private char[][] map = {
            "###############################".ToCharArray(),
            "#===                          #".ToCharArray(),
            "#=# # # # # # # # # # # # # # #".ToCharArray(),
            "#=                            #".ToCharArray(),
            "# # # # # # # # # # # # # # # #".ToCharArray(),
            "#                             #".ToCharArray(),
            "# # # # # # # # # # # # # # # #".ToCharArray(),
            "#                             #".ToCharArray(),
            "# # # # # # # # # # # # # # # #".ToCharArray(),
            "#                             #".ToCharArray(),
            "# #=# # # # # # # # # # # # # #".ToCharArray(),
            "# =0=                         #".ToCharArray(),
            "###############################".ToCharArray()
        };
        public char[][] Map { get { return map; } }

        private double percentBrick = 0.3;
        public BoardGame()
        {
            int numberBrics = (int)(CountFreeCell() * percentBrick);
            SpawnBrick(numberBrics);
            SpawnEnemy(6);
        }


        private int CountFreeCell()
        {
            int count = 0;
            foreach (char[] str in map)
            {
                count += str.Count(ch => ch == ' ');
            }
            return count;
        }
        private void SpawnBrick(int numberBrics)
        {
            var rdn = new Random();
            int number;
            int y;
            while(numberBrics != 0)
            {
                y = rdn.Next(1, map.Length - 1);
                number = rdn.Next(1, map[y].Count(ch => ch == ' ') + 1);
                map[y] = Replace(map[y], number, ' ', '@');
                numberBrics--;
            }
        }
        private void SpawnEnemy(int numberEnemy)
        {
            var rdn = new Random();
            int number;
            int y;
            while (numberEnemy != 0)
            {
                y = rdn.Next(1, map.Length - 1);
                number = rdn.Next(1, map[y].Count(ch => ch == ' ') + 1);
                map[y] = Replace(map[y], number, ' ', '1');
                numberEnemy--;
            }
        }
        
        private char[] Replace(char[] str, int num, char oldSymbol, char newSymbol)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == oldSymbol)
                    num--;
                if(num == 0)
                {
                    str[i] = newSymbol;
                    return str;
                }
            }
            return str;
        }
    }
}
