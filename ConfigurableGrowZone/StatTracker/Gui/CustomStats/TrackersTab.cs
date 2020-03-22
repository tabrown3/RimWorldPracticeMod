using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ConfigurableGrowZone
{
    public class TrackersTab : ITabDrawable<CustomStatsTab>
    {
        public CustomStatsTab TabType => CustomStatsTab.Trackers;
        private readonly MapStatTracker statTracker;
        public TrackersTab()
        {
            statTracker = Find.CurrentMap.GetComponent<MapStatTracker>();
        }

        public void DrawTab(Rect pane)
        {
            Widgets.DrawBoxSolid(pane, Color.red);

            foreach(var trackerComp in statTracker.TrackerComps)
            {
                //trackerComp.Name
            }
        }
    }
}
