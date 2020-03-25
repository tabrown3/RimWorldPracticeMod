using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace ConfigurableGrowZone
{
    [StaticConstructorOnStartup]
    public class MainTabWindow_CustomStats : MainTabWindow
    {
        private CustomStatTabs tabs = new CustomStatTabs();

        public override void PreOpen()
        {
            base.PreOpen();

            tabs.PreOpen();
        }
        public override void DoWindowContents(Rect inRect)
        {
            base.DoWindowContents(inRect);

            Rect leftTabRect = inRect.AtZero();
            leftTabRect.width = leftTabRect.width/2f - 5f;
            leftTabRect.yMin += 45f;
            TabDrawer.DrawTabs(leftTabRect, tabs.LeftTabs);

            DrawPane(leftTabRect, tabs.LeftActiveTab);

            Rect rightTabRect = inRect.AtZero();
            rightTabRect.width = rightTabRect.width/2f - 5f;
            rightTabRect.x = rightTabRect.width + 10f;
            rightTabRect.yMin += 45f;
            TabDrawer.DrawTabs(rightTabRect, tabs.RightTabs);
            
            DrawPane(rightTabRect, tabs.RightActiveTab);
        }

        private void DrawPane(Rect outerPane, ITabDrawable<CustomStatsTab> activeTab)
        {
            using (new GuiGroup(outerPane))
            {
                outerPane = outerPane.AtZero();
                Widgets.DrawBox(outerPane);

                Rect innerPane = new Rect(outerPane);
                innerPane.width -= 10f;
                innerPane.height -= 10f;

                activeTab.DrawTab(innerPane);
            }
        }
    }
}
