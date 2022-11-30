using Verse;
using VFECore.Abilities;

namespace BBPsycasts.CustomPsySettings
{
    public interface ISettings
    {
        bool SearchSettings(string searchString);

        void DisplaySettings(Listing_Standard listing_Standard);

        void ApplySettings(AbilityDef abilityDef);
    }
}