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

            return ThenInt(bob.height);
        }

        public RectStacker Then(Func<Rect, RectStacker> thenFunc)
        {
            var newRect = new Rect();
            newRect.y = this.curY;
            var bob = thenFunc(newRect);

            return ThenInt(bob.curHeight);
        }

        public RectStacker Then(RectStacker rectStacker)
        {
            return ThenInt(rectStacker.curHeight);
        }

        public RectStacker IfThen(Func<bool> isTrue, Func<Rect, Rect> thenFunc, Func<Rect, Rect> elseFunc = null)
        {
            if (isTrue())
            {
                return Then(thenFunc);
            }
            else
            {
                if (elseFunc != null)
                {
                    return Then(elseFunc);
                }
                else
                {
                    return this;
                }
            }
        }

        public RectStacker IfThen(Func<bool> isTrue, Func<Rect, RectStacker> thenFunc, Func<Rect, RectStacker> elseFunc = null)
        {
            if (isTrue())
            {
                return Then(thenFunc);
            }
            else
            {
                if (elseFunc != null)
                {
                    return Then(elseFunc);
                }
                else
                {
                    return this;
                }
            }
        }

        public RectStacker ThenGap(float gapSize)
        {
            curY += gapSize;
            curHeight += gapSize;

            return this;
        }

        public RectStacker ThenForEach<T>(List<T> inList, Func<Rect, T, int, RectStacker> thenFunc)
        {
            for (var i = 0; i < inList.Count; i++)
            {
                Then(u => thenFunc(u, inList[i], i));
            }

            return this;
        }

        private RectStacker ThenInt(float height)
        {
            curY += height;
            curHeight += height;

            return this;
        }
    }
}
