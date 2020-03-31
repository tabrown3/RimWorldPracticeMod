using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableGrowZone.StatTracker.Gui.CustomStats.AddMetric.Derived
{
    public class AddOperatorListModel : IValidatable
    {
        public List<AddOperatorRowModel> TopRows => Rows.Take(Rows.Count - 1).ToList();
        public AddOperatorRowModel BottomRow => Rows.Count > 0 ? Rows.Last() : null;

        public List<AddOperatorRowModel> Rows = new List<AddOperatorRowModel>();
        public bool IsValid()
        {
            if (BottomRow.IsEmpty())
            {
                return TopRows.All(u => u.IsValid());
            }
            else
            {
                return Rows.All(u => u.IsValid());
            }
        }
    }
}
