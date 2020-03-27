using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace ConfigurableGrowZone
{
    public class MapStatTracker : MapComponent
    {
        public readonly List<CompStatTracker> TrackerComps = new List<CompStatTracker>();

        public MapStatTracker(Map map) : base(map)
        {
        }

        public int RegisterStatTracker(CompStatTracker trackerComp)
        {
            if(!TrackerComps.Contains(trackerComp))
            {
                TrackerComps.Add(trackerComp);
                return TrackerComps.Count;
            }
            else
            {
                Log.Message("TrackerComps already contains this trackerComp; why did it try to add itself again?");
                return -1;
            }
        }

        public void DeregisterStatTracker(CompStatTracker trackerComp)
        {
            if (TrackerComps.Contains(trackerComp))
            {
                TrackerComps.Remove(trackerComp);
            }
            else
            {
                Log.Message("TrackerComps tried to remove a trackerComp it does not contain");
            }
        }

        public SourceMetric GetMetric(string trackerName, string metricKey)
        {
            return TrackerComps.SingleOrDefault(u => u.Name == trackerName)?.Data.SourceMetrics.SingleOrDefault(u => u.Key == metricKey);
        }
    }
}
