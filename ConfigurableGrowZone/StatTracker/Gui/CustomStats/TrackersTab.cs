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
        public List<CompStatTracker> StatTrackers => Find.CurrentMap?.GetComponent<MapStatTracker>()?.TrackerComps;

        public void DrawTab(Rect pane)
        {
            Widgets.DrawBoxSolid(pane, Color.red);

            Text.Font = GameFont.Small;
            GUI.color = Color.white;

            float trackerRectHeight = 20f;
            for(int i = 0; i < StatTrackers.Count; i++)
            {
                Rect trackerRect = new Rect(pane);
                trackerRect.y = i * trackerRectHeight;
                trackerRect.height = trackerRectHeight;

                Widgets.Label(trackerRect, StatTrackers[i].Name);
            }
        }
    }
}
