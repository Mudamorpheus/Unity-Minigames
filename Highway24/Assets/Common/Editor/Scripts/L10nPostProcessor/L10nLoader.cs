using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace L10nPostProcessor {
    public class TranslateVersions {
        [JsonProperty(PropertyName = "Version1")]
        public Dictionary<string, string> Version1;
        [JsonProperty(PropertyName = "Version2")]
        public Dictionary<string, string> Version2;
        [JsonProperty(PropertyName = "Version3")]
        public Dictionary<string, string> Version3;
        [JsonProperty(PropertyName = "Version4")]
        public Dictionary<string, string> Version4;
        [JsonProperty(PropertyName = "Version5")]
        public Dictionary<string, string> Version5;
    }

    public class L10nConfig {
        [JsonProperty(PropertyName = "AppTrackingTranslates")]
        public TranslateVersions AppTrackingTranslates;
        [JsonProperty(PropertyName = "GeoLocationTranslates")]
        public TranslateVersions GeoLocationTranslates;
    }
    
    public static class L10nLoader {
        private static TranslateVersions appTrackingTranslates;
        private static TranslateVersions geoLocationTranslates;
        public static Dictionary<string, Dictionary<string, string>> appTrackingDict;
        public static Dictionary<string, Dictionary<string, string>> geoLocationDict;
        private static T DeserializeFromFile<T>(string filePath) {
            var textAsset = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(textAsset);
        }
        
        public static void LoadL10nFile() {
            var configPath = Path.GetDirectoryName(Application.dataPath) + Path.DirectorySeparatorChar + $"External_configs{Path.DirectorySeparatorChar}L10n.json";
            appTrackingTranslates = null;
            geoLocationTranslates = null;
            if (!File.Exists($"{configPath}")) 
                return; 
            
            var l10NConfig  = DeserializeFromFile<L10nConfig>(configPath);
            if (l10NConfig == null) 
                return;
            appTrackingDict = new Dictionary<string, Dictionary<string, string>> {
                {"Version1", l10NConfig.AppTrackingTranslates.Version1},
                {"Version2", l10NConfig.AppTrackingTranslates.Version2},
                {"Version3", l10NConfig.AppTrackingTranslates.Version3},
                {"Version4", l10NConfig.AppTrackingTranslates.Version4},
                {"Version5", l10NConfig.AppTrackingTranslates.Version5}
            };

            geoLocationDict = new Dictionary<string, Dictionary<string, string>> {
                {"Version1", l10NConfig.GeoLocationTranslates.Version1},
                {"Version2", l10NConfig.GeoLocationTranslates.Version2},
                {"Version3", l10NConfig.GeoLocationTranslates.Version3},
                {"Version4", l10NConfig.GeoLocationTranslates.Version4},
                {"Version5", l10NConfig.GeoLocationTranslates.Version5}
            };
        }
    }
}
