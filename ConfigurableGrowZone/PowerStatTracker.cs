using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace ConfigurableGrowZone
{
    public class PowerStatTracker : MapComponent
    {
        public readonly List<CompPowerStatTracker> TrackerComps = new List<CompPowerStatTracker>();

        public PowerStatTracker(Map map) : base(map)
        {
        }

        public override void FinalizeInit()
        {
            base.FinalizeInit();
        }

        public void RegisterPowerStatTracker(CompPowerStatTracker trackerComp)
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

        public void DeregisterPowerStatTracker(CompPowerStatTracker trackerComp)
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

        public override void MapComponentTick()
        {
            //var trackedPowerNets = TrackerComps.Select(u => u.parent?.TryGetComp<CompPower>()?.PowerNet).ToList();

            //for (var i = 0; i < trackedPowerNets.Count; i++)
            //{
            //    var trackedPowerNet = trackedPowerNets[i];
            //    if (trackedPowerNet != null)
            //    {
            //        var netStoredEnergyMax = trackedPowerNets[i].batteryComps.Sum(u => u.Props.storedEnergyMax); // Wd
            //        var netCurrentStoredEnergy = trackedPowerNets[i].CurrentStoredEnergy(); // Wd

            //        Log.Message($"Net {i} - Storage Max: {netStoredEnergyMax}Wd, Storage Current: {netCurrentStoredEnergy}Wd", true);

            //        var netCurrentEnergyGainRate = trackedPowerNets[i].CurrentEnergyGainRate(); // Wd/tick
            //    }
            //    else
            //    {
            //        Log.Message($"Thing at index {i} has a CompPowerStatTracker but not a CompPower; did you put a CompPowerStatTracker on a Thing that's not a Building_PowerStatTracker or other power-related Thing?");
            //    }
            //}
        }
    }
}
