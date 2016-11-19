#if UNITY_EDITOR
namespace Stardust.Editor
{
    using UnityEngine;
    using UnityEditor;
    using System.Collections;
    using System.IO;

    /// <summary>
    /// Environment for editor.
    /// </summary>
    public static class EditorEnvironment
    {
        /// <summary>
        /// Path to the root "Assets" folder.
        /// </summary>
        public static string AssetsRootPath
        {
            get
            {
                return Application.dataPath;
            }
        }

        private static string _EditorAssetsPath;

        /// <summary>
        /// Path to the assets for editor.
        /// E.g. "Assets/Stardust"
        /// </summary>
        public static string EditorAssetsPath
        {
            get
            {
                if (string.IsNullOrEmpty(_EditorAssetsPath))
                {
                    _EditorAssetsPath = Path.Combine(AssetsRootPath, EditorConstants.Assets);
                }
                return _EditorAssetsPath;
            }
        }

        private static string _EditorDataFilePath;
        /// <summary>
        /// Path to the data file assets used under editor.
        /// E.g. "Assets/KUKRTools/Data"
        /// </summary>
        public static string EditorDataFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(_EditorDataFilePath))
                {
                    _EditorDataFilePath = Path.Combine(EditorAssetsPath, EditorConstants.DataFileFolder);
                }
                return _EditorDataFilePath;
            }
        }

        private static string _EditorSceneFilePath;
        /// <summary>
        /// Path to the scene files under editor.
        /// E.g. "Assets/Scenes"
        /// </summary>
        public static string EditorSceneFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(_EditorSceneFilePath))
                {
                    _EditorSceneFilePath = Path.Combine(AssetsRootPath, EditorConstants.SceneFileFolder);
                }
                return _EditorSceneFilePath;
            }
        }

        public static string GetEditorDataFile(string name)
        {
            return Path.Combine(EditorDataFilePath, string.Format("{0}.{1}", name, EditorConstants.DataFileExtension));
        }

        public static string GetSceneFile(string nameWithouExtension)
        {
            return Path.Combine(EditorSceneFilePath, string.Format("{0}.{1}", nameWithouExtension, EditorConstants.SceneFileExtension));
        }

        private static string _CurrentUnityMajorVersion;

        /// <summary>
        /// Gets the major release unity version like "5.4"
        /// </summary>
        public static string CurrentUnityMajorVersion
        {
            get
            {
                if (string.IsNullOrEmpty(_CurrentUnityMajorVersion))
                {
                    // Full version is like: 5.4.0f3
                    string fullVersion = Application.unityVersion;

                    int firstDot = fullVersion.IndexOf('.');
                    int secondDot = fullVersion.IndexOf('.', firstDot + 1);

                    if (secondDot > 0)
                    {
                        _CurrentUnityMajorVersion = fullVersion.Substring(0, secondDot);
                    }
                    else
                    {
                        _CurrentUnityMajorVersion = fullVersion;
                    }
                }
                return _CurrentUnityMajorVersion;
            }

        }

        public static string CurretPlatformName
        {
            get
            {
                string platform = "notsupported";
                switch (EditorUserBuildSettings.activeBuildTarget)
                {
                    case BuildTarget.Android:
                        platform = "Android";
                        break;
                    case BuildTarget.StandaloneWindows:
                        platform = Path.Combine("PC", "x86");
                        break;
                    case BuildTarget.StandaloneWindows64:
                        platform = Path.Combine("PC", "x86_64");
                        break;
                    case BuildTarget.StandaloneOSXIntel:
                    case BuildTarget.StandaloneOSXIntel64:
                    case BuildTarget.StandaloneOSXUniversal:
                        platform = "OSX";
                        break;
                    case BuildTarget.iOS:
                        platform = "iOS";
                        break;
                    // Others - not supported now.
                    case BuildTarget.WebGL:
                    default:
                        break;
                }
                return platform;
            }
        }

        public static string GetPlatformSpecificAssetPathForCurrentUnity(string assetFolderName)
        {
            // Assuming assetFolderName = "mono"
            // E.g. Assets/KUKRTools/mono
            string path = Path.Combine(EditorAssetsPath, assetFolderName);

            // E.g. Assets/KUKRTools/mono/5.4
            path = Path.Combine(path, CurrentUnityMajorVersion);

            // E.g. Assets/KUKRTools/mono/5.4/PC/x86
            return Path.Combine(path, CurretPlatformName);

        }

        public static string GetPlatformSpecificAssetPath(string assetFolderName)
        {
            // Assuming assetFolderName = "mono"
            // E.g. Assets/KUKRTools/mono
            string path = Path.Combine(EditorAssetsPath, assetFolderName);

            // E.g. Assets/KUKRTools/mono/PC/x86
            return Path.Combine(path, CurretPlatformName);
        }
    } 
}

#endif