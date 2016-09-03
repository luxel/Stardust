#if UNITY_EDITOR
namespace Stardust.Test
{
	using UnityEngine;
	using strange;
	using Stardust;
	using strange.extensions.context.api;

	public class TestGameContext : GameContext 
	{
		public TestGameContext (MonoBehaviour view, GameManager game) : base(view, game)
		{
		}

		public TestGameContext (MonoBehaviour view, ContextStartupFlags flags, GameManager game) : base(view, flags, game)
		{
		}

		protected override void BindCoreManagers()
		{
			base.BindCoreManagers();

			// Custom manager binding.
		}

		protected override void BindServices ()
		{
			base.BindServices ();

			// Custom service binding, can also overwrite base class bindings.
		}
	}
}
#endif