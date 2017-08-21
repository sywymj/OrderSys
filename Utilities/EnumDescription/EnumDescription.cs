using System;

namespace JSNet.Utilities
{  
    public class EnumDescription : Attribute
    {
        private string _text;

        public string Text
        {
            get
            {
                return _text;
            }
        }

        public EnumDescription(string text)
        {
            _text = text;
        }
    }

    public class EnumColorStyle:Attribute
    {
        private string _text;

        public string Text
        {
            get
            {
                return _text;
            }
        }

        public EnumColorStyle(string text)
        {
            _text = text;
        }
    }
}