using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigurableGrowZone
{
    public class DataPointEventArgs : EventArgs
    {
        public DataPoint DataPoint { get; }

        public DataPointEventArgs(DataPoint dataPoint)
        {
            DataPoint = dataPoint;
        }
    }
}
