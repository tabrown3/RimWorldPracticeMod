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
        
        public DigestStatMetric(string key, string name, Func<float> metricValueFunc, string unit, GameTime.InTicks resolution = GameTime.InTicks.Hour, Func<IEnumerable<float>, float> aggregator = null) : base(key, name, metricValueFunc, unit, resolution, aggregator)
        {
        }

        public override void Tick(int gameTick)
        {
            if (isInitialTick)
            {
                this.values = new float[this.resInTicks - (gameTick % this.resInTicks)];
                indexPos = 0;

                isInitialTick = false;
            }

            values[indexPos] = metricValueFunc();
            indexPos++;

            if (ShouldPushValue(gameTick))
            {
                PushValue(gameTick, aggregator(values));

                this.values = new float[this.resInTicks];
                indexPos = 0;
            }
        }
    }
}
