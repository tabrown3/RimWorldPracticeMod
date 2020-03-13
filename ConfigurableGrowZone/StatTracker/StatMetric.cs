using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigurableGrowZone
{
    public abstract class StatMetric
    {
        public readonly string Key;
        public readonly string Name;
        public readonly GameTime.InTicks Resolution;
        public readonly string Unit;

        public event EventHandler<DataPointEventArgs> ValuePushed;

        protected readonly Func<float> metricValueFunc;

        public StatMetric(string key, string name, Func<float> metricValueFunc, string unit, GameTime.InTicks resolution)
        {
            this.Key = key;
            this.Name = name;
            this.Unit = unit;
            this.Resolution = resolution;

            this.metricValueFunc = metricValueFunc;
        }

        public virtual void Tick(int gameTick)
        {
        }

        protected void PushValue(int gameTick, float value)
        {
            ValuePushed.Invoke(this, new DataPointEventArgs(new DataPoint(gameTick, value)));
        }
    }
}
