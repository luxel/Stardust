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

        /// <summary>
        /// Gets the path to store data files shipped with package. (read-only)
        /// </summary>
        public static string PackageDataPath
        {
            get
            {
                return Application.streamingAssetsPath;
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

        private static string _PackageResourcesPath = null;
        /// <summary>
        /// Gets the path to the resource files shipped with package. (read-only)
        /// </summary>
        public static string PacakgeResourcesPath
        {
            get
            {
                if (_PackageResourcesPath == null)
                {
                    _PackageResourcesPath = Path.Combine(PackageDataPath, Folders.Resources);
                }
                return _PackageResourcesPath;
            }
        }
    }
}