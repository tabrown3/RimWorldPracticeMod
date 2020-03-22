using System;
using UnityEngine;

namespace ConfigurableGrowZone
{
    public class GuiGroup : IDisposable
    {
        public GuiGroup(Rect rect)
        {
            GUI.BeginGroup(rect);
        }

        public void Dispose()
        {
            GUI.EndGroup();
        }
    }
}
