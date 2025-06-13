using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Generalisk.QuickBuild.Editor
{
    public static class SaveData
    {
        /// <summary>
        /// The path where the save data is located, relative to the projects root folder
        /// </summary>
        private const string SavePath = "UserSettings/QuickBuild.json";

        /// <summary>
        /// Loads the save data.
        /// 
        /// For internal use.
        /// </summary>
        /// <returns>The save data as A JObject</returns>
        private static JObject Load()
        {
            if (!Directory.Exists(SavePath + "/../"))
            { Directory.CreateDirectory(SavePath + "/../"); }

            if (!File.Exists(SavePath))
            { File.WriteAllText(SavePath, "{\n}\n"); }

            return JObject.Parse(File.ReadAllText(SavePath));
        }

        /// <summary>
        /// Saves the save data.
        /// 
        /// For internal use.
        /// </summary>
        /// <param name="data">The save data as A JObject</param>
        private static void Save(JObject data)
        {
            if (!Directory.Exists(SavePath + "/../"))
            { Directory.CreateDirectory(SavePath + "/../"); }

            File.WriteAllText(SavePath, data.ToString(Formatting.Indented));
        }

        /// <summary>
        /// Gets the value of A platform
        /// </summary>
        /// <param name="platform">The platform to load the info for</param>
        /// <returns>The retrieved value, returns false if the platform is unassigned in the save data</returns>
        public static bool Get(PlatformInfo platform)
        { return Get(platform.code); }

        /// <summary>
        /// Gets the value of A platform
        /// </summary>
        /// <param name="name">The platform code to load the info for</param>
        /// <returns>The retrieved value, returns false if the platform is unassigned in the save data</returns>
        public static bool Get(string name)
        {
            JObject json = Load();
            JToken value = json[name];
            if (value == null)
            { return false; }
            return value.Value<bool>();
        }

        /// <summary>
        /// Sets & saves the value of A platform
        /// </summary>
        /// <param name="platform">The platform to assign the save value for</param>
        /// <param name="value">The value to save</param>
        public static void Set(PlatformInfo platform, bool value)
        { Set(platform.code, value); }

        /// <summary>
        /// Sets & saves the value of A platform
        /// </summary>
        /// <param name="name">The code of the platform to assign the save value for</param>
        /// <param name="value">The value to save</param>
        public static void Set(string name, bool value)
        {
            JObject json = Load();
            try { json[name].Replace(value); }
            catch { json.Add(name, value); }
            Save(json);
        }
    }
}
