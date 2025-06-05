using System.Collections.Generic;
using UnityEditor;

namespace Generalisk.QuickBuild.Editor
{
    public struct PlatformInfo
    {
        public string name;
        public string code;
        public BuildTarget buildTarget;

        /// <summary>
        /// The extension used for the executable (e.g. exe, apk)
        /// 
        /// Only works with <code>appendName</code>.
        /// </summary>
        public string ext;
        public bool appendName;

#if DEDICATEDSERVER_SUPPORT
        /// <summary>
        /// Whether this Platform is A Server Platform or not
        /// </summary>
        public bool server;
#endif // DEDICATEDSERVER_SUPPORT

        /// <summary>
        /// The icon name, which is given to the <code>QuickBuild.GetIcon()</code> function.
        /// </summary>
        public string icon;
    }

    public enum BuildType
    {
        Player = 0,
#if ASSETBUNDLE_SUPPORT
        AssetBundles = 1,
#endif // ASSETBUNDLE_SUPPORT
#if ADDRESSABLE_SUPPORT
        Addressables = 2,
#endif // ADDRESSABLE_SUPPORT
    }

    internal struct PlatformCategory
    {
        public string name;
        public List<byte> indexes;
    }
}
