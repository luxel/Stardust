namespace Stardust.Services
{
	using System;
	/// <summary>
	/// Log service - used to write game logs.
	/// </summary>
	public interface ILogService : IGameService
	{
		/// <summary>
		/// Logs a debug message.
		/// </summary>
		void Debug(string text);
		/// <summary>
		/// Logs a debug message with format.
		/// </summary>
		void Debug(string format, params object[] args);
		/// <summary>
		/// Logs an info message.
		/// </summary>
		void Info(string text);
		/// <summary>
		/// Logs an info message with format.
		/// </summary>
		void Info(string format, params object[] args);
		/// <summary>
		/// Logs a warning message.
		/// </summary>
		void Warning(string text);
		/// <summary>
		/// Logs a warining message with format.
		/// </summary>
		void Warning(string format, params object[] args);
		/// <summary>
		/// Logs an error message.
		/// </summary>
		void Error(string text);
		/// <summary>
		/// Logs an error message with format.
		/// </summary>
		void Error(string format, params object[] args);
		/// <summary>
		/// Logs an exception.
		/// </summary>
		void Exception(Exception e);
		/// <summary>
		/// Logs an exception with a message.
		/// </summary>
		void Exception(Exception e, string text);
		/// <summary>
		/// Logs an exception with a formatted message.
		/// </summary>
		void Exception(Exception e, string format, params object[] args);
	}
}