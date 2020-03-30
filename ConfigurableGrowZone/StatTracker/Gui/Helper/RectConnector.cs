using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ConfigurableGrowZone
{
    public abstract class RectConnector
    {
        private Vector2 curPos = Vector2.zero;
        private Vector2 curLength = Vector2.zero;

        public RectConnector() { }

        public RectConnector(Vector2 startingPos)
        {
            curPos = startingPos;
        }

        public RectConnector(Rect inRect)
        {
            curPos = new Vector2(inRect.x, inRect.y);
        }

        public RectConnector Then(Func<Rect, Rect> thenFunc)
        {
            var currentRect = CreateRectAtPos(curPos);
            var newRect = thenFunc(currentRect);

            return ThenInt(GetRectLength(newRect));
        }

        public RectConnector Then(Func<Rect, RectConnector> thenFunc)
        {
            var currentRect = CreateRectAtPos(curPos);
            var outConnector = thenFunc(currentRect);

            return ThenInt(outConnector.curLength);
        }

        public RectConnector Then(RectConnector rectStacker)
        {
            return ThenInt(rectStacker.curLength);
        }

        public RectConnector IfThen(Func<bool> isTrue, Func<Rect, Rect> thenFunc, Func<Rect, Rect> elseFunc = null)
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

        public RectConnector IfThen(Func<bool> isTrue, Func<Rect, RectConnector> thenFunc, Func<Rect, RectConnector> elseFunc = null)
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

        public RectConnector ThenGap(float gapSize)
        {
            curPos += FloatToVec2(gapSize);
            curLength += FloatToVec2(gapSize);

            return this;
        }

        public RectConnector ThenForEach<T>(List<T> inList, Func<Rect, T, int, RectConnector> thenFunc)
        {
            for (var i = 0; i < inList.Count; i++)
            {
                Then(u => thenFunc(u, inList[i], i));
            }

            return this;
        }

        protected abstract Rect CreateRectAtPos(Vector2 inPos);
        protected abstract Vector2 GetRectPos(Rect inRect);
        protected abstract Vector2 GetRectLength(Rect inRect);
        protected abstract Vector2 FloatToVec2(float inFloat);

        private RectConnector ThenInt(Vector2 length)
        {
            curPos += length;
            curLength += length;

            return this;
        }
    }
}
