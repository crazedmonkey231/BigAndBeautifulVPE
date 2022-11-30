using BBPsycasts.CustomPsySettings;
using BBPsycasts.CustomPsySettings.Viewer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BBPsycasts
{
    public class BBPsycastsSettings : ModSettings
    {

        private bool _refreshSettings = false;
        private Dictionary<string, UIViewerWithChildren> _settings = new Dictionary<string, UIViewerWithChildren>();

        public bool RefreshSettings { get => _refreshSettings; set => _refreshSettings = value; }

        public Dictionary<string, UIViewerWithChildren> Settings { get => _settings; set => _settings = value; }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref _refreshSettings, "refreshSettings");
            Scribe_Collections.Look(ref _settings, "settings", LookMode.Undefined, LookMode.Deep);
        }
    }
}
