using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace TestTask.Models
{
    public class MessageContainer
    {
        public string LanguageName { get; set; }
        public string LocaleName { get; set; }
        public Dictionary<LangKeys, string> Phrases { get; set; } = new Dictionary<LangKeys, string>();

        public string this[LangKeys langKey] => Phrases.ContainsKey(langKey) ? Phrases[langKey] : null;

        [JsonIgnore]
        public FileInfo FileInfo { get; set; }
    }
}
