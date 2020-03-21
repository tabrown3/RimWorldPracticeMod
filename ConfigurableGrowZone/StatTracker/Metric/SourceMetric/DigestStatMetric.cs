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
        
        public DigestStatMetric(string key, string name, IPullable<float> source, string unit, TimeDomain domain, IAggregator<float> aggregator = null) : base(key, name, source, unit, domain, aggregator)
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

            values[indexPos] = source.PullValue();
            indexPos++;

            if (Domain.IsResolutionBoundary(gameTick))
            {
                PushValue(gameTick, aggregator.Aggregate(values));

                this.values = new float[this.Domain.ResInTicks];
                indexPos = 0;
            }
        }

        public override List<float> GetInternalState()
        {
            return values.ToList();
        }

        public override void SetInternalState(List<float> state)
        {
            if(state != null)
            {
                values = state.ToArray();
            }
        }
    }
}
