namespace Stardust
{
    public static class Constants
    {
        public const string EditorMenuPrefix = "Stardust";
    }
	public static class ServiceNames
	{
//		public const string LogService = "ILogService";
	}
	internal static class Folders
	{
		/// <summary>
		/// Folder name for database files storage
		/// </summary>
		public static readonly string Database = "db";
		/// <summary>
		/// Folder name for resources files storage
		/// </summary>
		public static readonly string Resources = "res";
	}
	internal static class Databases
	{
		/// <summary>
		/// Name for resources DB.
		/// </summary>
		public static readonly string Resources = "resources";
	}
#if UNITY_EDITOR
    public static class EditorPrefKeys
    {
        public static readonly string ResourceManagerSimulationMode = "Stardust_Resource_Simulation";
		public static readonly string BuildOutputPath = "Stardust_BuildOutputPath";
		public static readonly string BuildLastTargetPath = "Stardust_BuildTargetPath";
		public static readonly string BuildLastTargetName = "Stardust_BuildTargetName";
	}
	/// <summary>
	/// Constants used for editor
	/// </summary>
	public static class EditorConstants
	{
		/// <summary>
		/// Root folder to store the assets used under editor
		/// </summary>
		public const string Assets = "Stardust";

		/// <summary>
		/// Asset folder for data files storage.
		/// </summary>
		public const string DataFileFolder = "Data";
		public const string DataFileExtension = "dat";
		public const string SceneFileFolder = "Scenes";
		public const string SceneFileExtension = "unity";
	} 
#endif
}