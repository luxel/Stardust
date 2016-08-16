namespace Stardust.Services.Resources
{
	using UnityEngine;
	using System.Collections.Generic;
	using System.IO;
	using Stardust;
	using Stardust.Utilities;
	using Stardust.Utilities.ObjectDb;

	public sealed class ResourceService : GameServiceBase
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

		private SimpleObjectDB<string, ResourceItem> data = new SimpleObjectDB<string, ResourceItem>(Databases.Resources);

		private int gameVersion = 0;

		public T LoadResource<T> (string bundleName, string resourceName) where T: Component
		{
			string key = GetStringKey(bundleName, resourceName);
			T resource = null;

			// Whether to load from downloaded asset bundles.
			bool loadFromBundle = false;
			// Whether to load from the built-in package resources.
			bool loadFromPackage = false;
			// Whether to load from the cache.
			bool loadFromCache = false;

			// Cache checking

			// Actual loading.

			if (loadFromPackage)
			{
				resource = Resources.Load<T>(key);
			}
			else if (loadFromBundle)
			{
				// TODO: Loads it from the bundle.
			}

			return resource;
		}

		protected override void OnStartup ()
		{
			// Load the DB locally
			data.Load();
			// TODO: Calculate the current game version.

		}
	}
}