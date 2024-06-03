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
        private double numberImg;
        private double speedAnimation;
        public override double Speed { get; set; } = 4;
        public override string Diraction { get; set; }
        public override double Left { get; set; }
        public override double Top { get; set; }
        private bool alive = true;
        public override bool Alive
        {
            get { return alive; }
            set
            {
                if (value)
                {
                    numberImg = 0;
                    speedAnimation = 0.25;
                }
                else
                    if(value != alive)
                {
                    speedAnimation = 0.1;
                    numberImg = 0;
                }
                alive = value;
            }
        }
        public override double SizeSmallHitBox => Setting.SizeSmallHitBoxPlayer;
        public override double Right { get; set; }
        public override double Bottom { get; set; }
        public Player() { }
        public Player(double left, double top, double right, double bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
            Alive = true;
        }
        public override string Dead()
        {
            if (numberImg > 8) { 
                return path; }
            path = $"../data/player/dead/{(int)numberImg}.png";
            numberImg += speedAnimation;
            return path;
        }

        public void PutBomb()
        {

            FixationY();
            FixationX();
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
            numberImg %= 3;
            return path;

        }
    }
}
