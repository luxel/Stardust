namespace Stardust
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// File based object db which loads data from streamingAssetsPath and saving data to data path, using binary serialization.
    /// </summary>
    public class ObjectBinaryFileDb<IdType, ObjectType> : ObjectFileDb<IdType, ObjectType> where ObjectType : IObjectWithId<IdType>
    {
        public ObjectBinaryFileDb(string name)
            : this(name, Folders.Database)
        {
        }

        public ObjectBinaryFileDb(string name, string folderName)
            : base(name, folderName, Serializer.BinaryFormatter)
        {
        }

        protected override void SaveToStream(Stream stream)
        {
            base.SaveToStream(stream);
            // Ensuer file truncate. (see http://stackoverflow.com/q/2152978/23354)
            stream.SetLength(stream.Position);
        }

    }
    /// <summary>
    /// File based object db which loads data from streamingAssetsPath and saving data to data path, using json serialization.
    /// </summary>
    public class ObjectTextFileDb<IdType, ObjectType> : ObjectFileDb<IdType, ObjectType> where ObjectType : IObjectWithId<IdType>
    {
        /// <summary>
        /// Right now unity doesn't support direct json serialization on collections, so we have to wrap the list into an object.
        /// </summary>
        private JsonListWrapper<ObjectType> wrapper;

        public ObjectTextFileDb(string name)
            : this(name, Folders.Database)
        {
        }

        public ObjectTextFileDb(string name, string folderName)
            : base(name, folderName, Serializer.TextFormatter)
        {
            wrapper = new JsonListWrapper<ObjectType>();
            wrapper.list = base.List;
        }

        protected override void LoadFromStream(Stream stream)
        {
            Formatter.DeserializeInto<JsonListWrapper<ObjectType>>(stream, wrapper);
        }

        protected override void SaveToStream(Stream stream)
        {
            Formatter.Serialize<JsonListWrapper<ObjectType>>(stream, wrapper);
            // Ensure file truncates.
            stream.SetLength(stream.Position);
        }
    }

    /// <summary>
    /// File based object db which loads data from streamingAssetsPath and saving data to data path.
    /// </summary>
    /// <typeparam name="IdType"></typeparam>
    /// <typeparam name="ObjectType"></typeparam>
    public class ObjectFileDb<IdType, ObjectType> : ObjectDd<IdType, ObjectType>, IObjectFileDb<IdType, ObjectType> where ObjectType : IObjectWithId<IdType>
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

#if UNITY_EDITOR
        protected bool saveToPackageDataPathUnderEditor = true;
#endif

        internal ObjectFileDb(string name, IObjectFormatter formatter)
            : this(name, Folders.Database, formatter)
        {
        }

        internal ObjectFileDb(string name, string folderName, IObjectFormatter formatter)
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
            if (saveToPackageDataPathUnderEditor)
            {
                // When under editor, the content should be directly saved to package data path.
                filePathToSave = ReadonlyFilePath;
            }
#endif
            var info = Directory.GetParent(filePathToSave);
            if (!info.Exists)
            {
#if UNITY_EDITOR
                Debug.LogFormat("Creating directory {0}", info.FullName);
#endif
                info.Create();
            }

            string filePathToWrite = filePathToSave;

            bool useTempFile = false;            
            if (File.Exists(FilePath))
            {
                filePathToWrite = TempFilePath;
                useTempFile = true;
            }

            using (FileStream stream = File.OpenWrite(filePathToWrite))
            {
                SaveToStream(stream);
            }            

            if (useTempFile)
            {
                File.Copy(filePathToWrite, filePathToSave, true);
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