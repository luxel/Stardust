namespace Stardust
{
	using UnityEngine;

	public interface IGameManager 
	{
		T GetInstance<T>();
		void GameInitialize(strange.extensions.context.impl.ContextView root);
		/// <summary>
		/// GameManager startup
		/// </summary>
		void GameStartup();
		/// <summary>
		/// GameManager quit(shutdown)
		/// </summary>
		void GameQuit();
		/// <summary>
		/// GameManager pause
		/// </summary>
		void GamePause();
		/// <summary>
		/// GameManager resume
		/// </summary>
		void GameResume();
	}
}
