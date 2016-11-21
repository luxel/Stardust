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
        /// Gets the current unix timestamp in milliseconds.
        /// </summary>
		/// <returns>The current unix timestamp milliseconds.</returns>
        public static int GetCurrentUnixTimestampMs()
        {
            return DateTime.Now.ToUnixTimestampMs();
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
		/// Converts a DateTime object to unix timestamp (in milliseconds.)
		/// </summary>
		/// <returns>The unix timestamp.</returns>
		public static int ToUnixTimestampMs(this DateTime dt)
        {
            return (int)((dt.ToUniversalTime().Ticks - UniversalTimeEpochTicks) / 10000);
        }

        /// <summary>
        /// Converts a unix timestamp (seconds) to local date time.
        /// </summary>
        /// <returns>The DateTime object.</returns>
        public static DateTime UnixTimestampToDateTime(int time)
		{
			return DateTimeEpoch.AddSeconds(time).ToLocalTime();
		}

        /// <summary>
        /// Whether the two DateTime represents the same day.
        /// </summary>
        public static bool IsSameDay(DateTime dt1, DateTime dt2)
        {
            return dt1.Year == dt2.Year && dt1.Month == dt2.Month && dt1.Day == dt2.Day;
        }
	}
}