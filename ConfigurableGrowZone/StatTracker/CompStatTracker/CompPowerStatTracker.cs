using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace ConfigurableGrowZone
{
    public class CompPowerStatTracker : CompStatTracker
    {
        public CompProperties_PowerStatTracker Props => (CompProperties_PowerStatTracker)props;
        public PowerNet PowerNet => parent.GetComp<CompPower>()?.PowerNet;

        public CompPowerStatTracker()
        {
        }

        public override void Initialize(CompProperties baseProps)
        {
            base.Initialize(baseProps);
            CompProperties_PowerStatTracker props = (CompProperties_PowerStatTracker)baseProps;



            //var firstMetric = new PollStatMetric(
            //        "StoredEnergyEachHourPoll",
            //        "Stored Energy at Hour",
            //        new CurrentStoredEnergyPullable(parent),
            //        "Wd",
            //        new TwentyFourHourDomain()
            //    );
            var metricTypeName = typeof(PollSourceMetric).FullName;
            var metricType = Type.GetType(metricTypeName);
            var pullableTypeName = typeof(CurrentStoredEnergyPullable).FullName;
            var pullableType = Type.GetType(pullableTypeName);
            var domainTypeName = typeof(TwentyFourHourDomain).FullName;
            var domainType = Type.GetType(domainTypeName);
            var firstMetric = (PollSourceMetric)Activator.CreateInstance(metricType, new object[] {
                "StoredEnergyEachHourPoll",
                "Stored Energy at Hour",
                Activator.CreateInstance(pullableType, new object[] { parent }),
                "Wd",
                Activator.CreateInstance(domainType)
            });
            this.AddSourceMetric(firstMetric);

            //var secondMetric = new DigestStatMetric(
            //        "EnergyGainByHourDigest",
            //        "Energy per Hour",
            //        new CurrentEnergyGainRatePullable(parent),
            //        "Wd/h",
            //        new TwentyFourHourDomain(),
            //        new SumAggregator()
            //    );
            //this.AddSourceMetric(secondMetric);

            //this.AddSourceMetric(
            //    new WindowStatMetric(
            //        "EnergyGainByQuarterHourWindow",
            //        "Energy per Quarter Hour",
            //        new CurrentEnergyGainRatePullable(parent),
            //        "Wd/qt.h",
            //        new QuarterHourDomain(),
            //        new SumAggregator()
            //    )
            //);

            //this.AddDerivedMetric(
            //    new DerivedMetric(
            //        "NegativeStoredEnergyEachHourPoll",
            //        "Negative Stored Energy at Hour",
            //        new List<SourceMetric>() { firstMetric },
            //        new List<IOperator<float>>() { new NegateOperator() }
            //    )
            //);

            //this.AddDerivedMetric(
            //    new DerivedMetric(
            //        "DoubleStoredEnergyNegatedEachHourPoll",
            //        "Summed Negative Stored Something at Hour",
            //        new List<SourceMetric>() { firstMetric, secondMetric },
            //        new List<IOperator<float>>() { new PlusOperator(), new NegateOperator() }
            //    )
            //);
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
                        Find.WindowStack.Add(new Dialog_BarChart(Data.History.Get(metric.Key))); // TODO: make this not break if key DNE in Dict
                    }));
                }
                foreach (var derivedMetric in Data.DerivedMetrics)
                {
                    list.Add(new FloatMenuOption(derivedMetric.Name, delegate
                    {
                        Find.WindowStack.Add(new Dialog_BarChart(Data.History.Get(derivedMetric.Key))); // TODO: make this not break if key DNE in Dict
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
    }
}
