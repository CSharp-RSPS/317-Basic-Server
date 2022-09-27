using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Creates a projectile.
    /// </summary>
    [PacketDef(PacketDefinition.CreateProjectile)]
    public sealed class SendCreateProjectile : IPacketPayloadBuilder
    {

        public int PositionOffset { get; private set; }

        public int SecondXOffset { get; private set; }

        public int SecondYOffet { get; private set; }

        public int Target { get; private set; }

        public int GraphicId { get; private set; }

        public int StartingHeight { get; private set; }

        public int EndingHeight { get; private set; }

        public int StartingTime { get; private set; } //In game ticks

        public int Speed { get; private set; } // In game ticks

        public int InitialSlope { get; private set; }

        public int InitialDistanceFromSource { get; private set; }


        public SendCreateProjectile(int positionOffset, int secondXOffset, int secondYOffet, 
            int target, int graphicId, int startingHeight, int endingHeight, 
            int startingTime, int speed, int initialSlope, int initialDistanceFromSource)
        {
            PositionOffset = positionOffset;
            SecondXOffset = secondXOffset;
            SecondYOffet = secondYOffet;
            Target = target;
            GraphicId = graphicId;
            StartingHeight = startingHeight;
            EndingHeight = endingHeight;
            StartingTime = startingTime;
            Speed = speed;
            InitialSlope = initialSlope;
            InitialDistanceFromSource = initialDistanceFromSource;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteByte(PositionOffset);
            writer.WriteByte(SecondXOffset);
            writer.WriteByte(SecondYOffet);
            writer.WriteShort(Target);
            writer.WriteShortLittleEndian(GraphicId);
            writer.WriteByte(StartingHeight);
            writer.WriteByte(EndingHeight);
            writer.WriteShortLittleEndian(StartingTime);
            writer.WriteShortLittleEndian(Speed);
            writer.WriteByte(InitialSlope);
            writer.WriteByte(InitialDistanceFromSource);
        }

    }

}
