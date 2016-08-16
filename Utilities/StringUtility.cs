namespace Stardust
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Text;

	/// <summary>
	/// Provides utility methods for strings.
	/// </summary>
	public static class StringUtility
	{
		public static int GetBytesLength(this string str)
		{
			return System.Text.Encoding.Unicode.GetBytes(str).Length;
		}

		/// <summary>
		/// Generates an MD5 hash for an input string
		/// </summary>
		/// <returns>The md5sum in string.</returns>
		/// <param name="strToEncrypt">String to encrypt.</param>
		public static string Md5Sum(this string strToEncrypt)
		{
			StringBuilder _md5sumBuilder = new StringBuilder();
			System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
			byte[] bytes = ue.GetBytes(strToEncrypt);

			// encrypt bytes
			System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
			byte[] hashBytes = md5.ComputeHash(bytes);

			// Convert the encrypted bytes back to a string (base 16)
			_md5sumBuilder.Length = 0;

			for (int i = 0; i < hashBytes.Length; i++)
			{
				string str = System.Convert.ToString(hashBytes[i], 16);
				if (str.Length == 1) {
					_md5sumBuilder.Append('0').Append(str);
				}
				else if (str.Length == 2) {
					_md5sumBuilder.Append(str);
				}
				else {
					_md5sumBuilder.Append('0').Append('0');
				}
			}

			return _md5sumBuilder.ToString().PadLeft(32, '0');
		}

		/// <summary>
		/// Encodes string into an URL-friendly format, with upper case letters for the hexadecimal code.
		/// </summary>
		/// <returns>A new string with all illegal characters replaced with %xx where xx is the hexadecimal code for the character code.</returns>
		/// <param name="s">A string to be escaped.</param>
		public static string UpperCaseUrlEncode(this string s)
		{
			char[] temp = UnityEngine.WWW.EscapeURL(s).ToCharArray();

			for (int i = 0; i < temp.Length - 2; i++)
			{
				if (temp[i] == '%')
				{
					temp[i + 1] = char.ToUpper(temp[i + 1]);
					temp[i + 2] = char.ToUpper(temp[i + 2]);
				}
			}
			return new string(temp);
		}

		/// <summary>
		/// Determines if the given char is a number.
		/// </summary>
		public static bool IsNumber (char c)
		{
			return c >= '0' && c <= '9';
		}
	}
}