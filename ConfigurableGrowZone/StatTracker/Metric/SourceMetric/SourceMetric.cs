using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;

namespace ConfigurableGrowZone
{
    public abstract class SourceMetric : IMetric
    {
        public string Key { get; }
        public string Name { get; }
        public string Unit { get; }
        public TimeDomain Domain { get; }
        
        public IObservable<DataPoint> ValuePushed => valuePushed;

        protected readonly IPullable<float> source;

        private Subject<DataPoint> valuePushed = new Subject<DataPoint>();

        public SourceMetric(string key, string name, IPullable<float> source, string unit, TimeDomain domain)
        {
            this.Key = key;
            this.Name = name;
            this.Unit = unit;
            this.Domain = domain;

            this.source = source;
        }

        public abstract void Tick(int gameTick);

        protected void PushValue(int gameTick, float value)
        {
            valuePushed.OnNext(new DataPoint(gameTick, value));
        }
    }
}
