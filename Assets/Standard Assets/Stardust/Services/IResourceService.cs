namespace Stardust.Services
{	
	using UnityEngine;

	public interface IResourceService : IGameService
	{
        T LoadAsset<T>(string assetBundleName, string assetName, bool unloadBundleImmediately = false) where T : UnityEngine.Object;
        AssetBundle LoadAssetBundle(string assetBundleName);
        void LoadLevel(string assetBundleName, string levelName, bool isAdditive);
        AsyncOperation LoadLevelAsync(string assetBundleName, string levelName, bool isAdditive);
    }
}