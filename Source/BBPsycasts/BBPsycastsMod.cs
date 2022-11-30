using BBPsycasts.CustomPsySettings;
using BBPsycasts.CustomPsySettings.Module;
using BBPsycasts.CustomPsySettings.Viewer;
using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VanillaPsycastsExpanded;
using Verse;
using VFECore;
using VFECore.Abilities;
using AbilityDef = VFECore.Abilities.AbilityDef;

namespace BBPsycasts
{

    public class BBPsycastsMod : Mod
    {
        private static Harmony _harmony;
        private static BBPsycastsSettings _settings;
        private static ISettings _renderer;

        public static BBPsycastsSettings ModSettings { get => _settings; set => _settings = value; }

        public BBPsycastsMod(ModContentPack content) : base(content)
        {
            _harmony = new Harmony(Content.Name);
            _harmony.PatchAll(Assembly.GetExecutingAssembly());
            _settings = GetSettings<BBPsycastsSettings>();
            _harmony.Patch(AccessTools.Method(typeof(DefGenerator), "GenerateImpliedDefs_PreResolve"), prefix: new HarmonyMethod(GetType(), "ApplySettings"));
            _harmony.Patch(AccessTools.Method(typeof(DefGenerator), "GenerateImpliedDefs_PostResolve"), postfix: new HarmonyMethod(GetType(), "PostLoadSettings"));

        }

        public static void ApplySettings()
        {
            if (_settings.RefreshSettings || _settings.Settings.NullOrEmpty())
            {
                List<AbilityDef> items = DefDatabase<AbilityDef>.AllDefs.Where(i =>
                {
                    return i.HasModExtension<AbilityExtension_Psycast>();
                }).ToList();

                int size = items.Count();
                for (int index = 0; index < size; index++)
                {
                    AbilityDef ability = items[index];
                    Dictionary<string, float> fields = GetFieldValuesFromToDict(ability);

                    DefModExtension modExtension = ability.GetModExtension<AbilityExtension_Psycast>();
                    Dictionary<string, float> modExtensionFields = GetFieldValuesFromToDict(modExtension);

                    UIViewerWithChildren uiViewerWithChildren = new UIViewerWithChildren(index, ability.defName);
                    uiViewerWithChildren.AddChild("F", new UIDropdownNumeric("Fields", fields));
                    uiViewerWithChildren.AddChild("EXT", new UIDropdownNumeric("Mod Extension Fields", modExtensionFields));
                    _settings.Settings.SetOrAdd(ability.defName, uiViewerWithChildren);
                }
            }
        }

        public static void PostLoadSettings()
        {
            foreach (KeyValuePair<string, UIViewerWithChildren> kvp in _settings.Settings)
            {
                AbilityDef abilityDef = DefDatabase<AbilityDef>.GetNamedSilentFail(kvp.Key);
                if (abilityDef != null)
                {
                    ISettings fields = kvp.Value.GetChild("F");
                    fields.ApplySettings(abilityDef);

                    ISettings extFields = kvp.Value.GetChild("EXT");
                    extFields.ApplySettings(abilityDef);
                }
            }

            UIViewerCarousel home = new UIViewerCarousel
            {
                Id = 0,
                Name = "Home"
            };
            foreach (KeyValuePair <string, UIViewerWithChildren> child in _settings.Settings)
            {
                home.AddChild(child.Key, child.Value);
            }
            _renderer = home;
        }

        private static Dictionary<string, float> GetFieldValuesFromToDict(object item)
        {
            Dictionary<string, float> fields = new Dictionary<string, float>();
            AccessTools.GetFieldNames(item).ForEach(delegate (string fieldName)
            {
                FieldAction(fields, item, fieldName);
            });
            return fields;
        }

        private static void FieldAction(Dictionary<string, float> fields, object item, string fieldName)
        {
            string value = Traverse.Create(item).Field(fieldName).GetValue()?.ToString();
 
            bool isDouble = double.TryParse(value, out double doubleVal);
            bool isFloat = float.TryParse(value, out float floatVal);
            bool isint = int.TryParse(value, out int intVal);

            if (isDouble)
            {
                fields.SetOrAdd(fieldName, (float)doubleVal);
            }
            else if (isFloat)
            {
                fields.SetOrAdd(fieldName, floatVal);
            }
            else if (isint)
            {
                fields.SetOrAdd(fieldName, intVal);
            }
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.Gap();
            _renderer.DisplaySettings(listingStandard);
            listingStandard.Gap();
            listingStandard.End();
            _settings.Write();
        }

        public override string SettingsCategory()
        {
            return Content.Name;
        }

        public override void WriteSettings()
        {
            base.WriteSettings();
        }
    }
}
