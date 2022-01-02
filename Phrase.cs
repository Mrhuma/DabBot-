using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DabBot_
{
    public class Phrase
    {
        private Regex _regex;
        private List<string> _links;

        public Regex regex { get { return _regex; } set { _regex = value; } }
        public List<string> links { get { return _links; } set { _links = value; } }

        public Phrase(Regex regex, List<string> links)
        {
            this.regex = regex;
            this.links = links;
        }
    }
}
