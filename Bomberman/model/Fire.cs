using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman.model
{

    public class Fire
    {
        private string path;
        private double numberImg = 0;
        private double speedAnimation = 0.125;
        private string center = "0";
        private DateTime createTimeFire;
        private double left;
        private double top;
        public Fire(double left, double top, bool center = false)
        {
            this.left = left;
            this.top = top;
            if (center)
                this.center = "";
            createTimeFire = DateTime.Now;
        }
        public bool CheckIntersection(double left, double top)
        {
            if (Math.Abs(left - this.left) < Setting.CellSize && Math.Abs(this.top - top) < Setting.CellSize)         
                return true;
            return false;
        }
        public bool FireOut()
        {
            if ((DateTime.Now - createTimeFire).Seconds  >= Setting.TimeFire)
                return true;
            return false;
        }
        public string Update()
        {
            path = $"../data/bomb/fire/{center}";
            path += $"{(int)numberImg}.png";
            numberImg += speedAnimation;
            if(numberImg == 4)
            {
                numberImg = 2;
                speedAnimation *= -1;
            }

            return path;
        }
    }
}
