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

        private char[][] map;
        public char[][] Map { get { return map; } }
        public int level = 1;

        private double percentBrick = 0.3;
        public BoardGame()
        {
            CreatePole();
        }
        /// <summary>
        /// создаёт поле с разрушаемыми блоками и врагами
        /// </summary>
        public void CreatePole()
        {
            Setting.CopyMapPole(out map);
            int numberBrics = (int)(CountFreeCell() * percentBrick);
            SpawnBrick(numberBrics);
            SpawnEnemy(8);
        }
        /// <summary>
        /// метод считает кол-во свободных клеток
        /// </summary>
        /// <returns></returns>
        private int CountFreeCell()
        {
            int count = 0;
            foreach (char[] str in map)
            {
                count += str.Count(ch => ch == ' ');
            }
            return count;
        }
        /// <summary>
        /// метод распределения разрушаемых блоков
        /// </summary>
        /// <param name="numberBrics"></param>
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
        /// <summary>
        /// метод распределения врагов
        /// </summary>
        /// <param name="numberEnemy"></param>
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
        /// <summary>
        /// метод замены
        /// </summary>
        /// <param name="arraySymbols">массив символов</param>
        /// <param name="num"> кол-во замен</param>
        /// <param name="oldSymbol"> старый символ</param>
        /// <param name="newSymbol"> новый символ</param>
        /// <returns></returns>

        private char[] Replace(char[] arraySymbols, int num, char oldSymbol, char newSymbol)
        {
            for (int i = 0; i < arraySymbols.Length; i++)
            {
                if (arraySymbols[i] == oldSymbol)
                    num--;
                if(num == 0)
                {
                    arraySymbols[i] = newSymbol;
                    return arraySymbols;
                }
            }
            return arraySymbols;
        }
    }
}
