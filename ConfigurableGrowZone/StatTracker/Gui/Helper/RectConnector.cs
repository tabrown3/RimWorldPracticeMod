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
        private float curPos = 0f;
        private float curLength = 0f;
        protected abstract Rect CreateRectAtPos(float inPos);
        protected abstract float GetRectPos(Rect inRect);
        protected abstract float GetRectLength(Rect inRect);

        public RectConnector(float startingPos)
        {
            curPos = startingPos;
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
            var newRect = thenFunc(currentRect);

            return ThenInt(newRect.curLength);
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
            curPos += gapSize;
            curLength += gapSize;

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

        private RectConnector ThenInt(float length)
        {
            curPos += length;
            curLength += length;

            return this;
        }
    }
}
