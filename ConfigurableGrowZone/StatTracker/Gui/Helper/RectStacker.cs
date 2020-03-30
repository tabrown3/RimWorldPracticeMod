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
        public RectStacker(Rect outerRect) : base(outerRect.y)
        {
        }

        protected override Rect CreateRectAtPos(float inPos)
        {
            return new Rect()
            {
                y = inPos
            };
        }

        protected override float GetRectPos(Rect inRect)
        {
            return inRect.y;
        }
    }
}
