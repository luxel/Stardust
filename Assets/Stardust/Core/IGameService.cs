namespace Stardust
{
	using System.Collections;

	public interface IGameService
	{
		/// <summary>
		/// Other services required by this service.
		/// </summary>
		RequireServiceAttribute[] RequiredServices { get; }
		/// <summary>
		/// Whether this service depends on other service. Which requires other services to be up before this service starts up.
		/// </summary>
		bool DependsOnOtherService { get; }
		/// <summary>
		/// Gets whether the game service is already started up and running.
		/// </summary>
		bool StartedUp { get; }
		/// <summary>
		/// Startup this service.
		/// </summary>
		void Startup();
	}
} 