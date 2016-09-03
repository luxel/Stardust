namespace Stardust
{
	using System;
	using System.IO;
	using UnityEngine;

	/// <summary>
	/// Core environment methods used by the game or framework.
	/// </summary>
	public static class GameEnvironment
	{
		/// <summary>
		/// Gets the path to store common data files
		/// </summary>
		public static string DataPath
		{
			get 
			{
				return Application.persistentDataPath;
			}
		}

		private static string _DatabasePath = null;

		/// <summary>
		/// Gets the path to store the database files.
		/// </summary>
		public static string DatabasePath
		{
			get
			{
				if (_DatabasePath == null)
				{
					_DatabasePath = Path.Combine(DataPath, Folders.Database);
				}
				return _DatabasePath;
			}
		}

		private static string _ResourcesPath = null;
		/// <summary>
		/// Gets the path to store the resource files.
		/// </summary>
		public static string ResourcesPath
		{
			get 
			{
				if (_ResourcesPath == null)
				{
					_ResourcesPath = Path.Combine(DataPath, Folders.Resources);
				}
				return _ResourcesPath;
			}
		}
	}
}