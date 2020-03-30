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
        public Vector2 CurPos = Vector2.zero;
        public Vector2 CurLength = Vector2.zero;

        public RectConnector() { }

        public RectConnector(Vector2 startingPos)
        {
            CurPos = startingPos;
        }

        public RectConnector(Rect inRect)
        {
            CurPos = new Vector2(inRect.x, inRect.y);
        }

        public RectConnector Then(Func<Rect, Rect> thenFunc)
        {
            var currentRect = RectAtPos();
            var newRect = thenFunc(currentRect);

            return ThenInt(GetRectLength(newRect));
        }

        public RectConnector Then(Func<Rect, RectConnector> thenFunc)
        {
            var currentRect = RectAtPos();
            var outConnector = thenFunc(currentRect);

            return ThenInt(outConnector.CurLength);
        }

        public RectConnector Then(RectConnector rectStacker)
        {
            return ThenInt(rectStacker.CurLength);
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
            CurPos += FloatToVec2(gapSize);
            CurLength += FloatToVec2(gapSize);

            return this;
        }

        public RectConnector ThenForEach<T>(List<T> inList, Func<Rect, T, int, Rect> thenFunc)
        {
            for (var i = 0; i < inList.Count; i++)
            {
                Then(u => thenFunc(u, inList[i], i));
            }

            return this;
        }

        public Rect GetRect()
        {
            return new Rect(CurPos, CurLength);
        }

        protected abstract Rect RectAtPos();
        protected abstract Vector2 FloatToVec2(float inFloat);
        protected abstract Vector2 SelectivePosSum(Vector2 inLength, Vector2 curPos);
        protected abstract Vector2 SelectiveLengthSum(Vector2 inLength, Vector2 curLength);

        private RectConnector ThenInt(Vector2 length)
        {
            CurPos = SelectivePosSum(length, CurPos);
            CurLength = SelectiveLengthSum(length, CurLength);

            return this;
        }

        private Vector2 GetRectLength(Rect inRect)
        {
            return new Vector2(inRect.width, inRect.height);
        }
    }
}
