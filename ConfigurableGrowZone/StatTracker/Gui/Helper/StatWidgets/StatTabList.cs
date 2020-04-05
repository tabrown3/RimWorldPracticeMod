using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace ConfigurableGrowZone
{
    public class StatTabList
    {
        private object lastMouseoverObj = null;
        public Rect DrawItem<T>(Rect inRect, T selectedObj, T obj, int ind, Action<Rect, T, int> drawFunc, Action<T> onClick) where T : class
        {
            Rect listItemRect = new Rect(inRect);
            listItemRect.height = 45f;

            if (ind % 2 == 0)
            {
                Widgets.DrawAltRect(listItemRect);
            }

            drawFunc(listItemRect, obj, ind);

            if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && Mouse.IsOver(listItemRect))
            {
                onClick(obj);
                SoundDefOf.Click.PlayOneShotOnCamera();
            }

            Widgets.DrawHighlightIfMouseover(listItemRect);

            if (Mouse.IsOver(listItemRect) && lastMouseoverObj != obj)
            {
                SoundDefOf.Mouseover_Standard.PlayOneShotOnCamera();
                lastMouseoverObj = obj;
            }

            if (selectedObj == obj)
            {
                Widgets.DrawHighlightSelected(listItemRect);
            }

            return listItemRect;
        }
    }
}
