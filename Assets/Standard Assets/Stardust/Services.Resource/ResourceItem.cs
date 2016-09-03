namespace Stardust.Services.Resources
{
	using System;
	using System.Collections;
	using Stardust.Utilities;

	public class ResourceItem : IObjectWithId<string>
	{
		/// <summary>
		/// Id of the object.
		/// </summary>
		public string Id
		{
			get 
			{
				return Name;
			}
		}

		/// <summary>
		/// Name of the resource, also the key of it. It's a relative path to the resouce.
		/// </summary>
		public string Name;

		/// <summary>
		/// The version of the resource which is already inside the package.
		/// </summary>
		public int PackageVersion;

		/// <summary>
		/// The version which has been downloaded locally.
		/// </summary>
		public int LocalVersion;
	}
}