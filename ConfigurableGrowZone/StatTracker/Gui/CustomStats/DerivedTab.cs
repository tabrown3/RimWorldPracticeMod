using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ConfigurableGrowZone
{
    public class DerivedTab : ICustomStatsTab
    {
        public ActiveTab TabType => ActiveTab.Derived;

        public void DrawTab(Rect pane)
        {
            Widgets.DrawBoxSolid(pane, Color.blue);
        }
    }
}
