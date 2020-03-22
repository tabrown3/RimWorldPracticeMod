using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ConfigurableGrowZone
{
    public class DerivedTab : ITabDrawable<CustomStatsTab>
    {
        public CustomStatsTab TabType => CustomStatsTab.Derived;

        public void DrawTab(Rect pane)
        {
            Widgets.DrawBoxSolid(pane, Color.blue);
        }
    }
}
