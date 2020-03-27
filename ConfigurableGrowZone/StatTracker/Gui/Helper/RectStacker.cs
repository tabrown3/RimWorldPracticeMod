using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ConfigurableGrowZone
{
    public class RectStacker
    {
        private float curY = 0f;
        private float curHeight = 0f;
        public RectStacker(float startingY)
        {
            curY = startingY;
        }
        public RectStacker(Rect outerRect)
        {
            curY = outerRect.y;
        }

        public RectStacker Then(Func<Rect, Rect> thenFunc)
        {
            var newRect = new Rect();
            newRect.y = this.curY;
            var bob = thenFunc(newRect);

            curY += bob.height;
            curHeight += bob.height;

            return this;
        }

        public RectStacker Then(Func<Rect, RectStacker> thenFunc)
        {
            var newRect = new Rect();
            newRect.y = this.curY;
            var bob = thenFunc(newRect);

            curY += bob.curHeight;
            curHeight += bob.curHeight;

            return this;
        }

        public RectStacker Then(RectStacker rectStacker)
        {
            curY += rectStacker.curHeight;
            curHeight += rectStacker.curHeight;
            return this;
        }

        public RectStacker ThenGap(float gapSize)
        {
            curY += gapSize;
            curHeight += gapSize;

            return this;
        }
    }
}
