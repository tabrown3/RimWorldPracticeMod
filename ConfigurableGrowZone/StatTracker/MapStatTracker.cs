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

        public void RegisterPowerStatTracker(CompStatTracker trackerComp)
        {
            if(!TrackerComps.Contains(trackerComp))
            {
                TrackerComps.Add(trackerComp);
            }
            else
            {
                Log.Message("TrackerComps already contains this trackerComp; why did it try to add itself again?");
            }
        }

        public void DeregisterPowerStatTracker(CompStatTracker trackerComp)
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
    }
}
