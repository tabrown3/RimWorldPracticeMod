using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using Verse;

namespace ConfigurableGrowZone
{
    public class TrackersTab : ITabDrawable<CustomStatsTab>
    {
        public CustomStatsTab TabType => CustomStatsTab.Trackers;
        public List<CompStatTracker> StatTrackers => Find.CurrentMap?.GetComponent<MapStatTracker>()?.TrackerComps;
        public IObservable<CompStatTracker> OnListItemClick => onListItemClick;

        private readonly Subject<CompStatTracker> onListItemClick = new Subject<CompStatTracker>();

        public void DrawTab(Rect pane)
        {
            Widgets.DrawBoxSolid(pane, Color.red);

            Text.Font = GameFont.Small;
            GUI.color = Color.white;

            float trackerRectHeight = 20f;
            for(int i = 0; i < StatTrackers.Count; i++)
            {
                var statTracker = StatTrackers[i];

                Rect trackerRect = new Rect(pane);
                trackerRect.y = i * trackerRectHeight;
                trackerRect.height = trackerRectHeight;

                Widgets.Label(trackerRect, statTracker.Name);

                if(Event.current.type == EventType.MouseDown && Event.current.button == 0 && Mouse.IsOver(trackerRect))
                {
                    onListItemClick.OnNext(statTracker);
                }
            }
        }
    }
}
