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

        //event EventHandler<DataPointEventArgs> ValuePushed;

        IObservable<DataPoint> ValuePushed { get; }
    }
}