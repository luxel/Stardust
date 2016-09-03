namespace Stardust
{
	using System;

	public interface IGameServiceManager
	{
		/// <summary>
		/// Checks whether the service with specified type is already up and running.
		/// </summary>
		bool IsServiceUpAndRunning(Type type);
		/// <summary>
		/// Checks whether the service with specified name is already up and running.
		/// </summary>
		bool IsServiceUpAndRunning(string name);
		/// <summary>
		/// Gets a service with its type. This is a bit slower comparing to T GetService<T> (string name).
		/// </summary>
		T GetService<T> () where T : IGameService;
		/// <summary>
		/// Gets a service with its name. Usually it will be the name of the service interface.
		/// Try to use this as much as possible because it's faster than T GetService<T> ().
		/// E.g. "ILogService" for ILogService
		/// </summary>
		T GetService<T> (string name) where T : IGameService;
		/// <summary>
		/// Try to resolve a service and register it into service cache.
		/// </summary>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		void ResolveService<T>() where T: IGameService;
	}
}