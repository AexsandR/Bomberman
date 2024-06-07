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
        /// <summary>
        /// метод который определяет закончился ли огонь
        /// </summary>
        /// <returns></returns>
        public bool FireOut()
        {
            if ((DateTime.Now - createTimeFire).Seconds  >= Setting.TimeFire)
                return true;
            return false;
        }
        /// <summary>
        /// метод обновление картинок у объекта
        /// </summary>
        /// <returns></returns>
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
