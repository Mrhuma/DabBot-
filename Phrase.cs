using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DabBot_
{
    class Phrase
    {
        private Regex _text;
        private int _mediaCount;

        public Regex text { get { return _text; } set { _text = value; } }
        public int mediaCount { get { return _mediaCount; } set { _mediaCount = value; } }

        public Phrase(Regex text, int mediaCount)
        {
            this.text = text;
            this.mediaCount = mediaCount;
        }
    }
}
