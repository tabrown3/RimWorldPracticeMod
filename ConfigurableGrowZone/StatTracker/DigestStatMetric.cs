using System;
using System.Linq;
using Verse;

namespace ConfigurableGrowZone
{
    public class DigestStatMetric : StatMetric
    {
        

        private readonly int resInTicks;
        
        private float[] values;
        private readonly Func<float[], float> reductionFunc;

        private bool isInitialTick = true;

        private int indexPos;
        
        public DigestStatMetric(string key, string name, Func<float> metricValueFunc, string unit, GameTime.InTicks resolution = GameTime.InTicks.Hour, Func<float[], float> reductionFunc = null) : base(key, name, metricValueFunc, unit, resolution)
        {
            this.resInTicks = (int)this.Resolution;

            if (reductionFunc == null)
            {
                this.reductionFunc = u => u.Average();
            }
            else
            {
                this.reductionFunc = reductionFunc;
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

            if (ShouldDigest(gameTick))
            {
                PushValue(gameTick, Digest());

                this.values = new float[this.resInTicks];
                indexPos = 0;
            }
        }

        private float Digest()
        {
            return reductionFunc(values);
        }

        private bool ShouldDigest(int gameTick)
        {
            return gameTick % resInTicks == resInTicks - 1; // should digest on the last tick of the period
        }
    }
}
