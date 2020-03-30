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

        public RectConnector(float startingPos)
        {
            curPos = startingPos;
        }

        public T Then<T>(Func<Rect, Rect> thenFunc) where T : RectConnector
        {
            var newRect = CreateRectAtPos(curPos);
            var bob = thenFunc(newRect);

            return ThenInt<T>(bob.height);
        }

        public T Then<T>(Func<Rect, RectConnector> thenFunc) where T : RectConnector
        {
            var newRect = CreateRectAtPos(curPos);
            var bob = thenFunc(newRect);

            return ThenInt<T>(bob.curLength);
        }

        public T Then<T>(T rectStacker) where T : RectConnector
        {
            return ThenInt<T>(rectStacker.curLength);
        }

        public T IfThen<T>(Func<bool> isTrue, Func<Rect, Rect> thenFunc, Func<Rect, Rect> elseFunc = null) where T : RectConnector
        {
            if (isTrue())
            {
                return Then<T>(thenFunc);
            }
            else
            {
                if (elseFunc != null)
                {
                    return Then<T>(elseFunc);
                }
                else
                {
                    return (T)this;
                }
            }
        }

        public T IfThen<T>(Func<bool> isTrue, Func<Rect, RectConnector> thenFunc, Func<Rect, RectConnector> elseFunc = null) where T : RectConnector
        {
            if (isTrue())
            {
                return Then<T>(thenFunc);
            }
            else
            {
                if (elseFunc != null)
                {
                    return Then<T>(elseFunc);
                }
                else
                {
                    return (T)this;
                }
            }
        }

        public T ThenGap<T>(float gapSize) where T : RectConnector
        {
            curPos += gapSize;
            curLength += gapSize;

            return (T)this;
        }

        public T ThenForEach<T, T2>(List<T2> inList, Func<Rect, T2, int, T> thenFunc) where T : RectConnector
        {
            for (var i = 0; i < inList.Count; i++)
            {
                Then<T>(u => thenFunc(u, inList[i], i));
            }

            return (T)this;
        }

        private T ThenInt<T>(float length) where T : RectConnector
        {
            curPos += length;
            curLength += length;

            return (T)this;
        }
    }
}
