namespace Stardust
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using strange;

	public class GameServiceManager : IGameServiceManager
	{
		[Inject]
		public IGameManager GameManager { get; set; }

		Dictionary<string, IGameService> services = new Dictionary<string, IGameService>();

		Dictionary<Type, IGameService> servicesWithTypes = new Dictionary<Type, IGameService>();

		public GameServiceManager()
		{
			
		}

		public T GetService<T> () where T : IGameService
		{
			return (T)servicesWithTypes[typeof(T)];
		}

		public T GetService<T> (string name) where T : IGameService
		{
			return (T)services[name];
		}

		public bool IsServiceUpAndRunning(Type type)
		{
			bool up = false;
			if (servicesWithTypes.ContainsKey(type))
			{
				up = servicesWithTypes[type].StartedUp;
			}
			return up;
		}

		public bool IsServiceUpAndRunning(string name)
		{
			bool up = false;
			if (services.ContainsKey(name))
			{
				up = services[name].StartedUp;
			}
			return up;
		}

		public void ResolveService<T> () where T: IGameService
		{
			T service = GameManager.GetInstance<T>();
			if (service == null)
			{
				throw new StardustException("Unable to resolve service for " + typeof(T).FullName);
			}

			// Check depending services
			if (service.DependsOnOtherService)
			{
				bool requiredServicesAllUp = true;
				for (int i = 0; i < service.RequiredServices.Length; i ++)
				{
					RequireServiceAttribute required = service.RequiredServices[i];
					if (required.DeterminedByServiceName && !IsServiceUpAndRunning(required.RequiredServiceName))
					{
						requiredServicesAllUp = false;
					}
					if (required.DeterminedByServiceType && !IsServiceUpAndRunning(required.RequiredServiceType))
					{
						requiredServicesAllUp = false;
					}

					if (!requiredServicesAllUp)
					{
						throw new StardustException("Required service is not up and running: " + (required.DeterminedByServiceName ? required.RequiredServiceName : required.RequiredServiceType.FullName));
					}
				}
			}

			// Starts up
			service.Startup();

			// Registering the serivce into collections
			servicesWithTypes.Add(typeof(T), service);
			services.Add(typeof(T).Name, service);
		}
	}
}