using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ConfigurableGrowZone
{
    public class Building_PowerStatTracker : Building
    {
        public override bool TransmitsPowerNow => true;
        public CompPower CompPower => GetComp<CompPower>();
        public CompPowerStatTracker CompPowerStatTracker => GetComp<CompPowerStatTracker>();

        public Building_PowerStatTracker()
        {
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);

            CompPowerStatTracker.AddMetric(
                new PowerStatMetric(
                    "AvgStoredEnergyEachHour",
                    () => CompPower.PowerNet.CurrentStoredEnergy()
                )
            );

            CompPowerStatTracker.AddMetric(
                new PowerStatMetric(
                    "AvgEnergyGainEachHour",
                    () => CompPower.PowerNet.CurrentEnergyGainRate(),
                    reductionFunc: u => u.Sum()
                )
            );
        }
    }
}
