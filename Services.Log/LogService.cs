namespace Stardust.Services.Log
{
	using System;
	using UnityEngine;

	/// <summary>
	/// Default logger service which logs the message through Unity Debug.Log API.
	/// </summary>
	public class LogService : GameServiceBase, ILogService
	{	 
		protected enum LogLevel
		{
			Debug,
			Info,
			Warning,
			Error
		}

		public void Debug(string text)
		{
			LogByLevel(LogLevel.Debug, text);
		}
		public void Debug(string format, params object[] args)
		{
			LogByLevel(LogLevel.Debug, format, args);
		}
		
		public void Info(string text)
		{
			LogByLevel(LogLevel.Info, text);
		}
		public void Info(string format, params object[] args)
		{
			LogByLevel(LogLevel.Info, format, args);
		}
		public void Warning(string text)
		{
			LogByLevel(LogLevel.Warning, text);
		}
		public void Warning(string format, params object[] args)
		{
			LogByLevel(LogLevel.Warning, format, args);
		}
		public void Error(string text)
		{
			LogByLevel(LogLevel.Error, text);
		}
		public void Error(string format, params object[] args)
		{
			LogByLevel(LogLevel.Error, format, args);
		}
		public void Exception(Exception e)
		{
			LogByLevel(LogLevel.Error, e, null);			
		}
		public void Exception(Exception e, string text)
		{
			LogByLevel(LogLevel.Error, e, text);
		}
		public void Exception(Exception e, string format, params object[] args)
		{
			LogByLevel(LogLevel.Error, e, format, args);
		}

		protected override void OnStartup ()
		{
			// Nothing to do here.
		}

		protected virtual void LogByLevel(LogLevel level, string format, params object[] args)
		{
			if (!IsLogLevelEnabled(level))
			{
				return;
			}
			LogByLevelWithoutChecking(level, string.Format(format, args), null);
		}

		protected virtual void LogByLevel(LogLevel level, string text)
		{
			if (!IsLogLevelEnabled(level))
			{
				return;
			}
			LogByLevelWithoutChecking(level, text, null);

		}

		protected virtual void LogByLevel(LogLevel level, Exception e, string format, params object[] args)
		{
			if (!IsLogLevelEnabled(level))
			{
				return;
			}
			LogByLevelWithoutChecking(level, string.Format(format, args), e);
		}

		protected virtual void LogByLevel(LogLevel level, Exception e, string text)
		{
			if (!IsLogLevelEnabled(level))
			{
				return;
			}
			LogByLevelWithoutChecking(level, text, e);
		}

		protected virtual void LogByLevelWithoutChecking(LogLevel level, string text, Exception e)
		{
			if (text != null)
			{
				switch (level) {
				case LogLevel.Debug:
				case LogLevel.Info:
					UnityEngine.Debug.Log(text);
					break;
				case LogLevel.Warning:
					UnityEngine.Debug.LogWarning(text);
					break;
				case LogLevel.Error:
					UnityEngine.Debug.LogError(text);
					break;
				default:
					break;
				}
			}
			if (e != null)
			{
				UnityEngine.Debug.LogException(e);
			}
		}

		protected virtual bool IsLogLevelEnabled(LogLevel level)
		{
			bool enabled = false;
			#if UNITY_EDITOR
			enabled = true;
			#else
			if (level == LogLevel.Error)
			{
				enabled = true;
			}
			#endif
			return enabled;
		}
	}
}