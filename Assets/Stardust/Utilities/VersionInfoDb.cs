namespace Stardust
{
    using UnityEngine;
    using System.Collections;
    using System;
    using System.IO;

    public class VersionInfoDb : ObjectTextFileDb<string, VersionInfo>
    {
        private static readonly string CurrentVersionName = "current";
                
        /// <summary>
        /// Current version info.
        /// </summary>
        public VersionInfo Current { get; private set; }

        public VersionInfoDb() :base("version")
        {
        }

#if UNITY_EDITOR
        /// <summary>
        /// Used by player builder. Gets the build version. Increases verison number if needed.
        /// </summary>
        public VersionInfo GetBuildVersion()
        {
            bool increaseVersion = false;

            if (Current.BuildVersion == 0)
            {
                increaseVersion = true;
            }
            else
            {
                if (!DateTimeUtility.IsSameDay(DateTime.Now, Current.BuildTime))
                {
                    increaseVersion = true;
                }
            }

            if (increaseVersion)
            {
                // Daily build version increasing strategy.
                Current.BuildVersion++;
                Current.BuildTimestamp = DateTimeUtility.GetCurrentUnixTimestamp();
                Current.Version = string.Format(Current.VersionFormat, Current.BuildVersion);
                Save();
                Debug.LogFormat("Version increased, new version is {0}", Current.Version);
            }

            return Current;
        }

        public void UpdateBuildTime(int timestamp)
        {
            if (Current != null)
            {
                Current.BuildTimestamp = timestamp;
                Save();
            }
        }
#endif

        protected override void PrepareDataCollections()
        {
            base.PrepareDataCollections();

            Current = base.Find(CurrentVersionName);

            if (Current == null)
            {
                Current = new VersionInfo() { Name = CurrentVersionName };
                Add(Current);
            }
        }
    }
}