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

            //foreach(var sourceMetricProps in props.SourceMetrics)
            //{

            //}

            var firstMetric = SpitOutMetric("ConfigurableGrowZone.PollSourceMetric", "StoredEnergyEachHourPoll", "Stored Energy at Hour", "ConfigurableGrowZone.CurrentStoredEnergyPullable", "Wd", "ConfigurableGrowZone.TwentyFourHourDomain");
            this.AddSourceMetric(firstMetric);

            var secondMetric = SpitOutMetric("ConfigurableGrowZone.DigestSourceMetric", "EnergyGainByHourDigest", "Energy per Hour", "ConfigurableGrowZone.CurrentEnergyGainRatePullable", "Wd/h", "ConfigurableGrowZone.TwentyFourHourDomain", "ConfigurableGrowZone.SumAggregator");
            this.AddSourceMetric(secondMetric);

            var thirdMetric = SpitOutMetric("ConfigurableGrowZone.WindowSourceMetric", "EnergyGainByQuarterHourWindow", "Energy per Quarter Hour", "ConfigurableGrowZone.CurrentEnergyGainRatePullable", "Wd/qt.h", "ConfigurableGrowZone.QuarterHourDomain", "ConfigurableGrowZone.SumAggregator");
            this.AddSourceMetric(thirdMetric);

            this.AddDerivedMetric(
                new DerivedMetric(
                    "NegativeStoredEnergyEachHourPoll",
                    "Negative Stored Energy at Hour",
                    new List<SourceMetric>() { firstMetric },
                    new List<IOperator<float>>() { new NegateOperator() }
                )
            );

            this.AddDerivedMetric(
                new DerivedMetric(
                    "DoubleStoredEnergyNegatedEachHourPoll",
                    "Summed Negative Stored Something at Hour",
                    new List<SourceMetric>() { firstMetric, secondMetric },
                    new List<IOperator<float>>() { new PlusOperator(), new NegateOperator() }
                )
            );
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

        private SourceMetric SpitOutMetric(string metricTypeName, string key, string name, string sourceTypeName, string unit, string domainTypeName, string aggregatorTypeName = null)
        {
            var metricType = Type.GetType(metricTypeName);
            var sourceType = Type.GetType(sourceTypeName);
            var domainType = Type.GetType(domainTypeName);

            SourceMetric metric;
            if (metricType == typeof(PollSourceMetric))
            {
                metric = new PollSourceMetric(
                    key,
                    name,
                    (IPullable<float>)Activator.CreateInstance(sourceType, new object[] { parent }),
                    unit,
                    (TimeDomain)Activator.CreateInstance(domainType)
                );
            }
            else if (metricType == typeof(DigestSourceMetric))
            {
                var aggregatorType = Type.GetType(aggregatorTypeName);

                metric = new DigestSourceMetric(
                    key,
                    name,
                    (IPullable<float>)Activator.CreateInstance(sourceType, new object[] { parent }),
                    unit,
                    (TimeDomain)Activator.CreateInstance(domainType),
                    (IAggregator<float>)Activator.CreateInstance(aggregatorType)
                );
            }
            else if (metricType == typeof(WindowSourceMetric))
            {
                var aggregatorType = Type.GetType(aggregatorTypeName);

                metric = new WindowSourceMetric(
                    key,
                    name,
                    (IPullable<float>)Activator.CreateInstance(sourceType, new object[] { parent }),
                    unit,
                    (TimeDomain)Activator.CreateInstance(domainType),
                    (IAggregator<float>)Activator.CreateInstance(aggregatorType)
                );
            }
            else
            {
                throw new Exception($"Error creating type {nameof(metricType)}: Custom metric types not supported at this time");
            }

            return metric;
        }
    }
}
