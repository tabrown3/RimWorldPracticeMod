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

            MetricSetup();
        }

        public override void ExposeData()
        {


        }

        private void MetricSetup()
        {
            CompPowerStatTracker.AddMetric(
                new PollStatMetric(
                    "StoredEnergyEachHour",
                    "Stored Energy at Hour",
                    () => CompPower.PowerNet.CurrentStoredEnergy(),
                    "Wd",
                    new TwentyFourHourDomain()
                )
            );

            CompPowerStatTracker.AddMetric(
                new DigestStatMetric(
                    "EnergyGainByHourDigest",
                    "Energy per Hour D",
                    () => CompPower.PowerNet.CurrentEnergyGainRate(),
                    "Wd",
                    new TwentyFourHourDomain(),
                    aggregator: u => u.Sum()
                )
            );

            // If windowSize is equal to resolution, WindowStatMetric behaves the same as DigestStatMetric
            CompPowerStatTracker.AddMetric(
                new WindowStatMetric(
                    "EnergyGainByQuarterHourWindow",
                    "Energy per Quarter Hour W",
                    () => CompPower.PowerNet.CurrentEnergyGainRate(),
                    "Wd",
                    new QuarterHourDomain(),
                    aggregator: u => u.Sum()
                )
            );
        }

        private void Persist<T>(string key, T data, Action<T> persistCb)
        {
            Scribe_Deep.Look(ref data, key);

            persistCb(data);
        }
    }
}
