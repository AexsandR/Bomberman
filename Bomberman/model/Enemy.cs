using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Bomberman.model.Base;

namespace Bomberman.model
{
    public class Enemy: Entity
    {
        private DateTime lastChangeDiraction;
        private Random rdn = new();
        private string path;
        private double numberImg = 0;
        private double speedAnimation = 0.1;
        private string[] diractions = { "up", "down", "left", "right" };
        private int timeChangeDiraction = (new Random()).Next(5,15);
        private int cellSize;
        private string? newDiractions = null;
        public override double Speed { get; set; } = 1;
        public override double Left { get; set; }
        public override double Top { get; set; }
        public override double Right { get; set; }
        public override double Bottom { get; set; }

        public override string Diraction { get; set; } = "left";
        public Enemy (double left, double top, double right, double bottom, int cellSize)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
            lastChangeDiraction = DateTime.Now;
            this.cellSize = cellSize;
        }
        public override bool CheckIntersection(double left, double top, bool enemy = false)
        {
            if(Math.Abs(left - Left) < cellSize && Math.Abs(Top - top) < cellSize)
            {
                int index = Array.IndexOf(diractions, Diraction);
                if (index % 2 == 1)
                    index -= 2;

                Diraction = diractions[++index];
                return true;
            }
            return false;
        }

        private bool CheckFreeCell(char[][] map, int x, int y, string diraction)
        {
            if (diraction == "up" || diraction == "down") 
            { 
                if (map[y + 1][x] != '#' && map[y + 1][x] != '@' || map[y - 1][x] != '#' && map[y - 1][x] != '@')
                    return true;
            }
            else
            {
                if (map[y][x + 1] != '#' && map[y][x + 1] != '@' || map[y][x - 1] != '#' && map[y][x - 1] != '@')
                    return true;
            }

            return false;
        }
        public void SwapRdnDiraction(char[][] map)
        {
            if ((DateTime.Now - lastChangeDiraction).Seconds >= timeChangeDiraction)
            {
                if (Left % cellSize <= 0.2 && Top % cellSize <= 0.2)
                {
                    if(newDiractions is null)
                        newDiractions = diractions[rdn.Next(diractions.Length)];
                    Left = (int)Left / cellSize * cellSize;
                    Top = (int)Top / cellSize * cellSize;
                    if(CheckFreeCell(map, (int)Left / cellSize, (int)Top / cellSize, newDiractions))
                    {
                        Diraction = newDiractions;
                        newDiractions = null;
                        lastChangeDiraction = DateTime.Now;
                        timeChangeDiraction = rdn.Next(5, 15);
                    }

                    
                }
            }
        }
        public override void Dead()
        {
            throw new NotImplementedException();
        }

        public override string Update()
        {
            path = "../data/enemy/Valcom/move/";
            path += $"{(int)numberImg}.png";
            numberImg += speedAnimation;
            numberImg %= 2;
            return path;
        }
    }
}
