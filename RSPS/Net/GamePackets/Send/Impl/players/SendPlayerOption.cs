using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
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
        /// <param name="actionText">The text to display</param>
        /// <param name="flag">TODO: No clue yet</param>
        public SendPlayerOption(int optionIndex, string actionText, bool flag = false)
        {
            if (optionIndex < 1 || optionIndex > 5)
            {
                throw new Exception("Invalid option index");
            }
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
            writer.WriteByteNegated(OptionIndex);
            writer.WriteByteAdditional(Flag ? 1 : 0);
            writer.WriteRS2String(ActionText);
        }

    }

}
