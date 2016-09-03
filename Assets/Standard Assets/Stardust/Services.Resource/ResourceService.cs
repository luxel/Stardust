namespace Stardust.Services.Resources
{
	using UnityEngine;
	using System.Collections.Generic;
	using System.IO;
	using Stardust;
	using Stardust.Utilities;
	using Stardust.Utilities.ObjectDb;

	public sealed class ResourceService : GameServiceBase, IResourceService
	{	
		#region static members

		private static StringCache _StringCache = new StringCache();

		private static readonly string ResourcePathFormat = "{0}/{1}";

		private static string GetStringKey(string bundleName, string resourceName)
		{
			return _StringCache.GetFormatted(ResourcePathFormat, bundleName, resourceName);
		}

		private static AssetBundle LoadAssetBundleFromFile (string bundleName)
		{
			string path = Path.Combine(GameEnvironment.ResourcesPath, bundleName);

			if(!File.Exists(path))
			{
				return null;
			}

			return AssetBundle.LoadFromFile(path);
		}
        #endregion

        private ResourceManager manager;

        public T LoadAsset<T>(string assetBundleName, string assetName, bool unloadBundleImmediately = false) where T : UnityEngine.Object
        {
            return manager.LoadAsset<T>(assetBundleName, assetName, unloadBundleImmediately);
        }
        public AssetBundle LoadAssetBundle(string assetBundleName)
        {
            return manager.LoadAssetBundle(assetBundleName);
        }
        public void LoadLevel(string assetBundleName, string levelName, bool isAdditive)
        {
            manager.LoadLevel(assetBundleName, levelName, isAdditive);
        }
        public AsyncOperation LoadLevelAsync(string assetBundleName, string levelName, bool isAdditive)
        {
            return manager.LoadLevelAsync(assetBundleName, levelName, isAdditive);
        }

        protected override void OnStartup ()
        {
            manager = new Services.Resources.ResourceManager();
        }
	}
}