using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity
{
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public Position(int x, int y, int z)
        {
           this.X = x;
           this.Y = y;
           this.Z = z;
        }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
            Z = 0;
        }

        public void SetNewPosition(Position other)
        {
            X = other.X;
            Y = other.Y;
            Z = other.Z;
        }

        public void MovePosition(int amountX, int amountY)
        {
            X = X + amountX;
            Y = Y + amountY;
        }

        public Position GetPosition()
        {
            return this;
        }

        public int GetRegionX()
        {
            return (X >> 3) - 6;
        }

        public int GetRegionY()
        {
           return (Y >> 3) - 6;
        }

        public int GetLocalX(Position position)
        {
            return X - 8 * position.GetRegionX();
        }
        public int GetLocalY(Position position)
        {
            return Y - 8 * position.GetRegionY();
        }

        public int GetLocalX()
        {
            return GetLocalX(this);
        }

        public int GetLocalY()
        {
            return GetLocalY(this);
        }

        /**
         * Returns the delta coordinates. Note that the returned Position is not an
         * actual position, instead it's values represent the delta values between
         * the two arguments.
         *
         * @param a the first position
         * @param b the second position
         * @return the delta coordinates contained within a position
         */
        public static Position Delta(Position a, Position b)
        {
            return new Position(b.X - a.X, b.Y - a.Y);
        }

        public bool isViewableFrom(Position other)
        {
            Position p = Delta(this, other);
            return p.Z == 0 && p.X <= 14 && p.X >= -15 && p.Y <= 14 && p.Y >= -15;
        }

        
        public override string ToString()
        {
            return "Position(" + X + ", " + Y + ", " + Z + ")";
        }

    }
}
