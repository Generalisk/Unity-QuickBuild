using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

// Marked static for my own convenience
using static Generalisk.QuickBuild.Editor.Shared;
using static UnityEditor.EditorGUILayout;

namespace Generalisk.QuickBuild.Editor
{
    public class QuickBuildWindow : EditorWindow
    {
        private bool isDebugBuild = false;
        private BuildType buildType = BuildType.Player;
        private List<bool> enabled = new List<bool>();

        private List<PlatformCategory> categories = new List<PlatformCategory>();

        private Vector2 scrollPos = Vector2.zero;

        /// <summary>
        /// Opens the Quick Build Window
        /// </summary>
        [MenuItem("File/Quick Build %&B", priority = 210)]
        public static void Open()
        {
            QuickBuildWindow window = GetWindow<QuickBuildWindow>();
            window.titleContent = new GUIContent("Quick Build");
            window.minSize = new Vector2(480, 360);
        }

        /// <summary>
        /// Called when the Window is first opened
        /// </summary>
        void Awake()
        {
            buildType = SaveData.GetBuildType();
            isDebugBuild = SaveData.GetConfiguration();
        }

        /// <summary>
        /// Called every time the Window needs to refresh
        /// </summary>
        void OnGUI()
        {
            // Reload save data if platform list updated
            if (Platforms.Length != enabled.Count)
            {
                enabled.Clear();
                categories.Clear();

                foreach (PlatformInfo platform in Platforms)
                { enabled.Add(SaveData.GetPlatform(platform)); }
            }

            // Build Type (Player, Asset Bundles, Addressables etc.)
#if ASSETBUNDLE_SUPPORT || ADDRESSABLE_SUPPORT
            string buildTypeTitle = "None";
            switch (buildType)
            {
                case BuildType.Player: buildTypeTitle = "Player"; break;
#if ASSETBUNDLE_SUPPORT
                case BuildType.AssetBundles: buildTypeTitle = "Asset Bundles"; break;
#endif // ASSETBUNDLE_SUPPORT
#if ADDRESSABLE_SUPPORT
                case BuildType.Addressables: buildTypeTitle = "Addressables"; break;
#endif // ADDRESSABLE_SUPPORT
            }

            BeginHorizontal();
            LabelField("Build Type", GUILayout.Width(GetLabelWidth()));
            if (DropdownButton(new GUIContent(buildTypeTitle), FocusType.Keyboard))
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("Player"), buildType == BuildType.Player, SetPlayer);
#if ASSETBUNDLE_SUPPORT
                menu.AddItem(new GUIContent("Asset Bundles"), buildType == BuildType.AssetBundles, SetAssetBundles);
#endif // ASSETBUNDLE_SUPPORT
#if ADDRESSABLE_SUPPORT
                menu.AddItem(new GUIContent("Addressables"), buildType == BuildType.Addressables, SetAddressables);
#endif // ADDRESSABLE_SUPPORT
                menu.ShowAsContext();
            }
            EndHorizontal();
#else // ASSETBUNDLE_SUPPORT || ADDRESSABLE_SUPPORT
            // Safety precaution incase uninstalling package caused the value to turn null
            SetPlayer();
#endif // ASSETBUNDLE_SUPPORT || ADDRESSABLE_SUPPORT

            // Build Configuration (Debug/Release)
            if (buildType == BuildType.Player)
            {
                string configTitle = "Release";
                if (isDebugBuild) { configTitle = "Debug"; }

                BeginHorizontal();
                LabelField("Configuration", GUILayout.Width(GetLabelWidth()));
                if (DropdownButton(new GUIContent(configTitle), FocusType.Keyboard))
                {
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Debug"), isDebugBuild, SetDebug);
                    menu.AddItem(new GUIContent("Release"), !isDebugBuild, SetRelease);
                    menu.ShowAsContext();
                }
                EndHorizontal();
            }

            // Target Platform Selection
#if ADDRESSABLE_SUPPORT
            if (buildType != BuildType.Addressables)
            {
#endif // ADDRESSABLE_SUPPORT
                LabelField("Platforms", EditorStyles.boldLabel);
                scrollPos = BeginScrollView(scrollPos);
                List<PlatformInfo> platformz = Platforms.ToList();
                while (platformz.Count > 0)
                { DrawPlatform(ref platformz, platformz[0].name.Split("/")[0]); }
                EndScrollView();
#if ADDRESSABLE_SUPPORT
            }
#endif // ADDRESSABLE_SUPPORT

            // Footer
            BeginHorizontal();
#if ADDRESSABLE_SUPPORT
            if (buildType != BuildType.Addressables)
            {
#endif // ADDRESSABLE_SUPPORT
                if (GUILayout.Button(new GUIContent("Select All"), GUILayout.Width(80)))
                { for (int i = 0; i < enabled.Count; i++) { enabled[i] = true; } }
                if (GUILayout.Button(new GUIContent("Deselect All"), GUILayout.Width(80)))
                { for (int i = 0; i < enabled.Count; i++) { enabled[i] = false; } }
#if ADDRESSABLE_SUPPORT
            }

