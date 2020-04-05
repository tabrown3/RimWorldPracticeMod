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
    static public class StatWidgets
    {
        public static RectConnector DrawTextButtonSideLabel<T>(Rect inRect, string label, List<T> objectList, Func<T, string> labelFunc, T selectedObject, Action<T> objectCb)
        {
            return DrawTextButton(inRect, label, objectList, labelFunc, selectedObject, objectCb, u => new RectSpanner(u));
        }

        public static RectConnector DrawTextButtonBottomLabel<T>(Rect inRect, string label, List<T> objectList, Func<T, string> labelFunc, T selectedObject, Action<T> objectCb)
        {
            return DrawTextButton(inRect, label, objectList, labelFunc, selectedObject, objectCb, u => new RectStacker(u));
        }

        public static Rect DrawSectionHeader(Rect inRect, string heading)
        {
            Rect headerRect = new Rect(inRect);
            headerRect.height = 35f;
            headerRect.width = 100f;//Text.CalcSize(heading).x;

            Widgets.Label(headerRect, heading);

            return headerRect;
        }

        public static StatTabList StatTabListFactory()
        {
            return new StatTabList();
        }

        private static RectConnector DrawTextButton<T>(Rect inRect, string label, List<T> objectList, Func<T, string> labelFunc, T selectedObject, Action<T> objectCb, Func<Rect, RectConnector> connectorFunc)
        {
            return connectorFunc(inRect)
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
        }
    }
}
