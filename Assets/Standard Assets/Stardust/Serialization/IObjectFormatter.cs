namespace Stardust.Serialization
{
	using System;
	using System.IO;
	/// <summary>
	/// Interface for formatter which are responsible for object serailizations.
	/// </summary>
	public interface IObjectFormatter
	{
		/// <summary>
		/// Serialize the specified instance and writes into the specified stream.
		/// </summary>
		void Serialize<T>(Stream stream, T instance);
		/// <summary>
		/// Deserialize the data from the specified stream and creates the instance
		/// </summary>
		T Deserialize<T>(Stream stream);
		/// <summary>
		/// Deserialize the data from the specified stream and overwrites the existing instance.
		/// </summary>
		T DeserializeInto<T>(Stream stream, T instanceToOverwrite);
	}
}

