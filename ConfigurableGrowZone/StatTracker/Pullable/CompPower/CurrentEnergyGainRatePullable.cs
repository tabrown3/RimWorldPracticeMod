using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ConfigurableGrowZone
{
    public class CurrentEnergyGainRatePullable : IPullable<float>
    {
        public Func<float> PullValue { get; }
        public CurrentEnergyGainRatePullable(ThingWithComps thing)
        {
            PullValue = () => thing.GetComp<CompPower>().PowerNet.CurrentEnergyGainRate();
        }
    }
}
