using System.IO;
using UnityEditor;
#if ADDRESSABLE_SUPPORT
using UnityEditor.AddressableAssets.Settings;
#endif // ADDRESSABLE_SUPPORT
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Generalisk.QuickBuild.Editor
{
    public static class QuickBuild
    {
        /// <summary>
        /// Displays A Dialog Prompt asking if you really want to compile, warning you that it could take hours to potentially days.
        /// 
        /// For internal use, does not do anything else by itself.
        /// </summary>
        /// <returns><code>true</code> if the user clicks yes, <code>false</code> otherwise.</returns>
        internal static bool DisplayPrompt()
        {
            // As A child, I yearned for the day that Unity would properly optimize compilation times so I won't have to wait hours for the shaders to fucking compile every time I switch platform
            return EditorUtility.DisplayDialog("Are you sure you want to Compile?", "Depending on the amount of platforms selected and the size of the project, this process can take hours to potentially days...", "Yes!", "No!");
        }

        /// <summary>
        /// Quickly builds the given platforms
        /// </summary>
        /// <param name="buildType">The type of Build you want to perform</param>
        /// <param name="platforms">The Platforms you want to Build</param>
        public static void Build(BuildType buildType, params PlatformInfo[] platforms)
        { Build(buildType, false, platforms); }

        /// <summary>
        /// Quickly builds the given platforms
        /// </summary>
        /// <param name="buildType">The type of Build you want to perform</param>
        /// <param name="debug"><code>true</code> if you want to compile A Development Build, <code>false</code> otherwise.</param>
        /// <param name="platforms">The Platforms you want to Build</param>
        public static void Build(BuildType buildType, bool debug, params PlatformInfo[] platforms)
        { if (!DisplayPrompt()) { return; } BuildNoPrompt(buildType, debug, platforms); }

        /// <summary>
        /// Quickly builds the given platforms without displaying A prompt
        /// </summary>
        /// <param name="buildType">The type of Build you want to perform</param>
        /// <param name="platforms">The Platforms you want to Build</param>
        public static void BuildNoPrompt(BuildType buildType, params PlatformInfo[] platforms)
        { BuildNoPrompt(buildType, false, platforms); }

        /// <summary>
        /// Quickly builds the given platforms without displaying A prompt
        /// </summary>
        /// <param name="buildType">The type of Build you want to perform</param>
        /// <param name="debug"><code>true</code> if you want to compile A Development Build, <code>false</code> otherwise.</param>
        /// <param name="platforms">The Platforms you want to Build</param>
        public static void BuildNoPrompt(BuildType buildType, bool debug, params PlatformInfo[] platforms)
        {
            switch (buildType)
            {
                case BuildType.Player: BuildPlayerNoPrompt(debug, platforms); break;
#if ASSETBUNDLE_SUPPORT
                case BuildType.AssetBundles: BuildAssetBundlesNoPrompt(platforms); break;
#endif // ASSETBUNDLE_SUPPORT
#if ADDRESSABLE_SUPPORT
                case BuildType.Addressables: BuildAddressablesNoPrompt(); break;
#endif // ADDRESSABLE_SUPPORT
            }
        }

        /// <summary>
        /// Quickly builds the given platforms
        /// </summary>
        /// <param name="platforms">The Platforms you want to Build</param>
        public static void BuildPlayer(params PlatformInfo[] platforms)
        { BuildPlayer(false, platforms); }

        /// <summary>
        /// Quickly builds the given platforms
        /// </summary>
        /// <param name="debug"></param>
        /// <param name="platforms">The Platforms you want to Build</param>
        public static void BuildPlayer(bool debug, params PlatformInfo[] platforms)
        { if (!DisplayPrompt()) { return; } BuildPlayerNoPrompt(debug, platforms); }

        /// <summary>
        /// Quickly builds the given platforms without displaying A prompt
        /// </summary>
        /// <param name="platforms">The Platforms you want to Build</param>
        public static void BuildPlayerNoPrompt(params PlatformInfo[] platforms)
        { BuildPlayerNoPrompt(false, platforms); }

        /// <summary>
        /// Quickly builds the given platforms without displaying A prompt
        /// </summary>
        /// <param name="debug"><code>true</code> if you want to compile A Development Build, <code>false</code> otherwise.</param>
        /// <param name="platforms">The Platforms you want to Build</param>
        public static void BuildPlayerNoPrompt(bool debug, params PlatformInfo[] platforms)
        {
            foreach (PlatformInfo platform in platforms)
            {
#if DEDICATEDSERVER_SUPPORT
                int subtarget = 0;
                if (platform.buildTarget == BuildTarget.StandaloneWindows ||
                    platform.buildTarget == BuildTarget.StandaloneWindows64 ||
                    platform.buildTarget == BuildTarget.StandaloneOSX ||
                    platform.buildTarget == BuildTarget.StandaloneLinux64)
                {
                    if (platform.server)
                    { subtarget = (int)StandaloneBuildSubtarget.Server; }
                    else { subtarget = (int)StandaloneBuildSubtarget.Player; }
                }
#endif // DEDICATEDSERVER_SUPPORT

                string path = "Build/Player/" + platform.code;
                if (platform.appendName) { path += "/" + Application.productName; }
                if (!string.IsNullOrWhiteSpace(platform.ext)) { path += "." + platform.ext; }

                if (!Directory.Exists("Build/Player/" + platform.code))
                { Directory.CreateDirectory("Build/Player/" + platform.code); }

                BuildOptions option = BuildOptions.None;
                if (debug) { option = BuildOptions.Development; }

                BuildReport report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
                {
                    locationPathName = path,
                    options = option,
                    target = platform.buildTarget,
#if DEDICATEDSERVER_SUPPORT
                    subtarget = subtarget,
#endif // DEDICATEDSERVER_SUPPORT
                });

                if (report.summary.totalErrors > 0)
                {
                    if (!EditorUtility.DisplayDialog("An Error has Occurred while compiling", "Do you wish to continue compiling?", "Yes!", "No!"))
                    { return; }
                }

                EditorUtility.RevealInFinder("Build/Player/Untitled");
            }
        }

#if ASSETBUNDLE_SUPPORT
        /// <summary>
        /// Quickly Builds the Asset Bundles of the given Platform
        /// </summary>
        /// <param name="platforms">The Platforms you want to Build</param>
        public static void BuildAssetBundles(params PlatformInfo[] platforms)
        { if (!DisplayPrompt()) { return; } BuildAssetBundlesNoPrompt(platforms); }

        /// <summary>
        /// Quickly Builds the Asset Bundles of the given Platform without displaying A prompt
        /// </summary>
        /// <param name="platforms">The Platforms you want to Build</param>
        public static void BuildAssetBundlesNoPrompt(params PlatformInfo[] platforms)
        {
            foreach (PlatformInfo platform in platforms)
            {
#if DEDICATEDSERVER_SUPPORT
                int subtarget = 0;
                if (platform.buildTarget == BuildTarget.StandaloneWindows ||
                    platform.buildTarget == BuildTarget.StandaloneWindows64 ||
                    platform.buildTarget == BuildTarget.StandaloneOSX ||
                    platform.buildTarget == BuildTarget.StandaloneLinux64)
                {
                    if (platform.server)
                    { subtarget = (int)StandaloneBuildSubtarget.Server; }
                    else { subtarget = (int)StandaloneBuildSubtarget.Player; }
                }
#endif // DEDICATEDSERVER_SUPPORT

                if (!Directory.Exists("Build/Bundles/" + platform.code))
                { Directory.CreateDirectory("Build/Bundles/" + platform.code); }

                BuildPipeline.BuildAssetBundles(new BuildAssetBundlesParameters()
                {
                    outputPath = "Build/Bundles/" + platform.code + "/" + Application.productName,
                    options = BuildAssetBundleOptions.None,
                    targetPlatform = platform.buildTarget,
#if DEDICATEDSERVER_SUPPORT
                    subtarget = subtarget,
#endif // DEDICATEDSERVER_SUPPORT
                });

                EditorUtility.RevealInFinder("Build/Bundles/Untitled");
            }
        }
#endif // ASSETBUNDLE_SUPPORT

#if ADDRESSABLE_SUPPORT
        /// <summary>
        /// Quickly Builds the Projects Addressables
        /// </summary>
        public static void BuildAddressables()
        { if (!DisplayPrompt()) { return; } BuildAddressablesNoPrompt(); }

        /// <summary>
        /// Quickly Builds the Projects Addressables without displaying A prompt
        /// </summary>
        public static void BuildAddressablesNoPrompt()
        { AddressableAssetSettings.BuildPlayerContent(); }
#endif // ADDRESSABLE_SUPPORT

        /// <summary>
        /// For internal use, used for displaying the icons in the Quick Build Window.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The Icon Texture</returns>
        internal static Texture GetIcon(string name)
        {
            switch (name.ToLower())
            {
                case "client": return EditorGUIUtility.IconContent("BuildSettings.Standalone On").image;
#if DEDICATEDSERVER_SUPPORT
                case "server": return EditorGUIUtility.IconContent("BuildSettings.DedicatedServer On").image;
#endif // DEDICATEDSERVER_SUPPORT
                case "windows": return EditorGUIUtility.IconContent("BuildSettings.Metro On").image;
                // Couldn't find the OSX Icon so this will have to do since in the version i'm developing this on (6.1), this icon is just the apple icon.
                case "osx": case "macos": return EditorGUIUtility.IconContent("BuildSettings.tvOS On").image;
                case "linux": return EditorGUIUtility.IconContent("BuildSettings.EmbeddedLinux On").image;
                case "android": return EditorGUIUtility.IconContent("BuildSettings.Android On").image;
                case "ios": return EditorGUIUtility.IconContent("BuildSettings.iPhone On").image;
                case "webgl": return EditorGUIUtility.IconContent("BuildSettings.WebGL On").image;
                case "tvos": return EditorGUIUtility.IconContent("BuildSettings.tvOS On").image;
                case "visionos": return EditorGUIUtility.IconContent("BuildSettings.visionOS On").image;
                case "xbox": return EditorGUIUtility.IconContent("BuildSettings.XboxOne On").image;
                case "ps4": return EditorGUIUtility.IconContent("BuildSettings.PS4 On").image;
                case "ps5": return EditorGUIUtility.IconContent("BuildSettings.PS5 On").image;
                case "switch": return EditorGUIUtility.IconContent("BuildSettings.Switch On").image;
                default: return null;
            }
        }
    }
}
