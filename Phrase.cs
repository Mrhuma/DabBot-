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
        private Regex _regex;
        private int _mediaCount;
        private string _folderName;

        public Regex regex { get { return _regex; } set { _regex = value; } }
        public int mediaCount { get { return _mediaCount; } set { _mediaCount = value; } }
        public string folderName { get { return _folderName; } set { _folderName = value; } }

        public Phrase(Regex regex, int mediaCount, string folderName)
        {
            this.regex = regex;
            this.mediaCount = mediaCount;
            this.folderName = folderName;
        }
    }
}
