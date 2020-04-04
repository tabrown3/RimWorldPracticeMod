using System;
using System.Collections.Generic;
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

            return ThenInt(newRect);
        }

        public RectConnector Then(Func<Rect, RectConnector> thenFunc)
        {
            return Then(u => thenFunc(u).GetRect());
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

        public RectConnector IfThen(Func<bool> isTrue, Func<Rect, RectConnector> thenFunc, Func<Rect, Rect> elseFunc = null)
        {
            return IfThen(isTrue, u => thenFunc(u).GetRect(), elseFunc);
        }

        public RectConnector ThenGap(float gapSize)
        {
            return ThenInt(FloatToVec2(gapSize));
        }

        public RectConnector ThenForEach<T>(List<T> inList, Func<Rect, T, int, Rect> thenFunc)
        {
            for (var i = 0; i < inList.Count; i++)
            {
                Then(u => thenFunc(u, inList[i], i));
            }

            return this;
        }

        public RectConnector ThenForEach<T>(List<T> inList, Func<Rect, T, int, RectConnector> thenFunc)
        {
            return ThenForEach(inList, (u, v, w) => thenFunc(u, v, w).GetRect());
        }

        public Rect GetRect()
        {
            return new Rect(CurPos, CurLength);
        }

        protected abstract Rect RectAtPos();
        protected abstract Vector2 FloatToVec2(float inFloat);

        protected abstract Vector2 ToPrimaryDim(Vector2 vec1, Func<float, float> opFunc);
        protected abstract Vector2 ToPrimaryDim(Vector2 vec1, Vector2 vec2, Func<float, float, float> opFunc);
        protected abstract Vector2 ToSecondaryDim(Vector2 vec1, Func<float, float> opFunc);
        protected abstract Vector2 ToSecondaryDim(Vector2 vec1, Vector2 vec2, Func<float, float, float> opFunc);
        protected abstract Vector2 GetPrimaryDim(Vector2 inVec);
        protected abstract Vector2 GetSecondaryDim(Vector2 inVec);

        private RectConnector ThenInt(Rect inRect)
        {
            return ThenInt(new Vector2(inRect.width, inRect.height));
        }

        private RectConnector ThenInt(Vector2 length)
        {
            // add primary dimensions of CurPos and length and take the secondary dimension of CurPos unaltered
            //  e.g. for RectStacker, evaluates to: new Vector2(CurPos.x, CurPos.y + length.y)
            CurPos = ToPrimaryDim(CurPos, length, (u, v) => u + v) + GetSecondaryDim(CurPos);

            // add primary dimensions of CurLength and length and take the max of CurLength and length's secondary dimension
            //  e.g. for RectStacker, evaluates to: new Vector2(Mathf.Max(CurLength.x, length.x), CurLength.y + length.y)
            CurLength = ToPrimaryDim(CurLength, length, (u, v) => u + v) + ToSecondaryDim(CurLength, length, (u, v) => Mathf.Max(u, v));

            return this;
        }
    }
}