            EditorGUI.BeginDisabledGroup(enabled.Where(x => x).ToArray().Length <= 0 && buildType != BuildType.Addressables);
#else // ADDRESSABLE_SUPPORT
            EditorGUI.BeginDisabledGroup(enabled.Where(x => x).ToArray().Length <= 0);
#endif // ADDRESSABLE_SUPPORT
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(new GUIContent("Build!"), EditorStyles.miniButtonRight, GUILayout.Width(100)))
            { QuickBuild.Build(buildType, isDebugBuild, GetPlatforms()); }
            EditorGUI.EndDisabledGroup();
            EndHorizontal();

            // Save data
            // TODO: Optimize
            SaveData.SetBuildType(buildType);
            SaveData.SetConfiguration(isDebugBuild);

            for (int i = 0; i < Platforms.Length; i++)
            { SaveData.SetPlatform(Platforms[i], enabled[i]); }
        }

        /// <summary>
        /// Draw A Platform Field/Category
        /// </summary>
        /// <param name="platforms">A reference to the list of currently unloaded platforms</param>
        /// <param name="filter">The Field/Category to Generate</param>
        private void DrawPlatform(ref List<PlatformInfo> platforms, string filter)
        {
            string name = filter.Split("/")[filter.Split("/").Length - 1];
            PlatformInfo[] platformz = platforms.Where(x => x.name == filter).ToArray();

            if (platformz.Length > 0)
            {
                PlatformInfo platform = platformz[0];
                int index = Platforms.ToList().IndexOf(platform);

                string[] nameSplit = filter.Split("/");
                for (int i = 0; i < nameSplit.Length - 1; i++)
                {
                    string catName = filter.Split("/")[0];
                    for (int x = 1; x <= i; x++)
                    { catName += "/" + filter.Split("/")[x]; }

                    int catIndex = GetCategoryIndex(catName);
                    if (!categories[catIndex].indexes.Contains((byte)index))
                    { categories[catIndex].indexes.Add((byte)index); }
                }

                BeginHorizontal();
                LabelField(new GUIContent(name, QuickBuild.GetIcon(platform.icon)), GUILayout.Width(GetLabelWidth()));
                enabled[index] = Toggle(enabled[index]);
                EndHorizontal();

                platforms.Remove(platform);
            }
            else
            {
                int index = GetCategoryIndex(filter);

                BeginHorizontal();
                LabelField(new GUIContent(name, QuickBuild.GetIcon(name)), GUILayout.Width(GetLabelWidth()));
                EditorGUI.BeginDisabledGroup(true);
                Toggle(GetCategory(index));
                EditorGUI.EndDisabledGroup();
                EndHorizontal();

                EditorGUI.indentLevel++;
                PlatformInfo[] search = platforms.Where(x => x.name.StartsWith(filter)).ToArray();
                List<string> names = new List<string>();
                foreach (PlatformInfo plat in search)
                {
                    string platName = plat.name.Split("/")[0];
                    for (int i = 1; i <= filter.Split("/").Length; i++)
                    { platName += "/" + plat.name.Split("/")[i]; }
                    names.Add(platName);
                }
                names = names.Distinct().ToList();
                foreach (string nam in names) { DrawPlatform(ref platforms, nam); }
                EditorGUI.indentLevel--;
            }
        }

        /// <summary>
        /// Gets an Array of all the currently enabled Platforms
        /// </summary>
        /// <returns></returns>
        private PlatformInfo[] GetPlatforms()
        {
            List<PlatformInfo> platforms = new List<PlatformInfo>();
            for (int i = 0; i < enabled.Count; i++)
            { if (enabled[i]) { platforms.Add(Platforms[i]); } }
            return platforms.ToArray();
        }

        /// <summary>
        /// Calculates the width of the Property Name/Label based on the current indent level
        /// </summary>
        /// <returns>The width of the Property Name/Label</returns>
        private float GetLabelWidth() { return EditorGUIUtility.labelWidth - (EditorGUI.indentLevel * 15); }

        /// <summary>
        /// Gets the index of the requested category
        /// 
        /// Creates A new category if the category requested does not currently exist
        /// </summary>
        /// <param name="name">The name of the category to find</param>
        /// <returns>The index of the requested category</returns>
        private int GetCategoryIndex(string name)
        {
            for (int i = 0; i < categories.Count; i++)
            { if (categories[i].name == name){ return i; } }

            categories.Add(new PlatformCategory()
            {
                name = name,
                indexes = new List<byte>(),
            });
            return categories.Count - 1;
        }

        /// <summary>
        /// Gets the state of the category checkbox
        /// </summary>
        /// <param name="index">the category index</param>
        /// <returns>true if all items attached to the category are checked, false otherwise</returns>
        private bool GetCategory(int index)
        {
            foreach (byte i in categories[index].indexes)
            { if (!enabled[i]) { return false; } }
            return true;
        }

        /// <summary>
        /// Set the current Build Configuration to Debug
        /// </summary>
        private void SetDebug() { isDebugBuild = true; }

        /// <summary>
        /// Set the current Build Configuration to Release
        /// </summary>
        private void SetRelease() { isDebugBuild = false; }

        /// <summary>
        /// Set the current Build type to Player
        /// </summary>
        private void SetPlayer() { buildType = BuildType.Player; }

#if ASSETBUNDLE_SUPPORT
        /// <summary>
        /// Set the current Build type to Asset Bundles
        /// </summary>
        private void SetAssetBundles() { buildType = BuildType.AssetBundles; }
#endif // ASSETBUNDLE_SUPPORT

#if ADDRESSABLE_SUPPORT
        /// <summary>
        /// Set the current Build Type to Addressables
        /// </summary>
        private void SetAddressables() { buildType = BuildType.Addressables; }
#endif // ADDRESSABLE_SUPPORT
    }
}
