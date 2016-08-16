using System;

namespace Stardust.Services.Tickable
{
	public class TickService : GameServiceBase, ITickService
	{
		private TickBehaviour ticker;

		public void Add(ITickable tickable)
		{
			if (tickable is IUpdateTickable)
				ticker.Add(tickable as IUpdateTickable);

			if (tickable is IPhysicallyTickable)
				ticker.AddPhysic(tickable as IPhysicallyTickable);

			if (tickable is ILateTickable)
				ticker.AddLate(tickable as ILateTickable);

			if (tickable is IEndOfFrameTickable)
				ticker.AddEndOfFrame(tickable as IEndOfFrameTickable);

			if (tickable is IIntervaledTickable)
				ticker.AddIntervaled(tickable as IIntervaledTickable);
		}

		public void Remove(ITickable tickable)
		{
			if (tickable is IUpdateTickable)
				ticker.Remove(tickable as IUpdateTickable);

			if (tickable is IPhysicallyTickable)
				ticker.RemovePhysic(tickable as IPhysicallyTickable);

			if (tickable is ILateTickable)
				ticker.RemoveLate(tickable as ILateTickable);

			if (tickable is IEndOfFrameTickable)
				ticker.RemoveEndOfFrame(tickable as IEndOfFrameTickable);

			if (tickable is IIntervaledTickable)
				ticker.RemoveIntervaled(tickable as IIntervaledTickable);
		}

		protected override void OnStartup()
		{
			// Attach the ticker to root game object.
			this.ticker = Global.RootGo.AddComponent<TickBehaviour>();
		}
	}
}