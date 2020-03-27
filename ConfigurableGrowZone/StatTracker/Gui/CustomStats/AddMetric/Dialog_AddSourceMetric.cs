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

        public Dialog_AddSourceMetric()
        {
            this.closeOnClickedOutside = true;
            this.doCloseX = true;
            this.focusWhenOpened = true;
            this.absorbInputAroundWindow = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            float textInputHeight = 30f;

            new RectStacker(inRect).Then(u =>
            {
                Rect nameEntryRect = new Rect(u);
                nameEntryRect.height = textInputHeight;
                nameEntryRect.width = 400f;
                name = Widgets.TextEntryLabeled(nameEntryRect, "Name", name);

                return nameEntryRect;
            })
            .ThenGap(15f)
            .Then(u =>
            {
                Rect keyEntryRect = new Rect(u);
                keyEntryRect.height = textInputHeight;
                keyEntryRect.width = 400f;
                key = Widgets.TextEntryLabeled(keyEntryRect, "Key", key);

                return keyEntryRect;
            })
            .ThenGap(15f)
            .Then(u =>
            {
                return new RectStacker(u).Then(v =>
                {
                    Rect radioButtonRect = new Rect(v);
                    radioButtonRect.width = 100f;
                    radioButtonRect.height = 20f;
                    Widgets.RadioButtonLabeled(radioButtonRect, "Poll", false);

                    return radioButtonRect;
                })
                .ThenGap(10f)
                .Then(v =>
                {
                    Rect radioButtonRect = new Rect(v);
                    radioButtonRect.width = 100f;
                    radioButtonRect.height = 20f;
                    Widgets.RadioButtonLabeled(radioButtonRect, "Digest", false);

                    return radioButtonRect;
                })
                .ThenGap(10f)
                .Then(v =>
                {
                    Rect radioButtonRect = new Rect(v);
                    radioButtonRect.width = 100f;
                    radioButtonRect.height = 20f;
                    Widgets.RadioButtonLabeled(radioButtonRect, "Window", false);

                    return radioButtonRect;
                });
            })
            .ThenGap(15f)
            .Then(u =>
            {
                Rect textButtonRect = new Rect(u);
                textButtonRect.width = 100f;
                textButtonRect.height = 35f;
                if(Widgets.ButtonText(textButtonRect, "Domain"))
                {

                }

                return textButtonRect;
            })
            .Then(u =>
            {
                Rect textButtonRect = new Rect(u);
                textButtonRect.width = 100f;
                textButtonRect.height = 35f;
                if (Widgets.ButtonText(textButtonRect, "Source"))
                {

                }

                return textButtonRect;
            })
            .Then(u =>
            {
                Rect textButtonRect = new Rect(u);
                textButtonRect.width = 100f;
                textButtonRect.height = 35f;
                if (Widgets.ButtonText(textButtonRect, "Aggregator"))
                {

                }

                return textButtonRect;
            });
        }
    }
}
