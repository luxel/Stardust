namespace Stardust
{
	using System;
	using UnityEngine;
	using strange;
	using strange.extensions.context.api;
	using strange.extensions.context.impl;
	using Stardust.Services;

	/// <summary>
	/// The global game manager which exists throught the whole game life cycle.
	/// </summary>
	public class GameManager : IGameManager
	{		
		public IGameServiceManager GameService { get; private set; }

		/// <summary>
		/// Gets whether the GameManager is already initialized (GameInitialize executed)
		/// </summary>
		public bool IsInitialized { get; private set; }
		/// <summary>
		/// Gets whether the GameManager is already started up. (GameStartup executed)
		/// </summary>
		public bool IsStarted { get; private set; }

		/// <summary>
		/// Underlining context for the game manager.
		/// </summary>
		protected ContextView GameRoot { get; private set; }
		/// <summary>
		/// Refrence to the CrossContext.
		/// </summary>
		protected CrossContext Context { get; private set; }

		public GameManager() {}

		public void GameInitialize(ContextView root)
		{
			GameRoot = root;

			CoreInitialization();
		}

		/// <summary>
		/// GameManager startup
		/// </summary>
		public void GameStartup()
		{			
			if (!IsInitialized)
			{
				throw new StardustException("GameManager is not initialized, cannot start up!");
			}

			if (IsStarted)
			{
				return;
			}

			OnBeforeStartup();

			InitializeServices();

			OnAfterStartup();

			#if UNITY_EDITOR
			Debug.Log("[GameManager] startup completed.");
			#endif
		}
		/// <summary>
		/// GameManager quit(shutdown)
		/// </summary>
		public void GameQuit()
		{
		}
		/// <summary>
		/// GameManager pause
		/// </summary>
		public void GamePause()
		{
		}
		/// <summary>
		/// GameManager resume
		/// </summary>
		public void GameResume()
		{
		}

		public T GetInstance<T>()
		{
			return Context.injectionBinder.GetInstance<T>();
		}

		/// <summary>
		/// Executes at the begining of GameStartup, before everything else.
		/// </summary>
		protected virtual void OnBeforeStartup()  
		{
			// Child class to implement when needed.
		}
		protected virtual void OnCoreInitialization()
		{
			// Child class to implement when needed.
		}
		protected virtual void OnInitializeServices() 
		{
			// Child class to implement when needed.
		}
		/// <summary>
		/// Executes at the end of GameStartup.
		/// </summary>
		protected virtual void OnAfterStartup() 
		{
			// Child class to implement when needed.
		}

		/// <summary>
		/// Creates the global game context, child class should override this to provide its own context.
		/// </summary>
		protected virtual CrossContext CreateGameContext()
		{
			return new GameContext(GameRoot, this);
		}

		private void CoreInitialization()
		{
			// Core initialization logic - context intialization
			Context = CreateGameContext();
			GameRoot.context = Context;
			Context.injectionBinder.Bind<IGameManager>().ToValue(this);

			// Service initialization logic
			GameService = GetInstance<IGameServiceManager>();

			// Set the global references
			Global.GameManager = this;
			Global.ServiceManager = GameService;
			Global.RootGo = GameRoot.gameObject;

			// Child class hook
			OnCoreInitialization();

			IsInitialized = true;
		}

		private void InitializeServices()
		{
			// Now prepare the Stardust built-in services
			GameService.ResolveService<ILogService>();

			// Child class hook
			OnInitializeServices();
		}
	}
}