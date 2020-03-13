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
                new PollStatMetric(
                    "StoredEnergyByHour",
                    "Stored Energy by Hour",
                    () => CompPower.PowerNet.CurrentStoredEnergy(),
                    "Wd"
                )
            );

            CompPowerStatTracker.AddMetric(
                new DigestStatMetric(
                    "EnergyGainByHourDigest",
                    "Energy Gain by Hour D",
                    () => CompPower.PowerNet.CurrentEnergyGainRate(),
                    "Wd",
                    aggregator: u => u.Sum()
                )
            );

            // If windowSize is equal to resolution, WindowStatMetric behaves the same as DigestStatMetric
            CompPowerStatTracker.AddMetric(
                new WindowStatMetric(
                    "EnergyGainByHourWindow",
                    "Energy Gain by Hour W",
                    () => CompPower.PowerNet.CurrentEnergyGainRate(),
                    "Wd",
                    aggregator: u => u.Sum()
                )
            );
        }
    }
}
