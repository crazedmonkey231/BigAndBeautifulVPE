using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using VFECore.Abilities;

namespace BBPsycasts.CustomPsySettings.Viewer
{
    public abstract class UIViewerBase : IExposable, ISettings
    {
        private int _id;
        private string _name;

        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }

        public UIViewerBase() { }

        public UIViewerBase(int id, string name)
        {
            _id = id;
            _name = name;
        }

        public virtual void DisplaySettings(Listing_Standard listing_Standard) 
        {
            listing_Standard.Label(_name.CapitalizeFirst());
        }

        public virtual void ExposeData()
        {
            Scribe_Values.Look(ref _id, "_id");
            Scribe_Values.Look(ref _name, "_name");
        }

        public virtual bool SearchSettings(string searchString)
        {
            return _name.ToLower().Contains(searchString.ToLower());
        }

        public virtual void ApplySettings(AbilityDef abilityDef)
        {

        }
    }
}
