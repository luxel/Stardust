namespace Stardust.Utilities
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Caches strings to minimize memory allocations caused by string contact.
	/// </summary>
	public sealed class StringCache
	{
		private Dictionary<string, Dictionary<string, string>> _Cache = new Dictionary<string, Dictionary<string, string>>();

		public string GetFormatted(string format, string str1, string str2)
		{
			Dictionary<string, string> dict = null;
			if (!_Cache.ContainsKey(str1))
			{
				dict = new Dictionary<string, string>();
				_Cache[str1] = dict;
			}
			else
			{
				dict = _Cache[str1];
			}

			if (dict.ContainsKey(str2))
			{
				return dict[str2];
			}
			else
			{
				string value = string.Format(format, str1, str2);
				dict[str2] = value;
				return value;
			}
		}

		/// <summary>
		/// Gets a contacted string for give strings, from the static string cache, to avoid memory allocations.
		/// </summary>
		public string GetContacted(string str1, string str2)
		{
			Dictionary<string, string> dict = null;
			if (!_Cache.ContainsKey(str1))
			{
				dict = new Dictionary<string, string>();
				_Cache[str1] = dict;
			}
			else
			{
				dict = _Cache[str1];
			}

			if (dict.ContainsKey(str2))
			{
				return dict[str2];
			}
			else
			{
				string value = str1 + str2;
				dict[str2] = value;
				return value;
			}
		}
	}
}