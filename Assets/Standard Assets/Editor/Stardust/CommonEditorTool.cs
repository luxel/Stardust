#if UNITY_EDITOR
namespace Stardust.Editor
{
    using UnityEngine;
    using UnityEditor;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;  

    public abstract class CommonEditorTool
    {
        private const string CommonLogFormat = "[{0}] {1}";
        protected string LogName { get; private set; }
        protected string FatalError { get; private set; }

        protected bool autoTriggerError = true;

        public CommonEditorTool()
        {
            LogName = GetLogName();
        }

        #region Error handling

        protected void OnFatalError(string message, bool triggerErrorImmediately = false)
        {
            FatalError = message;
            if (triggerErrorImmediately || autoTriggerError)
            {
                CheckError();
            }
        }

        /// <summary>
        /// Checks for fatal errors, if error, throw exception to stop the current process. Otherwise, continue the build.
        /// </summary>
        protected void CheckError()
        {
            if (!string.IsNullOrEmpty(FatalError))
            {
                throw new Exception(string.Format("[{0}] Build stopped due to fatal error: {1}", LogName, FatalError));
            }
        }

        /// <summary>
        /// Checks the existence of target file and reports an error if not found.
        /// </summary>
        /// <param name="filePath">Full path to the file.</param>
        protected void CheckFileExistence(string filePath)
        {
            if (!File.Exists(filePath))
            {
                OnFatalError(string.Format("File not exists: {0}", filePath));
            }
        }

        #endregion

        #region Logging
        protected virtual string GetLogName()
        {
            return this.GetType().Name;
        }

        protected void Log(string message)
        {
            Debug.Log(GetCommonLog(message));
        }

        protected void Log(string format, params object[] args)
        {
            Debug.Log(GetCommonLog(format, args));
        }

        protected void LogWarning(string message)
        {
            Debug.LogWarning(GetCommonLog(message));
        }

        protected void LogWarning(string format, params object[] args)
        {
            Debug.LogWarning(GetCommonLog(format, args));
        }

        protected void LogError(string message)
        {
            Debug.LogError(GetCommonLog(message));
        }

        protected void LogError(string format, params object[] args)
        {
            Debug.LogError(GetCommonLog(format, args));
        }

        private string GetCommonLog(string message)
        {
            return string.Format(CommonLogFormat, LogName, message);
        }

        private string GetCommonLog(string format, params object[] args)
        {
            return GetCommonLog(string.Format(format, args));
        }
        #endregion

    }
} 
#endif