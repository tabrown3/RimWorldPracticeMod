using System;
using UniRx;

namespace ConfigurableGrowZone
{
    public interface IMetric
    {
        string Key { get; }
        string Name { get; }
        string Unit { get; }
        TimeDomain Domain { get; }

        IObservable<DataPoint> ValuePushed { get; }
    }
}