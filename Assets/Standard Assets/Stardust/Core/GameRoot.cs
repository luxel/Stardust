namespace Stardust
{
	using UnityEngine;
	using System.Collections;

	/// <summary>
	/// The core application context GameObject for Stardust running in Unity Engine.
	/// </summary>
	public class GameRoot<T> : strange.extensions.context.impl.ContextView where T: GameManager, new()
	{	
		#region Singleton

		private static GameRoot<T> _instance;

		private static bool _applicationIsQuitting = false;

		public static GameRoot<T> Instance
		{
			get 
			{
				if (_applicationIsQuitting) 
				{
					#if UNITY_EDITOR
					Debug.LogWarning("[GameRoot] Application quitting!");
					#endif
					return null;
				}
				if (_instance == null)
				{
					Debug.LogError("[GameRoot] Game Root is not created or missing in current scene!");
					return null;
				}
				return _instance;
			}
		}

		#endregion

		/// <summary>
		/// Gets the reference to the game manager.
		/// </summary>
		public IGameManager GameManager { get; private set; }

		/// <summary>
		/// Whether game manager startup should be called automatically in Start().
		/// </summary>
		public bool AutoStartup = false;

		protected virtual void Awake()
		{
			// In one game only one game root is permitted.
			if (_instance != null)
			{
				Debug.LogError("[GameRoot] Another game root already exists in current game! Something went wrong! New game root will not be created!");
				// Self destroy, and disable
				GameObject.Destroy(this.gameObject);
				this.enabled = false;
				return;
			}

			// Create the game manager.
			GameManager = new T();

			// Initializations
			GameManager.GameInitialize(this);

			// Game root should exist for whole game.
			GameObject.DontDestroyOnLoad(this.gameObject);
			_instance = this;
		}

		protected virtual void Start()
		{
			if (AutoStartup)
			{
				// Starts up the game
				GameManager.GameStartup();
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			_instance = null;
		}

		/// <summary>
		/// Handles the game pause/resume events
		/// </summary>
		protected void OnApplicationPause(bool paused)
		{
			if (paused)
			{
				GameManager.GamePause();
			}
			else
			{
				GameManager.GameResume();
			}
		}

		/// <summary>
		/// Handles the game quit event.
		/// </summary>
		protected void OnApplicationQuit()
		{
			_applicationIsQuitting = true;
			GameManager.GameQuit();
		}
	}
}