namespace Stardust.Services.Tickable
{
	public interface ITickable
	{
		TickableState Status { get; }
	}
	/// <summary>
	/// LateUpdate ticks
	/// </summary>
	public interface ILateTickable : ITickable
	{
		void LateTick(float deltaSec);
	}
	/// <summary>
	/// FixedUpdate ticks
	/// </summary>
	public interface IPhysicallyTickable : ITickable
	{
		void PhysicsTick(float deltaSec);
	}
	/// <summary>
	/// Update ticks
	/// </summary>
	public interface IUpdateTickable : ITickable
	{
		void Tick(float deltaSec);
	}
	/// <summary>
	/// Ticks at end of each frame
	/// </summary>
	public interface IEndOfFrameTickable : ITickable
	{
		void EndOfFrameTick(float deltaSec);
	}

	/// <summary>
	/// Ticks based on system time
	/// </summary>
	public interface IIntervaledTickable : ITickable
	{
		void IntervaledTick();
	}
}

