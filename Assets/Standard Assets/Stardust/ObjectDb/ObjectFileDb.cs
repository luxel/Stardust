namespace Stardust
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// File based object db which loads data from streamingAssetsPath and saving data to data path.
    /// </summary>
    /// <typeparam name="IdType"></typeparam>
    /// <typeparam name="ObjectType"></typeparam>
    public class ObjectFileDb<IdType, ObjectType> : ObjectDd<IdType, ObjectType> where ObjectType : IObjectWithId<IdType>
    {
        public string ReadonlyFilePath { get; private set; }
        /// <summary>
        /// Full path to the db file.
        /// </summary>
        public string FilePath { get; private set; }
        /// <summary>
        /// Full path to the temp db file.
        /// </summary>
        protected string TempFilePath { get; private set; }
        /// <summary>
        /// Name of the parent folder.
        /// </summary>
        protected string ParentFolderName { get; private set; }

        protected IObjectFormatter Formatter { get; private set; }        

        public ObjectFileDb(string name, IObjectFormatter formatter)
            : this(name, Folders.Database, formatter)
        {
        }

        public ObjectFileDb(string name, string folderName, IObjectFormatter formatter)
            : base(name)
        {
            ParentFolderName = folderName;
            ReadonlyFilePath = GetFilePath(GameEnvironment.PackageDataPath);
            FilePath = GetFilePath(GameEnvironment.DataPath);
            TempFilePath = GetTempFilePath();
            this.Formatter = formatter;

        }

        /// <summary>
        /// Saves the DB to the file system.
        /// </summary>
        public void Save()
        {
            string filePathToSave = FilePath;
#if UNITY_EDITOR
            // When under editor, the content should be directly saved to package data path.
            filePathToSave = ReadonlyFilePath;
#endif
            bool useTempFile = false;
            if (File.Exists(FilePath))
            {
                filePathToSave = TempFilePath;
                useTempFile = true;
            }

            using (FileStream stream = File.OpenWrite(filePathToSave))
            {
                SaveToStream(stream);
            }

            if (useTempFile)
            {
                File.Copy(TempFilePath, filePathToSave, true);
            }
        }

        /// <summary>
        /// Loads the DB from file.
        /// </summary>
        public void Load()
        {
            Clear();

            string filePathToLoad = FilePath;
            if (!File.Exists(filePathToLoad))
            {
                filePathToLoad = ReadonlyFilePath;
            }
            if (File.Exists(filePathToLoad))
            {
                using (FileStream stream = File.OpenRead(filePathToLoad))
                {
                    LoadFromStream(stream);
                }
            }
            PrepareDataCollections();
        }
        /// <summary>
        /// Builds the full path to the DB storage file.
        /// </summary>
        protected virtual string GetFilePath(string parentPath)
        {
            string folderPath = string.IsNullOrEmpty(ParentFolderName) ? parentPath : Path.Combine(parentPath, ParentFolderName);
            return Path.Combine(folderPath, Name);
        }
        
        /// <summary>
        /// Builds the full path to the DB storage temp file.
        /// </summary>
        protected virtual string GetTempFilePath()
        {
            return FilePath + ".tmp";
        }
        /// <summary>
        /// Loads the data from a binary stream. Child class should handle the deserialization here.
        /// </summary>
        protected virtual void LoadFromStream(Stream stream)
        {            
            Formatter.DeserializeInto<List<ObjectType>>(stream, List);
        }
        /// <summary>
        /// Saves the data into a binary stream. Child class should handle the serialization here.
        /// </summary>
        protected virtual void SaveToStream(Stream stream)
        {
            Formatter.Serialize<List<ObjectType>>(stream, List);
        }
    }
}