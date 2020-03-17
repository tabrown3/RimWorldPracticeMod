using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableGrowZone
{
    public interface IAggregator<T>
    {
        Func<IEnumerable<T>, T> Aggregate { get; }
    }
}
