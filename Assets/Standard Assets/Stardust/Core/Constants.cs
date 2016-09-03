namespace Stardust
{
    public static class Constants
    {
        public const string EditorMenuPrefix = "Stardust";
    }
	public static class ServiceNames
	{
		public const string LogService = "ILogService";
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
    }
#endif
}