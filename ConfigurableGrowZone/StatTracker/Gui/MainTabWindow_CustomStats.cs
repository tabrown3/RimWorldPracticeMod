using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace ConfigurableGrowZone
{
    [StaticConstructorOnStartup]
    public class MainTabWindow_CustomStats : MainTabWindow
    {
        private List<TabRecord> leftTabs = new List<TabRecord>();
        private List<TabRecord> rightTabs = new List<TabRecord>();

        private enum TabEnum
        {
            Trackers,
            Signals,
            Metrics,
            Derived
        }

        private TabEnum leftTabActive = TabEnum.Trackers;
        private TabEnum rightTabActive = TabEnum.Metrics;

        public override void PreOpen()
        {
            base.PreOpen();

            leftTabs.Clear();
            rightTabs.Clear();

            leftTabs.Add(new TabRecord("Trackers", () => leftTabActive = TabEnum.Trackers, () => leftTabActive == TabEnum.Trackers));
            leftTabs.Add(new TabRecord("Signals", () => leftTabActive = TabEnum.Signals, () => leftTabActive == TabEnum.Signals));

            rightTabs.Add(new TabRecord("Metrics", () => rightTabActive = TabEnum.Metrics, () => rightTabActive == TabEnum.Metrics));
            rightTabs.Add(new TabRecord("Derived", () => rightTabActive = TabEnum.Derived, () => rightTabActive == TabEnum.Derived));
        }
        public override void DoWindowContents(Rect inRect)
        {
            base.DoWindowContents(inRect);

            Rect leftTabRect = inRect.AtZero();
            leftTabRect.width /= 2;
            leftTabRect.yMin += 45f;
            TabDrawer.DrawTabs(leftTabRect, leftTabs);
            Widgets.DrawBoxSolid(leftTabRect, Color.gray);

            DrawLeftPane(leftTabRect);

            Rect rightTabRect = inRect.AtZero();
            rightTabRect.width /= 2;
            rightTabRect.x = rightTabRect.width + 5f;
            rightTabRect.width -= 5f;
            rightTabRect.yMin += 45f;
            TabDrawer.DrawTabs(rightTabRect, rightTabs);
            Widgets.DrawBoxSolid(rightTabRect, Color.green);
            
            DrawRightPane(rightTabRect);
        }

        private void DrawLeftPane(Rect outerPane)
        {
            using (new GuiGroup(outerPane))
            {
                outerPane = outerPane.AtZero();
                Rect innerPane = new Rect(outerPane);
                innerPane.width -= 10f;
                innerPane.height -= 10f;
                Widgets.DrawBoxSolid(innerPane, Color.red);
            }
        }

        private void DrawRightPane(Rect outerPane)
        {
            using (new GuiGroup(outerPane))
            {
                outerPane = outerPane.AtZero();
                Rect innerPane = new Rect(outerPane);
                innerPane.width -= 10f;
                innerPane.height -= 10f;
                Widgets.DrawBoxSolid(innerPane, Color.blue);
            }
        }
    }
}
