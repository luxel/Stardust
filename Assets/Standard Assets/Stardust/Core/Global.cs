namespace Stardust
{
	using System;
	using UnityEngine;

	/// <summary>
	/// Global core component references.
	/// </summary>
	public static class Global
	{
		/// <summary>
		/// Reference to the current running IGameManager instance.
		/// </summary>
		public static IGameManager GameManager { get; internal set; }

		/// <summary>
		/// Reference to the current running IGameServiceManager instance.
		/// </summary>
		/// <value>The services.</value>
		public static IGameServiceManager ServiceManager { get; internal set; }

		/// <summary>
		/// The root game object for Stardust.
		/// </summary>
		public static GameObject RootGo { get; internal set; }
	}
}