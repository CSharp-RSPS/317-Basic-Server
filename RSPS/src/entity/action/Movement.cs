using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity
{
    public struct Movement
    {
        public readonly Position StartPosition;
        public readonly Position EndPosition;
        public readonly int StartToEndSpeed;
        public readonly int EndToStartSpeed;
        public readonly int Direction;

        public Movement(Position startPosition, Position endPosition, int startToEndSpeed, int endToStartSpeed,
                         int direction)
        {
            StartPosition = startPosition;
            EndPosition = endPosition;
            StartToEndSpeed = startToEndSpeed;
            EndToStartSpeed = endToStartSpeed;
            Direction = direction;
        }
    }
}
