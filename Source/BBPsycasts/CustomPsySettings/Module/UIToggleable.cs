using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BBPsycasts.CustomPsySettings.Module
{
    public class UIToggleable : UIModuleBase
    {

        private string _label = "";
        private bool _state = false;
        private string _tooltip = "";

        public string Label { get => _label; set => _label = value; }

        public bool State { get => _state; set => _state = value; }

        public string Tooltip { get => _tooltip; set => _tooltip = value; }

        public UIToggleable() { }

        public UIToggleable(string label)
        {
            _label = label;
        }

        public UIToggleable(string label, bool state) : this(label)
        {
            _state = state;
        }

        public UIToggleable(string label, bool state, string tooltip) : this(label, state)
        {
            _tooltip = tooltip;
        }

        public void Toggle() => _state = !_state;

        public override void DisplaySettings(Listing_Standard listing_Standard)
        {
            base.DisplaySettings(listing_Standard);
            listing_Standard.CheckboxLabeled(Label, ref _state, Tooltip);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref _label, "_label");
            Scribe_Values.Look(ref _state, "_toggleable");
            Scribe_Values.Look(ref _tooltip, "_tooltip");
        }
    }
}
