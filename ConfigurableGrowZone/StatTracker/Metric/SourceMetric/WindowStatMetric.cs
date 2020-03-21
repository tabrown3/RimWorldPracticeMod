using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigurableGrowZone
{
    public class WindowStatMetric : SetStatMetric
    {
        private LinkedList<float> values = new LinkedList<float>();
        private readonly int windowSize;

        public WindowStatMetric(string key, string name, IPullable<float> source, string unit, TimeDomain domain, IAggregator<float> aggregator = null, int? windowSize = null) : base(key, name, source, unit, domain, aggregator)
        {
            if(windowSize.HasValue)
            {
                if(windowSize.Value <= 0)
                {
                    throw new Exception("WindowStatMetric windowSize must be greater than 0");
                }
                
                this.windowSize = windowSize.Value;
            }
            else
            {
                this.windowSize = this.Domain.ResInTicks;
            }
        }

        public override void Tick(int gameTick)
        {
            values.AddFirst(source.PullValue());

            if (values.Count > windowSize)
            {
                values.RemoveLast();
            }

            if (Domain.IsResolutionBoundary(gameTick))
            {
                PushValue(gameTick, aggregator.Aggregate(values));
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
                values = new LinkedList<float>(state);
            }
        }
    }
}
