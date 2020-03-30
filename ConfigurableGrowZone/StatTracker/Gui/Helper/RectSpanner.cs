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

        protected override Rect CreateRectAtPos(Vector2 inPos)
        {
            return new Rect()
            {
                x = inPos.x
            };
        }

        protected override Vector2 GetRectPos(Rect inRect)
        {
            return new Vector2(inRect.x, 0f);
        }

        protected override Vector2 GetRectLength(Rect inRect)
        {
            return new Vector2(inRect.width, 0f);
        }

        protected override Vector2 FloatToVec2(float inFloat)
        {
            return new Vector2(inFloat, 0f);
        }
    }
}
