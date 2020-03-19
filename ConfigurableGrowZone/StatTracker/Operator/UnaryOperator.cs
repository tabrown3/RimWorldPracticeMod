using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableGrowZone
{
    public abstract class UnaryOperator<T> : IOperator<T>
    {
        public T First { get; set; }
        public abstract T Call();
    }
}
