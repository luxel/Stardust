namespace Stardust.Services
{	
	using UnityEngine;

	public interface IResourceService : IGameService
	{
		T LoadResource<T> (string bundleName, string resourceName) where T: Component;
	}
}