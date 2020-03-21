using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace ConfigurableGrowZone
{
    public class CompPowerStatTracker : ThingComp
    {
        public string Name { get; set; }
        public PowerStatData Data { get; private set; }
        public CompProperties_PowerStatTracker Props => (CompProperties_PowerStatTracker)props;
        public PowerNet PowerNet => parent.GetComp<CompPower>()?.PowerNet;

        public CompPowerStatTracker()
        {
        }

        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);

            var latLong = GetLatLong(parent);

            Name = nameof(CompPowerStatTracker) + latLong;
            Data = new PowerStatData(latLong);

            var firstMetric = new PollStatMetric(
                    "StoredEnergyEachHourPoll",
                    "Stored Energy at Hour",
                    new CurrentStoredEnergyPullable(parent),
                    "Wd",
                    new TwentyFourHourDomain()
                );
            this.AddSourceMetric(firstMetric);

            this.AddSourceMetric(
                new DigestStatMetric(
                    "EnergyGainByHourDigest",
                    "Energy per Hour",
                    new CurrentEnergyGainRatePullable(parent),
                    "Wd/h",
                    new TwentyFourHourDomain(),
                    new SumAggregator()
                )
            );

            this.AddSourceMetric(
                new WindowStatMetric(
                    "EnergyGainByQuarterHourWindow",
                    "Energy per Quarter Hour",
                    new CurrentEnergyGainRatePullable(parent),
                    "Wd/qt.h",
                    new QuarterHourDomain(),
                    new SumAggregator()
                )
            );

            this.AddDerivedMetric(
                new DerivedMetric(
                    "NegativeStoredEnergyEachHourPoll",
                    "Negative Stored Energy at Hour",
                    new List<SourceMetric>() { firstMetric },
                    new List<IOperator<float>>() { new NegateOperator() }
                )
            );
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);

            // Using Find.CurrentMap since parent.Map is still null at this point
            Find.CurrentMap.GetComponent<PowerStatTracker>().RegisterPowerStatTracker(this);
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            base.PostDestroy(mode, previousMap);

            previousMap.GetComponent<PowerStatTracker>().DeregisterPowerStatTracker(this);
        }

        public void AddSourceMetric(SourceMetric metric)
        {
            Data.AddSourceMetric(metric);
        }

        public void AddDerivedMetric(DerivedMetric derivedMetric)
        {
            Data.AddDerivedMetric(derivedMetric);
        }

        public override void CompTick()
        {
            base.CompTick();

            int ticksGame = Find.TickManager.TicksGame;

            foreach (SourceMetric metric in Data.SourceMetrics)
            {
                metric.Tick(ticksGame);
            }

            foreach (DerivedMetric derivedMetric in Data.DerivedMetrics)
            {
                derivedMetric.Tick(ticksGame, Data.History.History);
            }
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (var gizmo in base.CompGetGizmosExtra())
            {
                yield return gizmo;
            }

            Command_Action command_Action = new Command_Action();
            command_Action.action = delegate
            {
                List<FloatMenuOption> list = new List<FloatMenuOption>();
                foreach (var metric in Data.SourceMetrics)
                {
                    list.Add(new FloatMenuOption(metric.Name, delegate
                    {
                        Find.WindowStack.Add(new Dialog_PowerStatTracker(Data.History.Get(metric.Key))); // TODO: make this not break if key DNE in Dict
                    }));
                }
                foreach (var derivedMetric in Data.DerivedMetrics)
                {
                    list.Add(new FloatMenuOption(derivedMetric.Name, delegate
                    {
                        Find.WindowStack.Add(new Dialog_PowerStatTracker(Data.History.Get(derivedMetric.Key))); // TODO: make this not break if key DNE in Dict
                    }));
                }
                Find.WindowStack.Add(new FloatMenu(list));
            };
            command_Action.defaultLabel = "View Stats";
            //command_Action.defaultDesc = "View reaouts of stats currently being tracked.";
            command_Action.hotKey = KeyBindingDefOf.Misc5;
            command_Action.icon = ContentFinder<Texture2D>.Get("UI/Commands/TempLower");
            yield return command_Action;
        }

        public override void PostExposeData()
        {
            Data.PersistData();
        }

        private Vector2 GetLatLong(Thing thing) // taken from game GenLocalDate.LocationForDate
        {
            int tile = thing.Tile;
            if (tile >= 0)
            {
                return Find.WorldGrid.LongLatOf(tile);
            }
            return Vector2.zero;
        }
    }
}
