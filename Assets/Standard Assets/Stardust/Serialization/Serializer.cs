namespace Stardust
{	
	using System.IO;
	/// <summary>
	/// The serializer providing useful methods for serailizations (with default formatter)
	/// </summary>
	public static class Serializer 
	{
		private static IObjectFormatter _Default;
        /// <summary>
        /// Gets the default formatter. Usually this is a binary formatter.
        /// </summary>
		public static IObjectFormatter Default 
		{
			get 
			{
				if (_Default == null)
				{
#if STARDUST_SERIALIZER_DEFAULT_JSON
					_Default = TextFormatter;
#else
                    _Default = BinaryFormatter;
#endif
                }
				return _Default;
			}
		}

        private static IObjectFormatter _TextFormatter;
        /// <summary>
        /// Gets the default text based formatter.
        /// </summary>
        public static IObjectFormatter TextFormatter
        {
            get
            {
                if (_TextFormatter == null)
                {
                    _TextFormatter = new JsonFormatter();
                }
                return _TextFormatter;
            }
        }

        private static IObjectFormatter _BinaryFormatter;
        /// <summary>
        /// Gets the default binary based formatter.
        /// </summary>
        public static IObjectFormatter BinaryFormatter
        {
            get
            {
                if (_BinaryFormatter == null)
                {
                    _BinaryFormatter = new ProtoBufFormatter();
                }
                return _BinaryFormatter;
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