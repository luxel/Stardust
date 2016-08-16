namespace Stardust.Utilities.ObjectDb
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using Stardust.Serialization;

	/// <summary>
	/// Base class for ObjectDB.
	/// </summary>
	public abstract class ObjectDBBase<IdType, ObjectType> : IObjectDb<IdType, ObjectType> where ObjectType:IObjectWithId<IdType>
	{
		/// <summary>
		/// Name of the DB. This should be unique and it will be used to store the file.
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// Count of all items.
		/// </summary>
		public int Count { get { return List.Count; } }
		/// <summary>
		/// Full path to the db file.
		/// </summary>
		public string FilePath { get; private set; }
		/// <summary>
		/// Full path to the temp db file.
		/// </summary>
		protected string TempFilePath { get; private set; }
		/// <summary>
		/// The collection object which holds the data.
		/// </summary>
		protected IDictionary<IdType, ObjectType> Data { get; private set; }
		protected IList<ObjectType> List { get; private set; }

		public ObjectDBBase (string name)
		{
			Name = name;
			Data = CreateDictionary();
			List = CreateList();
			FilePath = GetFilePath();
			TempFilePath = GetTempFilePath();
		}

		public ObjectType Get(int index)
		{
			ObjectType item = default(ObjectType);
			if (index >= 0 && index < List.Count)
			{
				item = List[index];
			}			
			return item;
		}

		public ObjectType Find(IdType id)
		{
			ObjectType item = default(ObjectType);

			if (Data.ContainsKey(id))
			{
				item = Data[id];
			}
			return item;
		}
		public ObjectType Add(ObjectType item)
		{
			List.Add(item);
			Data[item.Id] = item;
			return item;
		}
		public ObjectType Remove(IdType id)
		{
			ObjectType item = default(ObjectType);
			if (Data.ContainsKey(id))
			{
				item = Data[id];
				List.Remove(item);
				Data.Remove(id);
			}
			return item;
		}

		public void Clear()
		{
			Data.Clear();
			List.Clear();
		}

		/// <summary>
		/// Saves the DB to the file system.
		/// </summary>
		public void Save() 
		{
			if (File.Exists(FilePath))
			{
				using (FileStream stream = File.OpenWrite(TempFilePath))
				{
					SaveToStream(stream);
				}
			}

			File.Copy(TempFilePath, FilePath, true);
		}

		/// <summary>
		/// Loads the DB from file.
		/// </summary>
		public void Load() 
		{
			Clear();

			if (File.Exists(FilePath))
			{
				using (FileStream stream = File.OpenRead(FilePath))
				{
					LoadFromStream(stream);
					PrepareDataCollections();
				}
			}
		}

		public void LoadFrom(ICollection<ObjectType> collection)
		{
			if (collection != null)
			{
				Clear();

				foreach (ObjectType item in collection)
				{
					List.Add(item);	
				}
				PrepareDataCollections();
			}
		}

		/// <summary>
		/// Builds the full path to the DB storage file.
		/// </summary>
		protected virtual string GetFilePath()
		{
			return Path.Combine(GameEnvironment.DatabasePath, Name);
		}
		/// <summary>
		/// Builds the full path to the DB storage temp file.
		/// </summary>
		protected virtual string GetTempFilePath()
		{
			return FilePath + ".tmp";
		}
		/// <summary>
		/// Creates the list object for data storage. Be default List<ObjectType> is used.
		/// </summary>
		/// <returns>The list.</returns>
		protected virtual IList<ObjectType> CreateList()
		{
			return new List<ObjectType>();
		}
		protected virtual void PrepareDataCollections()
		{
			// Use list data to fill dictionary
			if (Data.Count == 0 && List.Count > 0)
			{
				for (int i = 0, count = List.Count; i < count; i ++)
				{
					ObjectType item = List[i];
					Data[item.Id] = item;
				}
			}
			else
			{
				// this is not implemented yet.
				throw new InvalidOperationException();
			}
		}
		/// <summary>
		/// Creates the dictionary object for data storage.
		/// </summary>
		protected abstract IDictionary<IdType, ObjectType> CreateDictionary();
		/// <summary>
		/// Loads the data from a binary stream. Child class should handle the deserialization here.
		/// </summary>
		protected abstract void LoadFromStream(Stream stream);
		/// <summary>
		/// Saves the data into a binary stream. Child class should handle the serialization here.
		/// </summary>
		protected abstract void SaveToStream(Stream stream);
	}
}