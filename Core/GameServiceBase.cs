namespace Stardust
{
	using System;
	using System.Collections;

	public abstract class GameServiceBase : IGameService
	{
		public bool StartedUp { get; private set; }

		public RequireServiceAttribute[] RequiredServices { get; private set; }

		public GameServiceBase()
		{
			GetRequiredServices();
		}

		public bool DependsOnOtherService
		{
			get 
			{
				return RequiredServices != null && RequiredServices.Length > 0;
			}
		}

		public void Startup()
		{
			// Let child class handle its own logic.
			OnStartup();

			StartedUp = true;
		}

		protected void GetRequiredServices()
		{
			Type type = this.GetType();
			object[] requiredServices = type.GetCustomAttributes(typeof(RequireServiceAttribute), true);

			if (requiredServices.Length > 0)
			{
				RequiredServices = new RequireServiceAttribute[requiredServices.Length];

				for (int i = 0; i < requiredServices.Length; i ++)
				{				
					RequiredServices[i] = (RequireServiceAttribute) requiredServices[i];
				}
			}
		}

		protected virtual void OnStartup()
		{
			// Child class to implement when needed.
		}
	}
}