namespace Stardust
{
	using System;
	using UnityEngine;
	using strange;
	using strange.extensions.context.api;
	using strange.extensions.context.impl;
	using strange.extensions.command.api;
	using strange.extensions.command.impl;
	using Stardust.Services;

	/// <summary>
	/// The global game context which exists throught the whole game life cycle. Holds the binding of all services, and global events.
	/// </summary>
	public class GameContext : MVCSContext 
	{
		protected IGameManager GameManager { get; private set; }
		
		public GameContext (MonoBehaviour view, GameManager gameManager) : base(view)
		{
			GameManager = gameManager;
		}

		public GameContext (MonoBehaviour view, ContextStartupFlags flags, GameManager gameManager) : base(view, flags)
		{
			GameManager = gameManager;
		}
		
		protected override void mapBindings()
		{
			BindCoreManagers();
			BindServices();

			// Pre-reflects everything.
			injectionBinder.ReflectAll();
		}

		protected virtual void BindCoreManagers()
		{
			injectionBinder.Bind<IGameServiceManager>().To<GameServiceManager>().ToSingleton();
		}

		protected virtual void BindServices()
		{
			injectionBinder.Bind<ILogService>().To<Stardust.Services.Log.LogService>().ToSingleton();
		}
	}
}