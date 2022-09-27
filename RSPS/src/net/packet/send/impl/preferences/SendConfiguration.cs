using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// The client stores various user settings in an array, the default values are also stored in another array. 
    /// This packet changes the default value for a setting and its current value to the one given.
    /// Opcode 87 (length 6) is extremely similar in structure, but the new value is received as an Middle Endian Small Int. 
    /// This suggests its for use with bigger setting values.
    /// </summary>
    [PacketDef(PacketDefinition.ForceClientSetting)]
    public sealed class SendConfiguration : IPacketPayloadBuilder
    {

        /// <summary>
        /// The ID of the setting
        /// </summary>
        public int SettingId { get; private set; }

        /// <summary>
        /// The new value
        /// </summary>
        public int Value { get; private set; }


        /// <summary>
        /// Creates a new force client setting packet payload builder
        /// </summary>
        /// <param name="settingId">The setting id</param>
        /// <param name="value">The new value</param>
        public SendConfiguration(int settingId, bool value) : this(settingId, value ? 1 : 0) { }

        /// <summary>
        /// Creates a new force client setting packet payload builder
        /// </summary>
        /// <param name="settingId">The setting id</param>
        /// <param name="value">The new value</param>
        public SendConfiguration(int settingId, int value)
        {
            SettingId = settingId;
            Value = value;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteShortLittleEndian(SettingId);
            writer.WriteByte(Value);
        }

    }
}
