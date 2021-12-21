using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DabBot_
{
    class Phrase
    {
        private string _text;
        private int _mediaCount;

        public string text { get { return _text; } set { _text = value; } }
        public int mediaCount { get { return _mediaCount; } set { _mediaCount = value; } }

        public Phrase(string text, int mediaCount)
        {
            this.text = text;
            this.mediaCount = mediaCount;
        }
    }
}
