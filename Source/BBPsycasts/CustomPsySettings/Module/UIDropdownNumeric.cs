using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using VFECore.Abilities;

namespace BBPsycasts.CustomPsySettings.Module
{
    public class UIDropdownNumeric : UIToggleable
    {
        private int _size;
        private Dictionary<string, float> _dictionary;

        public Dictionary<string, float> DropdownDictionary { get => _dictionary; set => _dictionary = value; }

        public UIDropdownNumeric() {}

        public UIDropdownNumeric(Dictionary<string, float> changeableNumeric)
        {
            DropdownDictionary = changeableNumeric;
            _size = DropdownDictionary.Count - 1;
        }

        public UIDropdownNumeric(string name, Dictionary<string, float> changeableNumeric) : this(changeableNumeric)
        {
            Label = name;
        }

        public UIDropdownNumeric(int id, string name, Dictionary<string, float> changeableNumeric) : this(name, changeableNumeric)
        {
            Id = id;
        }

        public override void DisplaySettings(Listing_Standard listing_Standard)
        {
            base.DisplaySettings(listing_Standard);
            if (State)
            {
                for (int i = 0; i < _size; i++)
                {
                    string key = DropdownDictionary.ElementAt(i).Key;
                    float value = DropdownDictionary.ElementAt(i).Value;
                    string buffer = value.ToStringSafe();

                    listing_Standard.TextFieldNumericLabeled("{0} : ".Formatted(key), ref value, ref buffer);

                    bool didParse = float.TryParse(buffer, out float newValue);
                    if (didParse)
                        DropdownDictionary[key] = newValue;
                }
            }
        }

        public override void ApplySettings(AbilityDef abilityDef)
        {
            base.ApplySettings(abilityDef);

            foreach (string key in _dictionary.Keys)
            {
                object result = Traverse.Create(abilityDef).Field(key)?.GetValue();

                if (result != null)
                {
                    TypeCode typeCode = Type.GetTypeCode(result.GetType());
                    float dictVal = _dictionary.TryGetValue(key);
                    Traverse trav = Traverse.Create(abilityDef).Field(key);
                    if (typeCode.Equals(TypeCode.Double))
                        trav.SetValue((double)dictVal);
                    else if (typeCode.Equals(TypeCode.Single))
                        trav.SetValue(dictVal);
                    else if (typeCode.Equals(TypeCode.Int64) || typeCode.Equals(TypeCode.Int32))
                        trav.SetValue((int)dictVal);
                }
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref _size, "_size");
            Scribe_Collections.Look(ref _dictionary, "_dictionary");
        }
    }
}
