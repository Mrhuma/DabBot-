using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DabBot_
{
    public static class JsonHelper
    {
        //Write phrases to json file
        public static void SerializePhrases(List<Phrase> phrases, string pathPrefix)
        {
            File.WriteAllText(pathPrefix + "phrases.json", JsonConvert.SerializeObject(phrases, Formatting.Indented, new Newtonsoft.Json.Converters.StringEnumConverter()));
        }

        //Read phrases from json file
        public static List<Phrase> DeserializePhrases(string pathPrefix)
        {
            return JsonConvert.DeserializeObject<List<Phrase>>(File.ReadAllText(pathPrefix + "phrases.json"), new Newtonsoft.Json.Converters.StringEnumConverter());
        }
    }
}
