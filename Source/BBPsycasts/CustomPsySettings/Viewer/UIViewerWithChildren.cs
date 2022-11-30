using MonoMod.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BBPsycasts.CustomPsySettings.Viewer
{
    public class UIViewerWithChildren : UIViewerBase
    {
        private int _index = 0;
        private Dictionary<string, ISettings> _windowChildren;

        public Dictionary<string, ISettings> WindowChildren { get => _windowChildren; }

        public int ChildCount => WindowChildren?.Count ?? 0;

        public int Index { get => _index; set => _index = value; }

        public UIViewerWithChildren() { }

        public UIViewerWithChildren(int id, string name) : base(id, name) { }

        public UIViewerWithChildren(int id, string name, Dictionary<string, ISettings> newWindow) : base(id, name)
        {
            _windowChildren = newWindow;
        }

        public void AddChild(string key, ISettings settingsToAdd)
        {
            if (_windowChildren == null)
                _windowChildren = new Dictionary<string, ISettings>();
            _windowChildren.SetOrAdd(key, settingsToAdd);
        }

        public ISettings GetChild(string key)
        {
            _windowChildren.TryGetValue(key, out ISettings settings);
            return settings;
        }

        public override void DisplaySettings(Listing_Standard listing_Standard) 
        {
            base.DisplaySettings(listing_Standard);
            listing_Standard.Gap();
            if (!_windowChildren.NullOrEmpty())
            {
                foreach (ISettings child in _windowChildren.Values)
                {
                    child.DisplaySettings(listing_Standard);
                    listing_Standard.GapLine();
                }
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref _index, "_index");
            Scribe_Collections.Look(ref _windowChildren, "_windowChildren", LookMode.Undefined, LookMode.Deep);
        }
    }
}
