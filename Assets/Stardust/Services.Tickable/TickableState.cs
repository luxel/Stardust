using System;

namespace Stardust.Services.Tickable
{
	public enum TickableState : byte
	{
		/// <summary>
		/// The tickable is running normally, each tick should be executed.
		/// </summary>
		Running,
		/// <summary>
		/// The tickable is paused, and any tick execution should be stopped.
		/// </summary>
		Paused,
		/// <summary>
		/// The tickable is completed and it should be removed from the tickable service.
		/// </summary>
		Completed
	}
}

