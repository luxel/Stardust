namespace Stardust
{
	using UnityEngine;
	using System.IO;

	/// <summary>
	/// Formatting object for json format using UnityEngine.JsonUtility
	/// </summary>
	public class JsonFormatter : IObjectFormatter
	{
		public void Serialize<T>(Stream stream, T instance)
		{
            StreamWriter writer = new StreamWriter(stream);			
			writer.Write(JsonUtility.ToJson(instance, true));
            writer.Flush();
		}
		public T Deserialize<T>(Stream stream)
		{
			string jsonString;
            StreamReader reader = new StreamReader(stream);
		    jsonString = reader.ReadToEnd();
			return JsonUtility.FromJson<T>(jsonString);
		}
		public T DeserializeInto<T>(Stream stream, T instance)
		{
			string jsonString;
            StreamReader reader = new StreamReader(stream);
			jsonString = reader.ReadToEnd();
			JsonUtility.FromJsonOverwrite(jsonString, instance);
			return instance;
		}
	}
}
