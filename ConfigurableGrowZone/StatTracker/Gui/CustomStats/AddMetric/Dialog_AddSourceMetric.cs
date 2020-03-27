using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ConfigurableGrowZone
{
    public class Dialog_AddSourceMetric : Window
    {
        private string name = "";
        private string key = "";

        private readonly List<Type> domains;
        private readonly List<Type> sources;
        private readonly List<Type> aggregators;

        public Dialog_AddSourceMetric(List<Type> domains, List<Type> sources, List<Type> aggregators)
        {
            this.domains = domains;
            this.sources = sources;
            this.aggregators = aggregators;

            this.closeOnClickedOutside = true;
            this.doCloseX = true;
            this.focusWhenOpened = true;
            this.absorbInputAroundWindow = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            new RectStacker(inRect)
                .Then(u => DrawTextEntry(u, "Name", name, v => name = v))
                .ThenGap(15f)
                .Then(u => DrawTextEntry(u, "Key", key, v => key = v))
                .ThenGap(15f)
                .Then(u =>
                {
                    return new RectStacker(u)
                        .Then(v => DrawRadioButton(v, "Poll"))
                        .ThenGap(10f)
                        .Then(v => DrawRadioButton(v, "Digest"))
                        .ThenGap(10f)
                        .Then(v => DrawRadioButton(v, "Window"));
                })
                .ThenGap(15f)
                .Then(u => DrawTextButton(u, "Domain", domains))
                .Then(u => DrawTextButton(u, "Source", sources))
                .Then(u => DrawTextButton(u, "Aggregator", aggregators));
        }

        private Rect DrawTextEntry(Rect inRect, string label, string inVal, Action<string> outValCb)
        {
            Rect nameEntryRect = new Rect(inRect);
            nameEntryRect.height = 30f;
            nameEntryRect.width = 200f;
            outValCb(Widgets.TextEntryLabeled(nameEntryRect, label, inVal));

            return nameEntryRect;
        }

        private Rect DrawRadioButton(Rect inRect, string label)
        {
            Rect radioButtonRect = new Rect(inRect);
            radioButtonRect.width = 100f;
            radioButtonRect.height = 20f;
            if (Widgets.RadioButtonLabeled(radioButtonRect, label, false))
            {

            }

            return radioButtonRect;
        }

        private Rect DrawTextButton(Rect inRect, string label, List<Type> typeList)
        {
            Rect textButtonRect = new Rect(inRect);
            textButtonRect.width = 100f;
            textButtonRect.height = 35f;
            if (Widgets.ButtonText(textButtonRect, label))
            {
                List<FloatMenuOption> list = typeList.Select(v => new FloatMenuOption(v.Name, delegate
                {
                    Log.Message($"Chosen thinger: {v.FullName}");
                })).ToList();

                Find.WindowStack.Add(new FloatMenu(list));
            }

            return textButtonRect;
        }
    }
}
