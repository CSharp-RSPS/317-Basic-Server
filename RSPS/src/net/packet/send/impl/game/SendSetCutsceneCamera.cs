﻿using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Set cutscene camera
    /// </summary>
    [PacketDef(PacketDefinition.SetCutsceneCamera)]
    public sealed class SendSetCutsceneCamera : IPacketPayloadBuilder
    {


        public void WritePayload(PacketWriter writer)
        {
            throw new NotImplementedException();
        }

    }

}
