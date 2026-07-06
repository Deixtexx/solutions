using System;
using System.Collections.Generic;
using System.Text;

namespace task04
{
    public class Cruiser : ISpaceship
    {
        public int Speed { get; private set; }
        public int FirePower { get; private set; }
        public int HP { get; set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public double Angle { get; private set; } // в радианах
        public Cruiser(double posX, double posY)
        {
            Speed = 50;
            FirePower = 100;
            HP = 200;
            PositionX = posX;
            PositionY = posY;
        }
        public void MoveForward()
        {
            PositionX += Speed * Math.Cos(Angle);
            PositionY += Speed * Math.Sin(Angle);
        }

        // представим, что задается новый угол
        public void Rotate(int angle)
        {
            Angle = (double)angle * (Math.PI / 180.0);
        }

        public void Fire(ISpaceship target)
        {
            target.HP -= FirePower;
        }
    }
}
