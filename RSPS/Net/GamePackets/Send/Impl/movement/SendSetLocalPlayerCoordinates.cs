﻿using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Set local player coordinates
    /// </summary>
    [PacketDef(PacketDefinition.SetLocalPlayerCoordinates)]
    public sealed class SendSetLocalPlayerCoordinates : IPacketPayloadBuilder
    {


        public void WritePayload(PacketWriter writer)
        {
            throw new NotImplementedException();
        }

    }

}