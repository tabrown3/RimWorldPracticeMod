using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableGrowZone
{
    public class AddOperatorRowModel : IValidatable
    {
        public Type ChosenOperator = null;
        public string ChosenTrackerName = "";
        public SourceMetric ChosenSourceMetric = null;

        public bool ChosenOperatorIsBinary = false;

        public bool IsValid()
        {
            bool hasOperator = ChosenOperator != null;

            bool isValid;
            if (hasOperator && ChosenOperatorIsBinary)
            {
                isValid = !string.IsNullOrEmpty(ChosenTrackerName) && ChosenSourceMetric != null;
            }
            else
            {
                isValid = hasOperator;
            }

            return isValid;
        }

        public bool IsEmpty()
        {
            return ChosenOperator == null && string.IsNullOrEmpty(ChosenTrackerName) && ChosenSourceMetric == null;
        }
    }
}
