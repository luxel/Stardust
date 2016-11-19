namespace Stardust.Services.Resources
{
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System;
#if UNITY_EDITOR
    using UnityEditor;
#endif

    public class LoadedAssetBundle
    {
        public AssetBundle AssetBundle { get; private set; }
        /// <summary>
        /// Used for dependency bundles, not used for now.
        /// </summary>
        public int ReferencedCount { get; private set; }

        internal event Action unload;

        internal void OnUnload()
        {
            AssetBundle.Unload(false);
            if (unload != null)
                unload();
        }

        public LoadedAssetBundle(AssetBundle assetBundle)
        {
            AssetBundle = assetBundle;
            ReferencedCount = 1;
        }

        public void IncreaseReference()
        {
            ReferencedCount++;
        }

        public void DecreaseReference()
        {
            ReferencedCount--;
        }
    }

    public class ResourceManager
    {
        protected Dictionary<string, LoadedAssetBundle> bundleCache = new Dictionary<string, LoadedAssetBundle>();
  
        /// <summary>
        /// Loads an asset from specified assetbundle.
        /// </summary>
        public T LoadAsset<T>(string assetBundleName, string assetName, bool unloadBundleImmediately = false)
            where T : UnityEngine.Object
        {
            T asset = null;

#if UNITY_EDITOR
            if (StardustEditorSettings.ResourcesSimulationMode)
            {
                string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetBundleName, assetName);
                if (assetPaths == null || assetPaths.Length == 0)
                {
                    Debug.LogError(string.Format("[ResourceManager] No asset is found with name {0} in {1}", assetName, assetBundleName));
                    return asset;
                }
                asset = AssetDatabase.LoadMainAssetAtPath(assetPaths[0]) as T;
                return asset;
            }
#endif

            AssetBundle bundle = LoadAssetBundle(assetBundleName);

            if (bundle != null)
            {
                asset = bundle.LoadAsset<T>(assetName);
                if (unloadBundleImmediately)
                {
                    UnloadBundle(assetBundleName, bundle);
                }
            }

            return asset;
        }

        /// <summary>
        /// Unloads a assetbundle and removes it from cache.
        /// </summary>
        public void UnloadBundle(string assetBundleName, AssetBundle bundle)
        {
            if (bundleCache.ContainsKey(assetBundleName))
            {
                bundleCache.Remove(assetBundleName);
            }
            bundle.Unload(false);
        }

        /// <summary>
        /// Loads an assetbundle.
        /// </summary>
        public AssetBundle LoadAssetBundle(string assetBundleName)
        {
            AssetBundle bundle = null;

            // 1. try to load from cache.
            LoadedAssetBundle loadedBundle = null;
            bundleCache.TryGetValue(assetBundleName, out loadedBundle);
            if (loadedBundle != null)
            {
                bundle = loadedBundle.AssetBundle;
            }

            // 2. try to load from the in-package bundle file.
            if (bundle == null)
            {
                string bundleFile = Path.Combine(GameEnvironment.PacakgeResourcesPath, assetBundleName);
                if (File.Exists(bundleFile))
                {
                    bundle = AssetBundle.LoadFromFile(bundleFile);
                }
            }

            // 3. add the bundle into cache
            if (bundle != null && loadedBundle == null)
            {
                bundleCache.Add(assetBundleName, new LoadedAssetBundle(bundle));
            }

            return bundle;
        }

        public void LoadLevel(string assetBundleName, string levelName, bool isAdditive)
        {
#if UNITY_EDITOR
            if (StardustEditorSettings.ResourcesSimulationMode)
            {
                ResourceManager.LoadLevelFromAssetPath(assetBundleName, levelName, isAdditive);
                return;
            }
#endif
            LoadAssetBundle(assetBundleName);
            SceneManager.LoadScene(levelName, isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);
        }

        public AsyncOperation LoadLevelAsync(string assetBundleName, string levelName, bool isAdditive)
        {
#if UNITY_EDITOR
            if (StardustEditorSettings.ResourcesSimulationMode)
            {
                return ResourceManager.LoadLevelAsyncFromAssetPath(assetBundleName, levelName, isAdditive);
            }
#endif
            LoadAssetBundle(assetBundleName);
            return SceneManager.LoadSceneAsync(levelName, isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);
        }

#if UNITY_EDITOR
        public static void LoadLevelFromAssetPath(string assetBundleName, string levelName, bool isAdditive)
        {
            string[] levelPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetBundleName, levelName);
            if (levelPaths.Length == 0)
            {
                Debug.LogError("There is no scene with name \"" + levelName + "\" in " + assetBundleName);
                return;
            }

            if (isAdditive)
                EditorApplication.LoadLevelAdditiveInPlayMode(levelPaths[0]);
            else
                EditorApplication.LoadLevelInPlayMode(levelPaths[0]);
        }

        public static AsyncOperation LoadLevelAsyncFromAssetPath(string assetBundleName, string levelName, bool isAdditive)
        {
            string[] levelPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetBundleName, levelName);
            if (levelPaths.Length == 0)
            {
                Debug.LogError("There is no scene with name \"" + levelName + "\" in " + assetBundleName);
                return null;
            }

            if (isAdditive)
                return EditorApplication.LoadLevelAdditiveAsyncInPlayMode(levelPaths[0]);
            else
                return EditorApplication.LoadLevelAsyncInPlayMode(levelPaths[0]);
        }
#endif   
    }
}