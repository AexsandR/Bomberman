using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Bomberman.model
{
    public class Brick
    {
        private string path;
        private double numberImg = 1;
        private double speedAnimation = 0.3;
        public bool WholeBrick { get; set; } = true;

        public bool Exit { get; set; } = false;
        public bool ProcessDestruction { get; set; } = false;
        public string Update()
        {
            if (numberImg > 8)
            {
                if (Exit)
                    return "../data/brick/exit.png";
                return path;

            }
            path = "../data/brick/";
            path += $"{(int)numberImg}.png";
            numberImg += speedAnimation;
            return path;
        }
    }
}
