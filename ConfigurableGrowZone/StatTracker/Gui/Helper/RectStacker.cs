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
        public RectStacker(Rect inRect) : base(inRect) { }

        protected override Rect CreateRectAtPos(Vector2 inPos)
        {
            return new Rect()
            {
                y = inPos.y
            };
        }

        protected override Vector2 GetRectPos(Rect inRect)
        {
            return new Vector2(0f, inRect.y);
        }

        protected override Vector2 GetRectLength(Rect inRect)
        {
            return new Vector2(0f, inRect.height);
        }

        protected override Vector2 FloatToVec2(float inFloat)
        {
            return new Vector2(0f, inFloat);
        }
    }
}
