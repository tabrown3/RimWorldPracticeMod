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
            var lolol = new RectSpanner(inRect)
                .Then(u =>
                {
                    Rect textButtonRect = new Rect(u);
                    textButtonRect.width = 100f;
                    textButtonRect.height = 35f;

                    if (Widgets.ButtonText(textButtonRect, label))
                    {
                        List<FloatMenuOption> list = objectList.Select(v => new FloatMenuOption(labelFunc(v), () => objectCb(v))).ToList();

                        Find.WindowStack.Add(new FloatMenu(list));
                    }

                    return textButtonRect;
                })
                .IfThen(() => selectedObject != null, u =>
                {
                    string selectedObjectLabel = labelFunc(selectedObject);

                    Rect typeNameRect = new Rect(u);
                    typeNameRect.width = Text.CalcSize(selectedObjectLabel).x;
                    typeNameRect.height = 35f;

                    Widgets.Label(typeNameRect, selectedObjectLabel);

                    return typeNameRect;
                });

            return new Rect(lolol.CurPos.x, lolol.CurPos.y, lolol.CurLength.x, 35f); // TODO: figure out how to make RectSpanner know its height and RectStacker know its width... maybe?
        }
    }
}
