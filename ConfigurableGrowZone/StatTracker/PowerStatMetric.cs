using System;
using System.Linq;
using Verse;

namespace ConfigurableGrowZone
{
    public class PowerStatMetric
    {
        public readonly string Key;
        public readonly GameTime.InTicks Resolution;
        public event EventHandler<DataPointEventArgs> OnDigest;
        public int InitialTick;

        private readonly int resInTicks;
        private readonly Func<float> metricValueFunc;
        private float[] values;
        private readonly Func<float[], float> reductionFunc;

        private bool isInitialTick = true;

        private int indexPos;
        
        public PowerStatMetric(string key, Func<float> metricValueFunc, GameTime.InTicks resolution = GameTime.InTicks.Hour, Func<float[], float> reductionFunc = null)
        {
            this.Key = key;
            this.Resolution = resolution;

            this.resInTicks = (int)this.Resolution;
            this.metricValueFunc = metricValueFunc;

            if (reductionFunc == null)
            {
                this.reductionFunc = u => u.Average();
            }
            else
            {
                this.reductionFunc = reductionFunc;
            }
        }

        public void Tick(int gameTick)
        {
            if (isInitialTick)
            {
                InitialTick = gameTick;
                this.values = new float[this.resInTicks - (InitialTick % this.resInTicks)];
                indexPos = 0;

                isInitialTick = false;
            }

            values[indexPos] = metricValueFunc();
            indexPos++;

            if (ShouldDigest(gameTick))
            {
                OnDigest.Invoke(this, new DataPointEventArgs(new DataPoint() {
                    DigestValue = Digest(),
                    Key = Key,
                    Resolution = Resolution,
                    TimeStamp = gameTick
                }));

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
