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

        public override void PostClose()
        {
            base.PostClose();

            tabs.PostClose();
        }

        public override void DoWindowContents(Rect inRect)
        {
            base.DoWindowContents(inRect);

            float tabWidth = inRect.width / 2f - 5f;

            Rect leftTabRect = inRect.AtZero();
            leftTabRect.width = tabWidth;
            leftTabRect.yMin += 45f;
            TabDrawer.DrawTabs(leftTabRect, tabs.LeftTabs);

            DrawPane(leftTabRect, tabs.LeftActiveTab);

            Rect rightTabRect = inRect.AtZero();
            rightTabRect.width = tabWidth;
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
                Widgets.DrawMenuSection(outerPane);

                Rect innerPane = new Rect(outerPane);
                innerPane.x = 10f;
                innerPane.width -= 20f;
                innerPane.height -= 10f;

                activeTab.DrawTab(innerPane);
            }
        }
    }
}
