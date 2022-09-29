using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.Mobiles.Players
{
    public class PlayerUpdateFlags : EntityUpdateFlags
    {

        public bool AsyncMovementUpdateRequired = false;
        public bool PublicChatUpdateRequired = false;
        public bool AppearanceUpdateRequired = false;

        public int Mask()
        {
            return Mask(false, false);
        }

        public int Mask(bool forceAppearance, bool noPublicChat)
        {
            int mask = 0x0;

            if (AsyncMovementUpdateRequired)
            {
                mask |= 0x400;
            }

            if (GraphicsUpdateRequired)
            {
                mask |= 0x100;
            }

            if (AnimationUpdateRequired)
            {
                mask |= 0x8;
            }

            if (ForcedChatUpdateRequired)
            {
                mask |= 0x4;
            }

            if (PublicChatUpdateRequired && !noPublicChat)
            {
                mask |= 0x80;
            }

            if (InteractingNpcUpdateRequired)
            {
                mask |= 0x1;
            }

            if (AppearanceUpdateRequired || forceAppearance)
            {
                mask |= 0x10;
            }

            if (FaceCoordinatesUpdateRequired)
            {
                mask |= 0x2;
            }

            if (PrimaryHitUpdateRequired)
            {
                mask |= 0x20;
            }

            if (SecondaryHitUpdateRequired)
            {
                mask |= 0x200;
            }
            return mask;
        
        }

        public void setUpdateRequired()
        {
            setUpdateRequired();
            //setAllBuffersOutdated();
        }

        public void SetAsyncMovementUpdateRequired()
        {
            setUpdateRequired();
            AsyncMovementUpdateRequired = true;
        }

        public void SetPublicChatUpdateRequired()
        {
            setUpdateRequired();
            PublicChatUpdateRequired = true;
        }

        public void SetAppearanceUpdateRequired()
        {
            setUpdateRequired();
            AppearanceUpdateRequired = true;
        }

        /**
         * Resets all update flags.
         */
        public override void ResetFlags()
        {
            base.ResetFlags();
            AsyncMovementUpdateRequired = false;
            PublicChatUpdateRequired = false;
            AppearanceUpdateRequired = false;
            //SetAllBuffersOutdated();
        }

    }
}
