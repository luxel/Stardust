namespace Stardust
{
	using System.Collections;
	using System.IO;
	using ProtoBuf;
	/// <summary>
	/// Formatting objects binary formatter using ProtoBuf
	/// </summary>
	public class ProtoBufFormatter : IObjectFormatter
	{
		public void Serialize<T>(Stream stream, T instance)
		{
			ProtoBuf.Serializer.Serialize(stream, instance);
		}
		public T Deserialize<T>(Stream stream)
		{
            return ProtoBuf.Serializer.Deserialize<T>(stream);
		}
		public T DeserializeInto<T>(Stream stream, T instance)
		{
			return ProtoBuf.Serializer.Merge<T>(stream, instance);
		}
	}
}
