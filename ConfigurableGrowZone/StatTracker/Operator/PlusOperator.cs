using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableGrowZone
{
    public class PlusOperator : BinaryOperator<float>
    {
        public override float Call()
        {
            return First + Second;
        }
    }
}
