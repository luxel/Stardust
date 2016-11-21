namespace Stardust
{
    using UnityEngine;
    using System;
    using System.Collections;

    /// <summary>
    /// The build version info for the game.
    /// </summary>
    [Serializable]
    public class VersionInfo : IObjectWithId<string>
    {
        /// <summary>
        /// The complete version string
        /// </summary>
        public string Version;

        /// <summary>
        /// The format of the complete version. If the major or minor version needs to be updated, update them here. {0} is the build versioin.
        /// </summary>
        public string VersionFormat = "0.0.{0}";

        /// <summary>
        /// Identifier of the version info.
        /// </summary>
        public string Name;

        /// <summary>
        /// Current build version value.
        /// </summary>
        public int BuildVersion = 0;
        /// <summary>
        /// The time of the build in unix timestamp format.
        /// </summary>
        public int BuildTimestamp = 0;
        /// <summary>
        /// Gets the build time in DateTime format
        /// </summary>
        public DateTime BuildTime
        {
            get
            {
                return DateTimeUtility.UnixTimestampToDateTime(BuildTimestamp);
            }
        }

        public string Id
        {
            get
            {
                return Name;
            }
        }
    }
}