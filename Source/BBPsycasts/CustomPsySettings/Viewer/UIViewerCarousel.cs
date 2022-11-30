using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BBPsycasts.CustomPsySettings.Viewer
{
    public class UIViewerCarousel : UIViewerWithChildren
    {
        private string _text;
        private string _cacheText;
        private string _label = "Search";
        private string _labelNotFound = "Item not found. Try search again.";
        private string _labelFound = "Find it?";
        private string _buttonLabelNext = "Next";
        private int _index = 0;
        private int _sliderValue = 0;
        private int _cacheSearchLocation = -1;
        private bool _clicked = false;
        private bool _found = false;

        public UIViewerCarousel() { }


        public override void DisplaySettings(Listing_Standard listing_Standard)
        {
            _cacheText = _text;
            int inc = _sliderValue + 1;
            if (_clicked)
            {
                _clicked = false;
                inc = inc >= ChildCount ? 0 : inc;
                _sliderValue = inc;
                _cacheSearchLocation = inc - 1;

            }
            _cacheSearchLocation = inc < ChildCount ? inc : ChildCount;
            _found = false;
            _text = listing_Standard.TextEntryLabeled(_label, _cacheText);
            listing_Standard.Gap();
            if (!_text.NullOrEmpty() && !_cacheText.Equals(_text))
            {
                for (int i = 0; _cacheSearchLocation < ChildCount; i++)
                {
                    if (WindowChildren.ElementAt(i).Value.SearchSettings(_text))
                    {
                        _found = true;
                        _sliderValue = i;
                        _cacheSearchLocation = i;
                        break;
                    }
                }
                if (!_found)
                {
                    _sliderValue = 0;
                    _cacheSearchLocation = -1;
                    _label = _labelNotFound;
                }
                else
                {
                    _label = _labelFound;
                }
            }
            _index = (int)listing_Standard.Slider(_sliderValue, 0, ChildCount - 1);
            _sliderValue = _index;
            _clicked = listing_Standard.ButtonText(_buttonLabelNext);
            WindowChildren.ElementAt(_sliderValue).Value.DisplaySettings(listing_Standard);
        }

        public override void ExposeData()
        {
            base.ExposeData();
        }
    }
}
