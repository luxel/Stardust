namespace Stardust
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using Stardust;

	/// <summary>
	/// Base class for ObjectDB.
	/// </summary>
	public class ObjectDd<IdType, ObjectType> : IObjectDb<IdType, ObjectType> where ObjectType:IObjectWithId<IdType>
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
		/// The collection object which holds the data.
		/// </summary>
		protected IDictionary<IdType, ObjectType> Data { get; private set; }
		protected List<ObjectType> List { get; private set; }

		public ObjectDd(string name) 
		{
            Name = name;
            Data = CreateDictionary();
            List = new List<ObjectType>();
        }

		public virtual ObjectType Get(int index)
		{
			ObjectType item = default(ObjectType);
			if (index >= 0 && index < List.Count)
			{
				item = List[index];
			}			
			return item;
		}

		public virtual ObjectType Find(IdType id)
		{
			ObjectType item = default(ObjectType);

			if (Data.ContainsKey(id))
			{
				item = Data[id];
			}
			return item;
		}
		public virtual ObjectType Add(ObjectType item)
		{
			List.Add(item);
			Data[item.Id] = item;
			return item;
		}

		public virtual ObjectType Remove(IdType id)
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

        public virtual ObjectType Remove(ObjectType item)
        {
            return Remove(item.Id);
        }

		public void Clear()
		{
			Data.Clear();
			List.Clear();
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

            Data.Clear();
			for (int i = 0, count = List.Count; i < count; i ++)
			{
				ObjectType item = List[i];
				Data[item.Id] = item;
			}
        }
        /// <summary>
        /// Creates the dictionary object for data storage.
        /// </summary>
        protected virtual IDictionary<IdType, ObjectType> CreateDictionary()
        {
            return new Dictionary<IdType, ObjectType>();
        }
    }
}