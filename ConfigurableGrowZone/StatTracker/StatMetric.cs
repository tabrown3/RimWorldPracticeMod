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

        protected readonly int resInTicks;

        public StatMetric(string key, string name, Func<float> metricValueFunc, string unit, GameTime.InTicks resolution)
        {
            this.Key = key;
            this.Name = name;
            this.Unit = unit;
            this.Resolution = resolution;

            this.metricValueFunc = metricValueFunc;

            this.resInTicks = (int)this.Resolution;
        }

        public abstract void Tick(int gameTick);

        protected virtual bool ShouldPushValue(int gameTick)
        {
            return gameTick % resInTicks == resInTicks - 1; // should digest on the last tick of the period
        }

        protected void PushValue(int gameTick, float value)
        {
            ValuePushed.Invoke(this, new DataPointEventArgs(new DataPoint(gameTick, value)));
        }
    }
}
