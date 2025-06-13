using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;

namespace Generalisk.QuickBuild.Editor
{
    public static class SaveData
    {
        private const string SavePath = "UserSettings/QuickBuild.json";

        private static JObject Load()
        {
            if (!Directory.Exists(SavePath + "/../"))
            { Directory.CreateDirectory(SavePath + "/../"); }

            if (!File.Exists(SavePath))
            { File.WriteAllText(SavePath, "{\n}\n"); }

            return JObject.Parse(File.ReadAllText(SavePath));
        }

        private static void Save(JObject data)
        {
            if (!Directory.Exists(SavePath + "/../"))
            { Directory.CreateDirectory(SavePath + "/../"); }

            File.WriteAllText(SavePath, data.ToString(Formatting.Indented));
        }

        public static bool Get(PlatformInfo platform)
        { return Get(platform.code); }

        public static bool Get(string name)
        {
            JObject json = Load();
            JToken value = json[name];
            if (value == null)
            { return false; }
            return value.Value<bool>();
        }

        public static void Set(PlatformInfo platform, bool value)
        { Set(platform.code, value); }

        public static void Set(string name, bool value)
        {
            JObject json = Load();
            try { json[name].Replace(value); }
            catch { json.Add(name, value); }
            Save(json);
        }
    }
}
