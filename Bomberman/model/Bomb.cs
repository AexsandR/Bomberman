using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman.model
{
    public class Bomb
    {
        private string path;
        private double numberImg = 0;
        private double speedAnimation = 0.1;
        private DateTime createTimeBomb;
        public Bomb()
        {
            createTimeBomb = DateTime.Now;
        }
        public bool BombAtaka()
        {
            if ((DateTime.Now - createTimeBomb).Seconds >= Setting.TimeBeforeFire)
                return true;
            return false;
        }
        public  string Update()
        {
            path = "../data/bomb/";
            path += $"{(int)numberImg}.png";
            numberImg += speedAnimation;
            numberImg %= 3;
            return path;
        }
    }
}
