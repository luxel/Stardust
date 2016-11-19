namespace Stardust
{
	using System;

	/// <summary>
	/// Provides utility methods for datetime
	/// </summary>
	public static class DateTimeUtility
	{
		/// <summary>
		/// Ticks for 1970/1/1 0:0:0 UTC time.
		/// </summary>
		private const long UniversalTimeEpochTicks = 621355968000000000;
		/// <summary>
		/// The static date time object for 1970/1/1 0:0:0 UTC time.
		/// </summary>
		private static readonly DateTime DateTimeEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		/// <summary>
		/// Gets the current unix timestamp in seconds.
		/// </summary>
		/// <returns>The current unix timestamp seconds.</returns>
		public static int GetCurrentUnixTimestamp()
		{
			return DateTime.Now.ToUnixTimestamp();
		}

		/// <summary>
		/// Converts a DateTime object to unix timestamp (in seconds.)
		/// </summary>
		/// <returns>The unix timestamp.</returns>
		public static int ToUnixTimestamp(this DateTime dt)
		{
			return (int)((dt.ToUniversalTime().Ticks - UniversalTimeEpochTicks) / 10000000);
		}

		/// <summary>
		/// Converts a unix timestamp to local date time.
		/// </summary>
		/// <returns>The DateTime object.</returns>
		public static DateTime UnixTimestampToDateTime(int time)
		{
			return DateTimeEpoch.AddSeconds(time).ToLocalTime();
		}
	}
}