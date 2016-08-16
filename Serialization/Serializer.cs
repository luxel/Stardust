namespace Stardust.Serialization
{	
	using System.IO;
	/// <summary>
	/// The serializer providing useful methods for serailizations (with default formatter)
	/// </summary>
	public static class Serializer 
	{
		private static IObjectFormatter _Default;
		public static IObjectFormatter Default 
		{
			get 
			{
				if (_Default == null)
				{
					#if STARDUST_SERIALIZER_DEFAULT_JSON
					_Default = new JsonFormatter();
					#else
					_Default = new ProtoBufFormatter();
					#endif
				}
				return _Default;
			}
		}

		/// <summary>
		/// Serialize the specified instance and writes into the specified stream.
		/// </summary>
		public static void Serialize<T>(Stream stream, T instance)
		{
			Default.Serialize<T>(stream, instance);
		}
		/// <summary>
		/// Deserialize the data from the specified stream and creates the instance
		/// </summary>
		public static T Deserialize<T>(Stream stream)
		{
			return Default.Deserialize<T>(stream);
		}
		/// <summary>
		/// Deserialize the data from the specified stream and overwrites the existing instance.
		/// </summary>
		public static T DeserializeInto<T>(Stream stream, T instanceToOverwrite)
		{
			return DeserializeInto<T>(stream, instanceToOverwrite);
		}
	}
}