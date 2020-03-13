using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigurableGrowZone
{
    public class WindowStatMetric : SetStatMetric
    {
        private readonly LinkedList<float> values = new LinkedList<float>();
        private readonly int windowSize;

        public WindowStatMetric(string key, string name, Func<float> metricValueFunc, string unit, GameTime.InTicks resolution = GameTime.InTicks.Hour, Func<IEnumerable<float>, float> aggregator = null, int? windowSize = null) : base(key, name, metricValueFunc, unit, resolution, aggregator)
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
                this.windowSize = resInTicks;
            }
        }

        public override void Tick(int gameTick)
        {
            values.AddFirst(metricValueFunc());

            if (values.Count > windowSize)
            {
                values.RemoveLast();
            }

            if (ShouldPushValue(gameTick))
            {
                PushValue(gameTick, aggregator(values));
            }
        }
    }
}
