using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ConfigurableGrowZone
{
    public class RectSpanner : RectConnector
    {
        public RectSpanner() : base() { }
        public RectSpanner(Vector2 startingPos) : base(startingPos) { }
        public RectSpanner(Rect inRect) : base(inRect) { }

        protected override Rect RectAtPos()
        {
            return new Rect()
            {
                x = this.CurPos.x,
                y = this.CurPos.y,
                height = this.CurLength.y
            };
        }

        protected override Vector2 SelectivePosSum(Vector2 inLength, Vector2 curPos)
        {
            return new Vector2(inLength.x + curPos.x, curPos.y);
        }

        protected override Vector2 SelectiveLengthSum(Vector2 inLength, Vector2 curLength)
        {
            return new Vector2(inLength.x + curLength.x, Mathf.Max(inLength.y, curLength.y));
        }

        protected override Vector2 FloatToVec2(float inFloat)
        {
            return new Vector2(inFloat, 0f);
        }
    }
}
