namespace Stardust.Services
{
	using Stardust.Services.Tickable;

	public interface ITickService : IGameService
	{
		void Add(ITickable tickable);
		void Remove(ITickable tickable);
	}
}