using System;

namespace ConfigurableGrowZone
{
    public interface IMetric
    {
        string Key { get; }
        string Name { get; }
        string Unit { get; }
        TimeDomain Domain { get; }

        event EventHandler<DataPointEventArgs> ValuePushed;
    }
}