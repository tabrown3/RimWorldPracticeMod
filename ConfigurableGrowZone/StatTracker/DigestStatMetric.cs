using System;
using System.Linq;
using Verse;

namespace ConfigurableGrowZone
{
    public class DigestStatMetric : StatMetric
    {
        private float[] values;
        private readonly Func<float[], float> reductionFunc;

        private bool isInitialTick = true;

        private int indexPos;
        
        public DigestStatMetric(string key, string name, Func<float> metricValueFunc, string unit, GameTime.InTicks resolution = GameTime.InTicks.Hour, Func<float[], float> digestFunc = null) : base(key, name, metricValueFunc, unit, resolution)
        {
            if (digestFunc == null)
            {
                this.reductionFunc = u => u.Average();
            }
            else
            {
                this.reductionFunc = digestFunc;
            }
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
                PushValue(gameTick, reductionFunc(values));

                this.values = new float[this.resInTicks];
                indexPos = 0;
            }
        }
    }
}
