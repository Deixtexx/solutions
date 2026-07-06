using System;
using System.Collections.Generic;
using System.Text;

namespace task04
{
    public class Fighter : ISpaceship
    {
        public int Speed { get; private set; }
        public int FirePower { get; private set; }
        public int HP { get; set; }
        public double PositionX { get; private set; }
        public double PositionY { get; private set; }
        public double Angle { get; private set; }

        public Fighter(double posX, double posY)
        {
            Speed = 100;
            FirePower = 40;
            HP = 100;
            PositionX = posX;
            PositionY = posY;
        }

        public void MoveForward()
        {
            PositionX += Speed * Math.Cos(Angle);
            PositionY += Speed * Math.Sin(Angle);
        }

        public void Rotate(int angle)
        {
            Angle = angle * (Math.PI / 180.0);
        }

        public void Fire(ISpaceship target)
        {
            target.HP -= FirePower;
        }
    }
}
