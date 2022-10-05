using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Game.Comms.Dialogues
{
    /// <summary>
    /// Holds the possible dialogue speaker expressions
    /// </summary>
    public enum DialogueExpression
    {

        [FacialExpression(591)]
        Default = 0,
        [FacialExpression(588)]
        Happy = 1,
        [FacialExpression(589)]
        Calm = 2,
        [FacialExpression(590)]
        CalmContinued = 3,
        [FacialExpression(592)]
        Evil = 4,
        [FacialExpression(593)]
        EvilContinued = 5,
        [FacialExpression(594)]
        DelightedEvil = 6,
        [FacialExpression(595)]
        Annoyed = 7,
        [FacialExpression(596)]
        Distressed,
        [FacialExpression(597)]
        DistressedContinued,
        [FacialExpression(600)]
        DisorientedLeft,
        [FacialExpression(601)]
        DisorientedRight,
        [FacialExpression(602)]
        Uninterested,
        [FacialExpression(603)]
        Sleepy,
        [FacialExpression(604)]
        PlainEvil,
        [FacialExpression(605)]
        Laughing,
        [FacialExpression(608)]
        LaughingSecondary,
        [FacialExpression(606)]
        LongerLaughing,
        [FacialExpression(607)]
        LongerLaughingSecondary,
        [FacialExpression(609)]
        EvilLaughShort,
        [FacialExpression(610)]
        SlighlySad,
        [FacialExpression(599)]
        Sad,
        [FacialExpression(611)]
        VerySad,
        [FacialExpression(612)]
        Other,
        [FacialExpression(598)]
        NearTears,
        [FacialExpression(613)]
        NearTearsSecondary,
        [FacialExpression(614)]
        Angry,
        [FacialExpression(615)]
        Angry2,
        [FacialExpression(616)]
        Angry3,
        [FacialExpression(617)]
        Angry4

    }
}
