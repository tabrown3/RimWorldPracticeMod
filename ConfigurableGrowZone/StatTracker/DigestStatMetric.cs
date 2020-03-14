using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace ConfigurableGrowZone
{
    public class DigestStatMetric : SetStatMetric
    {
        private float[] values;
        private bool isInitialTick = true;
        private int indexPos;
        
        public DigestStatMetric(string key, string name, Func<float> metricValueFunc, string unit, TimeDomain domain, Func<IEnumerable<float>, float> aggregator = null) : base(key, name, metricValueFunc, unit, domain, aggregator)
        {
        }

        public override void Tick(int gameTick)
        {
            if (isInitialTick)
            {
                this.values = new float[this.Domain.ResInTicks - (gameTick % this.Domain.ResInTicks)];
                indexPos = 0;

                isInitialTick = false;
            }

            values[indexPos] = metricValueFunc();
            indexPos++;

            if (ShouldPushValue(gameTick))
            {
                PushValue(gameTick, aggregator(values));

                this.values = new float[this.Domain.ResInTicks];
                indexPos = 0;
            }
        }
    }
}
