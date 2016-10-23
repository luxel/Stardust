namespace Stardust
{
    using System.Collections;
    using System.IO;

    public static class FileUtility
    {
        /// <summary>
        /// Deletes all files and directories under the specified path, but won't delete the path itself.
        /// </summary>
        public static void ClearDirectory(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }

		/// <summary>
		/// Gets a file name without its extension
		/// </summary>
		public static string GetFileNameWithoutExtension(string filename)
		{
			int index = filename.IndexOf('.');
			if (index > 0)
			{
				return filename.Substring(0, index);
			}
			else
			{
				return filename;
			}
		}
		/// <summary>
		/// Checks the specified path and determines whether it's a directory
		/// </summary>
		public static bool IsDirectory(string path)
		{
			if (!File.Exists(path)) return false;
			FileAttributes attr = File.GetAttributes(path);

			if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
    }
}