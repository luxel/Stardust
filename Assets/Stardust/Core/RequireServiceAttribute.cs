namespace Stardust
{
	using System;

	/// <summary>
	/// Declares a Stardust service to require other services to be up and running before it startsup.
	/// This attribute should only used on classes which inderites from GameServiceBase
	/// </summary>
	[AttributeUsage(AttributeTargets.Class,
		AllowMultiple = true,
		Inherited = true)]
	public class RequireServiceAttribute : Attribute
	{
		/// <summary>
		/// Name of the service required.
		/// </summary>
		public string RequiredServiceName { get; private set; }
		/// <summary>
		/// Whether the required service is determined by name.
		/// </summary>
		public bool DeterminedByServiceName
		{
			get 
			{
				return !string.IsNullOrEmpty(RequiredServiceName);
			}
		}
		/// <summary>
		/// Type of the service required.
		/// </summary>
		public Type RequiredServiceType { get; private set; }
		/// <summary>
		/// Whether the required service is determined by type.
		/// </summary>
		public bool DeterminedByServiceType
		{
			get 
			{
				return !DeterminedByServiceName && RequiredServiceType != null;
			}
		}
		
		public RequireServiceAttribute (string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}
			
			RequiredServiceName = name;
		}

		public RequireServiceAttribute (Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			RequiredServiceType = type;
		}
	}
}