using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Draw a given model on a given interface.
    /// </summary>
    [PacketDef(PacketDefinition.SetInterfaceModel)]
    public sealed class SendDrawInterfaceModel : IPacketPayloadBuilder
    {

        /// <summary>
        /// The interface identifier
        /// </summary>
        public int InterfaceId { get; private set; }

        /// <summary>
        /// The model identifier
        /// </summary>
        public int ModelId { get; private set; }


        /// <summary>
        /// Draws a model onto an interface
        /// </summary>
        /// <param name="interfaceId">The interface identifier</param>
        /// <param name="modelId">The model identifier</param>
        public SendDrawInterfaceModel(int interfaceId, int modelId)
        {
            InterfaceId = interfaceId;
            ModelId = modelId;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteShortAdditionalLittleEndian(InterfaceId);
            writer.WriteShort(ModelId);
        }

    }

}
