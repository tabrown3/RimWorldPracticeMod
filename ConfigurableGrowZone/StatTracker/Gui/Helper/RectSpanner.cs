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
        public RectSpanner(Rect outerRect) : base(outerRect.x)
        {
        }

        protected override Rect CreateRectAtPos(float inPos)
        {
            return new Rect()
            {
                x = inPos
            };
        }

        protected override float GetRectPos(Rect inRect)
        {
            return inRect.x;
        }

        protected override float GetRectLength(Rect inRect)
        {
            return inRect.width;
        }
    }
}
