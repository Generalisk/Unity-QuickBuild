using UnityEditor;

namespace Generalisk.QuickBuild.Editor
{
    /// <summary>
    /// A Collection of Constant values used for the QuickBuild Package
    /// </summary>
    public static class Shared
    {
        /// <summary>
        /// A list of all the Platforms to display in the Quick Build Window
        /// </summary>
        public static PlatformInfo[] Platforms { get; } =
        {
            new PlatformInfo()
            {
#if DEDICATEDSERVER_SUPPORT
                name = "Windows/Client/32-bit",
#else // DEDICATEDSERVER_SUPPORT
                name = "Windows/32-bit",
#endif // DEDICATEDSERVER_SUPPORT
                code = "Windows (x86)",
                buildTarget = BuildTarget.StandaloneWindows,
                ext = "exe",
                appendName = true,
#if DEDICATEDSERVER_SUPPORT
                server = false,
#endif // DEDICATEDSERVER_SUPPORT
                icon = "",
            },
            new PlatformInfo()
            {
#if DEDICATEDSERVER_SUPPORT
                name = "Windows/Client/64-bit",
#else // DEDICATEDSERVER_SUPPORT
                name = "Windows/64-bit",
#endif // DEDICATEDSERVER_SUPPORT
                code = "Windows",
                buildTarget = BuildTarget.StandaloneWindows64,
                ext = "exe",
                appendName = true,
#if DEDICATEDSERVER_SUPPORT
                server = false,
#endif // DEDICATEDSERVER_SUPPORT
                icon = "",
            },
#if DEDICATEDSERVER_SUPPORT
            new PlatformInfo()
            {
                name = "Windows/Server/32-bit",
                code = "Windows (x86) - Dedicated Server",
                buildTarget = BuildTarget.StandaloneWindows,
                ext = "exe",
                appendName = true,
                server = true,
                icon = "",
            },
            new PlatformInfo()
            {
                name = "Windows/Server/64-bit",
                code = "Windows - Dedicated Server",
                buildTarget = BuildTarget.StandaloneWindows64,
                ext = "exe",
                appendName = true,
                server = true,
                icon = "",
            },
#endif // DEDICATEDSERVER_SUPPORT
            new PlatformInfo()
            {
#if DEDICATEDSERVER_SUPPORT
                name = "MacOS/Client",
#else // DEDICATEDSERVER_SUPPORT
                name = "MacOS",
#endif // DEDICATEDSERVER_SUPPORT
                code = "MacOS",
                buildTarget = BuildTarget.StandaloneOSX,
                ext = "app",
                appendName = true,
#if DEDICATEDSERVER_SUPPORT
                server = false,
                icon = "Client",
#else // DEDICATEDSERVER_SUPPORT
                icon = "OSX",
#endif // DEDICATEDSERVER_SUPPORT
            },
#if DEDICATEDSERVER_SUPPORT
            new PlatformInfo()
            {
                name = "MacOS/Server",
                code = "MacOS - Dedicated Server",
                buildTarget = BuildTarget.StandaloneOSX,
                ext = "app",
                appendName = true,
                server = true,
                icon = "Server",
            },
#endif // DEDICATEDSERVER_SUPPORT
            new PlatformInfo()
            {
#if DEDICATEDSERVER_SUPPORT
                name = "Linux/Client",
#else // DEDICATEDSERVER_SUPPORT
                name = "Linux",
#endif // DEDICATEDSERVER_SUPPORT
                code = "Linux",
                buildTarget = BuildTarget.StandaloneLinux64,
                ext = "x86_64",
                appendName = true,
#if DEDICATEDSERVER_SUPPORT
                server = false,
                icon = "Client",
#else // DEDICATEDSERVER_SUPPORT
                icon = "Linux",
#endif // DEDICATEDSERVER_SUPPORT
            },
#if DEDICATEDSERVER_SUPPORT
            new PlatformInfo()
            {
                name = "Linux/Server",
                code = "Linux - Dedicated Server",
                buildTarget = BuildTarget.StandaloneLinux64,
                ext = "x86_64",
                appendName = true,
                server = true,
                icon = "Server",
            },
#endif // DEDICATEDSERVER_SUPPORT
            new PlatformInfo()
            {
                name = "Android",
                code = "Android",
                buildTarget = BuildTarget.Android,
                ext = "apk",
                appendName = true,
#if DEDICATEDSERVER_SUPPORT
                server = false,
#endif // DEDICATEDSERVER_SUPPORT
                icon = "Android",
            },
            new PlatformInfo()
            {
                name = "iOS",
                code = "iOS",
                buildTarget = BuildTarget.iOS,
                ext = "",
                appendName = false,
#if DEDICATEDSERVER_SUPPORT
                server = false,
#endif // DEDICATEDSERVER_SUPPORT
                icon = "IOS",
            },
            new PlatformInfo()
            {
                name = "WebGL",
                code = "WebGL",
                buildTarget = BuildTarget.WebGL,
                ext = "",
                appendName = false,
#if DEDICATEDSERVER_SUPPORT
                server = false,
#endif // DEDICATEDSERVER_SUPPORT
                icon = "WebGL",
            },
            new PlatformInfo()
            {
                name = "UWP",
                code = "UWP",
                buildTarget = BuildTarget.WSAPlayer,
                ext = "",
                appendName = false,
#if DEDICATEDSERVER_SUPPORT
                server = false,
#endif // DEDICATEDSERVER_SUPPORT
                icon = "Windows",
            },
            new PlatformInfo()
            {
                name = "tvOS",
                code = "tvOS",
                buildTarget = BuildTarget.tvOS,
                ext = "",
                appendName = false,
#if DEDICATEDSERVER_SUPPORT
                server = false,
#endif // DEDICATEDSERVER_SUPPORT
                icon = "tvOS",
            },
            new PlatformInfo()
            {
                name = "visionOS",
                code = "visionOS",
                buildTarget = BuildTarget.VisionOS,
                ext = "",
                appendName = false,
#if DEDICATEDSERVER_SUPPORT
                server = false,
#endif // DEDICATEDSERVER_SUPPORT
                icon = "visionOS",
            },
            // TODO: figure out extension and name appendment for this platform
            new PlatformInfo()
            {
                name = "PlayStation/PS4",
                code = "PS4",
                buildTarget = BuildTarget.PS4,
                ext = "",
                appendName = true,
#if DEDICATEDSERVER_SUPPORT
                server = false,
#endif // DEDICATEDSERVER_SUPPORT
                icon = "PS4",
            },
            // TODO: figure out extension and name appendment for this platform
            new PlatformInfo()
            {
                name = "PlayStation/PS5",
                code = "PS5",
                buildTarget = BuildTarget.PS5,
                ext = "",
                appendName = true,
#if DEDICATEDSERVER_SUPPORT
                server = false,
#endif // DEDICATEDSERVER_SUPPORT
                icon = "PS5",
            },
            // TODO: figure out extension and name appendment for this platform
            new PlatformInfo()
            {
                name = "Nintendo Switch",
                code = "Switch",
                buildTarget = BuildTarget.Switch,
                ext = "",
                appendName = true,
#if DEDICATEDSERVER_SUPPORT
                server = false,
#endif // DEDICATEDSERVER_SUPPORT
                icon = "Switch",
            },
            // TODO: figure out extension and name appendment for this platform
            new PlatformInfo()
            {
                name = "Xbox/Xbox One/Game Core",
                code = "Xbox One",
                buildTarget = BuildTarget.GameCoreXboxOne,
                ext = "",
                appendName = true,
#if DEDICATEDSERVER_SUPPORT
                server = false,
#endif // DEDICATEDSERVER_SUPPORT
                icon = "",
            },
            // TODO: figure out extension and name appendment for this platform
            new PlatformInfo()
            {
                name = "Xbox/Xbox One/Legacy",
                code = "Xbox One (Legacy)",
                buildTarget = BuildTarget.XboxOne,
                ext = "",
                appendName = true,
#if DEDICATEDSERVER_SUPPORT
                server = false,
#endif // DEDICATEDSERVER_SUPPORT
                icon = "",
            },
            // TODO: figure out extension and name appendment for this platform
            new PlatformInfo()
            {
                name = "Xbox/Series",
                code = "Xbox Series",
                buildTarget = BuildTarget.GameCoreXboxSeries,
                ext = "",
                appendName = true,
#if DEDICATEDSERVER_SUPPORT
                server = false,
#endif // DEDICATEDSERVER_SUPPORT
                icon = "",
            },
        };
    }
}
