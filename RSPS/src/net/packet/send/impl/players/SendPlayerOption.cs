using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Adds an option to a player's right click context menu.
    /// </summary>
    [PacketDef(PacketDefinition.PlayerOption)]
    public sealed class SendPlayerOption : IPacketVariablePayloadBuilder
    {

        /// <summary>
        /// The index in the option menu
        /// </summary>
        public int OptionIndex { get; private set; }

        /// <summary>
        /// TODO: No clue yet
        /// </summary>
        public bool Flag { get; private set; }

        /// <summary>
        /// The text to display
        /// </summary>
        public string ActionText { get; private set; }


        /// <summary>
        /// Creates a new player option packet payload builder
        /// </summary>
        /// <param name="optionIndex">The index in the option menu</param>
        /// <param name="flag">TODO: No clue yet</param>
        /// <param name="actionText">The text to display</param>
        public SendPlayerOption(int optionIndex, bool flag, string actionText)
        {
            OptionIndex = optionIndex;
            Flag = flag;
            ActionText = actionText;
        }

        public int GetPayloadSize()
        {
            return ActionText.Length + 2;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteByte(OptionIndex, Packet.ValueType.Negated);
            writer.WriteByte(Flag ? 1 : 0, Packet.ValueType.Additional);
            writer.WriteString(ActionText);
        }

    }

}
