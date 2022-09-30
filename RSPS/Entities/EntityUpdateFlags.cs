using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Entities
{
    public abstract class EntityUpdateFlags
    {

        public bool UpdateRequired = false;
        public bool GraphicsUpdateRequired = false;
        public bool AnimationUpdateRequired = false;
        public bool ForcedChatUpdateRequired = false;
        public bool FaceCoordinatesUpdateRequired = false;
        public bool PrimaryHitUpdateRequired = false;
        public bool SecondaryHitUpdateRequired = false;
        public bool InteractingNpcUpdateRequired = false;

        /**
        * Resets all update flags.
        */
        public virtual void ResetFlags()
        {
            UpdateRequired = false;
            GraphicsUpdateRequired = false;
            AnimationUpdateRequired = false;
            ForcedChatUpdateRequired = false;
            InteractingNpcUpdateRequired = false;
            FaceCoordinatesUpdateRequired = false;
            PrimaryHitUpdateRequired = false;
            SecondaryHitUpdateRequired = false;
        }

    }
}
