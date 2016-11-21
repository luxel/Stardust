namespace Stardust
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;

	/// <summary>
	/// An ObjectDB is an in-memory database on which we can do simple find/add/remove operations.
	/// It can also saves the data into a local file and loads from the same file.
	/// </summary>
	public interface IObjectDb<IdType, ObjectType> where ObjectType: IObjectWithId<IdType>
	{
		/// <summary>
		/// Name of the DB. This should be unique and it will be used to store the file.
		/// </summary>
		string Name { get; }
		/// <summary>
		/// Count of all items.
		/// </summary>
		int Count { get; }
		/// <summary>
		/// Gets an item at specified index.
		/// </summary>
		ObjectType Get(int index);
		/// <summary>
		/// Finds an item with specified id.
		/// </summary>
		ObjectType Find(IdType id);
		/// <summary>
		/// Adds a new item into the DB.
		/// </summary>
		ObjectType Add(ObjectType item);
		/// <summary>
		/// Removes an item with specified id.
		/// </summary>
		ObjectType Remove(IdType id);
        /// <summary>
        /// Removes an item
        /// </summary>
        ObjectType Remove(ObjectType item);
		/// <summary>
		/// Clears all the data in the DB.
		/// </summary>
		void Clear();
		/// <summary>
		/// Loads the DB from another collection
		/// </summary>
		/// <param name="stream">Stream.</param>
		void LoadFrom(ICollection<ObjectType> collection);
	}
    /// <summary>
	/// And file based object db which persists data to files.
	/// </summary>
	public interface IObjectFileDb<IdType, ObjectType> : IObjectDb<IdType, ObjectType> where ObjectType : IObjectWithId<IdType>
    {
        /// <summary>
        /// Saves the data to a file.
        /// </summary>
        void Save();
        /// <summary>
        /// Loads the data from a file.
        /// </summary>
        void Load();
    }
}