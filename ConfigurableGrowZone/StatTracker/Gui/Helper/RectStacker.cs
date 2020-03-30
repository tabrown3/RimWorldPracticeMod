using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ConfigurableGrowZone
{
    public class RectStacker : RectConnector
    {
        public RectStacker() : base() { }
        public RectStacker(Vector2 startingPos) : base(startingPos) { }
        public RectStacker(Rect inRect) : base(inRect) { /*CurLength.x = inRect.width;*/ }

        protected override Rect RectAtPos()
        {
            return new Rect()
            {
                x = this.CurPos.x,
                y = this.CurPos.y,
                width = this.CurLength.x
            };
        }

        protected override Vector2 SelectivePosSum(Vector2 inLength, Vector2 curPos)
        {
            return new Vector2(curPos.x, inLength.y + curPos.y);
        }

        protected override Vector2 SelectiveLengthSum(Vector2 inLength, Vector2 curLength)
        {
            return new Vector2(Mathf.Max(inLength.x, curLength.x), inLength.y + curLength.y);
        }

        protected override Vector2 FloatToVec2(float inFloat)
        {
            return new Vector2(0f, inFloat);
        }
    }
}
