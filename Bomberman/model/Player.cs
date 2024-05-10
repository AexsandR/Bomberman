using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bomberman.model.Base;
using System.Windows.Controls;
using System.Windows;



namespace Bomberman.model
{
    class Player: Entity
    {
        private string path;
        private double numberImg = 0;
        private double speedAnimation = 0.25;
        public override double Speed { get; set; } = 4;
        public override string Diraction { get; set; }
        public override double Left { get; set; }
        public override double Top { get; set; }
        public override bool Alive { get; set; } = true;

        public override double Right { get; set; }
        public override double Bottom { get; set; }
        public Player() { }
        public Player(double left, double top, double right, double bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }
        private void fixationCell()
        {
            switch (Diraction) { 
                case "up":
                    Bottom -= Speed;
                    Top += Speed;
                    break;
                case "down":
                    Bottom += Speed;
                    Top -= Speed;
                    break;
                case "left":
                    Left += Speed;
                    Right -= Speed;
                    break;
                case "right":
                    Left -= Speed;
                    Right += Speed;
                    break;
            }
        }
        private void fixationCell(double left, double top)
        {
            switch (Diraction)
            {
                case "down":
                case "up":
                    if(Left > left)
                    {
                        Right -= (Setting.CellSize - Left + left);
                        Left += (Setting.CellSize - Left + left);
                    }
                    else
                    {
                        Right += (Left - (left - Setting.CellSize));
                        Left -= Left - (left - Setting.CellSize);
                    }
                    break;
                case "left":
                case "right":
                    if (Top < top) // наша точка находиться выше чем обьекта которого мы задели
                    {
                        Bottom += Top - (top - Setting.CellSize);
                        Top -= Top - (top - Setting.CellSize);
                    }
                    else // тут ниже
                    {
                        Bottom -= Setting.CellSize - (Top - top);
                        Top += Setting.CellSize - (Top - top);
                    }
                    break;
            }
        }
        public override bool CheckIntersection(double left, double top, bool enemy = false)
        {
            if (Math.Abs(left - Left) < Setting.CellSize && Math.Abs(Top - top) < Setting.CellSize)
            {

                if (Math.Abs(left - Left) <= Setting.CellSize && Math.Abs(left - Left) >= Setting.CellSection &&
                    Math.Abs(Top - top) <= Setting.CellSize && Math.Abs(Top - top) >= Setting.CellSection && !enemy)
                    fixationCell(left, top);
                else if (!enemy)
                    fixationCell();
                else
                {
                    numberImg = 0;
                    speedAnimation = 0.1;
                }
                    return true;
            }
            return false;
        }
        public override string Dead()
        {
            path = $"../data/player/dead/{(int)numberImg}.png";
            numberImg += speedAnimation;
            if (numberImg == 7)
                Alive = false;
            numberImg %= 7;
            return path;
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
