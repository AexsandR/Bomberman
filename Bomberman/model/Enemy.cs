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
        private string? newDiractions = null;
        public int Cost { get; set; } = 1000;
        private bool alive = true;
        private bool endDead = false;
        public bool EndDead => endDead;
        public override bool Alive
        {
            get { return alive; }
            set
            {
                if (value)
                {
                    numberImg = 0;
                    speedAnimation = 0.1;
                }
                else
                    if (value != alive)
                {
                    speedAnimation = 0.05;
                    numberImg = 0;
                }
                alive = value;
            }
        }
        public override double SizeSmallHitBox => Setting.SizeSmallHitBoxEnemy;
        public Enemy (double left, double top, double right, double bottom)
        {
            alive = true;
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
            Speed = 1;
            Diraction = "left";
            lastChangeDiraction = DateTime.Now;
        }
        public void SwapDiraction()
        {
            int index = Array.IndexOf(diractions, Diraction);
            if (index % 2 == 1)
                index -= 2;
            Diraction = diractions[++index];

        }
        public bool CheckFreeCell(char[][] map, int x, int y, string diraction)
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
                if (Left % Setting.CellSize <= 1 && Top % Setting.CellSize <= 1)
                {
                    if(newDiractions is null)
                        newDiractions = diractions[rdn.Next(diractions.Length)];

                    if (CheckFreeCell(map, (int)Left / Setting.CellSize, (int)Top / Setting.CellSize, newDiractions))
                    {
                        Left = (int)Left / Setting.CellSize * Setting.CellSize;
                        Top = (int)Top / Setting.CellSize * Setting.CellSize;
                        Diraction = newDiractions;
                        newDiractions = null;

                        lastChangeDiraction = DateTime.Now;
                        timeChangeDiraction = rdn.Next(5, 15);
                    }
                    
                }
            }
        }
        public override string Dead()
        {
            if (numberImg > 4)
            {
                endDead = true;
                return path;
            }
            path = $"../data/enemy/Valcom/dead/{(int)numberImg}.png";
            numberImg += speedAnimation;
            return path;
        }
   
        public override string Update()
        {
            if (!Alive)
                return path;
            path = "../data/enemy/Valcom/move/";
            path += $"{(int)numberImg}.png";
            numberImg += speedAnimation;
            numberImg %= 3;
            return path;
        }
    }
}
