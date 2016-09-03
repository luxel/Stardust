namespace Stardust.Utilities
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
    }
}