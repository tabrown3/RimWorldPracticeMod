using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ConfigurableGrowZone
{
    static public class StatWidgets
    {
        public static Rect DrawTextButton<T>(Rect inRect, string label, List<T> objectList, Func<T, string> labelFunc, T selectedObject, Action<T> objectCb)
        {
            Rect outerRect = new Rect(inRect);
            outerRect.height = 35f;

            Rect textButtonRect = new Rect(outerRect);
            textButtonRect.width = 100f;

            if (Widgets.ButtonText(textButtonRect, label))
            {
                List<FloatMenuOption> list = objectList.Select(v => new FloatMenuOption(labelFunc(v), () => objectCb(v))).ToList();

                Find.WindowStack.Add(new FloatMenu(list));
            }

            if (selectedObject != null)
            {
                string selectedObjectLabel = labelFunc(selectedObject);

                Rect typeNameRect = new Rect(outerRect);
                typeNameRect.x = textButtonRect.x + textButtonRect.width;
                typeNameRect.width = Text.CalcSize(selectedObjectLabel).x;
                Widgets.Label(typeNameRect, selectedObjectLabel);
            }

            return outerRect;
        }
    }
}
