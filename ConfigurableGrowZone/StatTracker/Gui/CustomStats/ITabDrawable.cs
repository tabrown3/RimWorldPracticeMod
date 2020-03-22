using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ConfigurableGrowZone
{
    public interface ITabDrawable<T>
    {
        T TabType { get; }
        void DrawTab(Rect pane);
    }
}
