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

        /// <summary>
        /// This bundle will be cached permanently.
        /// </summary>
        public bool PermanentCache;

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

        private List<string> tempList = new List<string>();

        public T[] LoadAssets<T>(string assetBundleName)
            where T : UnityEngine.Object
        {
            return LoadAssets<T>(assetBundleName, false);
        }
        public T[] LoadAssets<T>(string assetBundleName, bool permanentCache, bool unloadBundleImmediately = false)
            where T : UnityEngine.Object
        {
            T[] assets = null;
            if (string.IsNullOrEmpty(assetBundleName))
            {
#if UNITY_EDITOR
                Debug.LogWarningFormat("Loading asset with empty name: {0}", assetBundleName);
#endif
                return assets;
            }
#if UNITY_EDITOR
            if (EditorStardustSettings.ResourcesSimulationMode || !EditorApplication.isPlaying)
            {
                string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName);
                if (assetPaths == null || assetPaths.Length == 0)
                {
                    Debug.LogError(string.Format("[ResourceManager] No asset is found with name {0}", assetBundleName));
                    return assets;
                }
                assets = new T[assetPaths.Length];
                for (int i = 0; i < assetPaths.Length; i ++)
                {
                    assets[i] = AssetDatabase.LoadMainAssetAtPath(assetPaths[i]) as T;
                }
                return assets;
            }
#endif
            AssetBundle bundle = LoadAssetBundle(assetBundleName, permanentCache);

            if (bundle != null)
            {
                assets = bundle.LoadAllAssets<T>();
                if (unloadBundleImmediately)
                {
                    UnloadBundle(assetBundleName, bundle);
                }
            }

            return assets;

        }
        public T LoadAsset<T>(string assetBundleName, string assetName)
            where T : UnityEngine.Object
        {
            return LoadAsset<T>(assetBundleName, assetName, false);
        }
        /// <summary>
        /// Loads an asset from specified assetbundle.
        /// </summary>
        public T LoadAsset<T>(string assetBundleName, string assetName, bool permanentCache, bool unloadBundleImmediately = false)
            where T: UnityEngine.Object
        {
            T asset = null;

            if (string.IsNullOrEmpty(assetName) || string.IsNullOrEmpty(assetBundleName))
            {
#if UNITY_EDITOR
                Debug.LogWarningFormat("Loading asset with empty name: {0}-{1}", assetBundleName, assetName);
#endif
                return asset;
            }
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

            AssetBundle bundle = LoadAssetBundle(assetBundleName, permanentCache);

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

        public void UnloadAllBundles()
        {
            tempList.Clear();
            foreach (var item in bundleCache.Keys)
            {
                if (!bundleCache[item].PermanentCache)
                {
                    tempList.Add(item);
                }
            }
            for (int i = 0; i < tempList.Count; i ++)
            {
                UnloadBundle(tempList[i]);
            }
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

        public void UnloadBundle(string assetBundleName)
        {
            if (bundleCache.ContainsKey(assetBundleName))
            {
                AssetBundle bundle = bundleCache[assetBundleName].AssetBundle;
                bundleCache.Remove(assetBundleName);
                bundle.Unload(false);
            }
        }
        /// <summary>
        /// Loads an assetbundle.
        /// </summary>
        public AssetBundle LoadAssetBundle(string assetBundleName, bool permanentCache = false)
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
                var loaded = new LoadedAssetBundle(bundle);
                loaded.PermanentCache = permanentCache;
                bundleCache.Add(assetBundleName, loaded);
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