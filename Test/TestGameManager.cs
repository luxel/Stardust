#if UNITY_EDITOR
namespace Stardust.Test
{
	using strange;
	using strange.extensions.context.impl;
	using Stardust;

	public class TestGameManager : Stardust.GameManager
	{
		public TestGameManager()
		{			
		}

		protected override CrossContext CreateGameContext ()
		{
			// Creates our own game context.
			return new TestGameContext(GameRoot, this);
		}

		protected override void OnInitializeServices ()
		{
			// Custom services.
			// GameService.ResolveService<ISomeService>();
		}
	}
}
#endif