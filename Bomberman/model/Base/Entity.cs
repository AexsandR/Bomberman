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

        private double posX;
        private double posY;
        public  double Top { get;  set; }
        public  double Bottom { get; set; }
        public  double Left { get; set; }
        public  double Right { get; set; }
        public  string Diraction { get; set; }
        public  double Speed { get; set; }
        public abstract bool Alive { get; set; }
        public bool Death { get; set; } = false;
        public abstract double SizeSmallHitBox { get; }
        /// <summary>
        ///  метод обновления каритинок смерти
        /// </summary>
        /// <returns></returns>
        public abstract string Dead();
        /// <summary>
        /// метод который обновляет картинку у объекта
        /// </summary>
        /// <returns></returns>
        public abstract string Update();
        /// <summary>
        /// метод фиксирует положение объекта ровно в клетке по двум осям
        /// </summary>
        private void FixationCell()
        {
            FixationY();
            FixationX();

        }
        /// <summary>
        /// метод, который фиксирует положеник объекта ровно в клетке по оси X
        /// </summary>
        public void FixationX()
        {
            double leftPos0 = Math.Round(Left / Setting.CellSize) * Setting.CellSize;
            double leftPos1 = Math.Round((Left + Setting.CellSize) / Setting.CellSize) * Setting.CellSize;
            if ((Left - leftPos0) > (leftPos1 - Left))
            {
                Left = leftPos1;
            }
            else
            {
                Left = leftPos0;
            }
            Right = Setting.Right - Left;
        }
        /// <summary>
        /// метод, который фиксирует положеник обьекта ровно в клетке по оси Y
        /// </summary>
        public void FixationY()
        {
            double TopPos0 = Math.Round(Top / Setting.CellSize) * Setting.CellSize;
            double TopPos1 = Math.Round((Top + Setting.CellSize) / Setting.CellSize) * Setting.CellSize;
            if ((Top - TopPos0) > (TopPos1 - Top))
            {
                Top = TopPos1;
            }
            else
            {
                Top = TopPos0;
            }
            Bottom = Setting.Bottom - Top;
        }
        /// <summary>
        /// фиксирует положение объекта в клетке. Используется при срезе клетки
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        private void FixationCell(double left, double top)
        {
            switch (Diraction)
            {
                case "down":
                case "up":
                    if (Left > left)
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
                    if (Top < top) // наша точка находиться выше чем объекта которого мы задели
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
        /// <summary>
        /// проверка пересечения
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="smallHitBox"> маленький хитбокс</param>
        /// <returns></returns>
        public  bool CheckIntersection(double left, double top, bool smallHitBox = false)
        {
            if (Math.Abs(left - Left) < Setting.CellSize && Math.Abs(Top - top) < Setting.CellSize)
            {
                posX = Left + (Setting.CellSize - SizeSmallHitBox) / 2;
                posY = Top + (Setting.CellSize - SizeSmallHitBox) / 2;
                if (smallHitBox && Math.Abs(left - posX) <= SizeSmallHitBox && smallHitBox && Math.Abs(left - posY) <= SizeSmallHitBox ||
                    left < posX && posX < left + Setting.CellSize && top < posY && posY < top + Setting.CellSize)
                    return true;
                if (smallHitBox)
                    return false;
                if (Math.Abs(left - Left) <= Setting.CellSize && Math.Abs(left - Left) >= Setting.CellSection &&
                    Math.Abs(Top - top) <= Setting.CellSize && Math.Abs(Top - top) >= Setting.CellSection)
                    FixationCell(left, top);
                else
                    FixationCell();
                return true;
            }
            return false;
        }
        /// <summary>
        /// метод перещения
        /// </summary>
        public void Move()
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

