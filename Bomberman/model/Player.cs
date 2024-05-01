using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bomberman.model.Base;
using System.Windows.Controls;



namespace Bomberman.model
{
    class Player: Entity
    {
        private string path;
        private double numberImg = 0;
        private double speedAnimation = 0.25;
        private int cellSize;
        public override double Speed { get; set; } = 3;
        public override string Diraction { get; set; }
        public override double Left { get; set; }
        public override double Top { get; set; }
        public override double Right { get; set; }
        public override double Bottom { get; set; }
        public Player() { }
        public Player(double left, double top, double right, double bottom, int cellSize)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
            this.cellSize = cellSize;
        }
        public override bool CheckIntersection(double left, double top, bool enemy = false)
        {
            if (Math.Abs(left - Left) < cellSize && Math.Abs(Top - top) < cellSize)
                    return true;
            return false;
        }
        public override void Dead()
        {
            throw new NotImplementedException();
        }

        public override string Update()
        {
            path = "../data/player/move";
            switch (Diraction)
            {
                case "up":
                    path += "!Y";
                    break;
                case "down":
                    path += "Y";
                    break;
                case "left":
                case "right":
                    path += "X";
                    break;
            }
            path += $"/{(int)numberImg}.png";
            numberImg += speedAnimation;
            numberImg %= 2;
            return path;

        }
    }
}
