using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace Bomberman.model.Base
{
    public abstract class Entity
    {

        public abstract double Top { get;  set; }
        public abstract double Bottom { get; set; }
        public abstract double Left { get; set; }
        public abstract double Right { get; set; }
        public abstract bool Alive { get; set; }
        public abstract string Diraction { get; set; }
        public abstract double Speed { get; set; }
        public abstract string Dead();
        public abstract string Update();
        public abstract bool CheckIntersection(double left, double top, bool enemy = false);
        public void move()
        {
            if (!Alive)
                return;
            
            if (Diraction == "up")
            { 
                Top -= Speed;
                Bottom += Speed;
            }
            if (Diraction == "down")
            {
                Top += Speed;
                Bottom -= Speed;
            }

            if (Diraction == "left")
            {
                Right += Speed;
                Left -= Speed;
            }
            if (Diraction == "right")
            {
                Left += Speed;
                Right -= Speed;
            }
            }
        }
    }

