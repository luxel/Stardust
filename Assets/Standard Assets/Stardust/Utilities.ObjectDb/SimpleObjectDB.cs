namespace Stardust.Utilities.ObjectDb
{
	using System.Collections.Generic;
	using System.IO;
	using Stardust.Serialization;

	public sealed class SimpleObjectDB<IdType, ObjectType> : ObjectDBBase<IdType, ObjectType> where ObjectType:IObjectWithId<IdType>
	{
		private Dictionary<IdType, ObjectType> dataCollection;
		private List<ObjectType> listCollection;

		public SimpleObjectDB (string name) : base(name)
		{
			dataCollection = Data as Dictionary<IdType, ObjectType>;
			listCollection = List as List<ObjectType>;
		}

		protected override IDictionary<IdType, ObjectType> CreateDictionary()
		{
			return new Dictionary<IdType, ObjectType>();
		}

		protected override void LoadFromStream(Stream stream)
		{
			Serializer.DeserializeInto<List<ObjectType>>(stream, listCollection);
		}

		protected override void SaveToStream(Stream stream)
		{
			Serializer.Serialize<List<ObjectType>>(stream, listCollection);
		}
	}
}