namespace Stardust
{
    using UnityEngine;
    using System.Collections;

    /// <summary>
    /// Settings for the data service.
    /// </summary>
    public class DataServiceSettings
    {
        public const string DefaultServerUrl = @"http://localhost";
        public const int DefaultRequestTimeOut = 2 * 1000;

        public static DataServiceSettings Default = new DataServiceSettings() { ServerUrl = DefaultServerUrl, RequestTimeOut = DefaultRequestTimeOut };

        public static DataServiceSettings Current { get; internal set; }

        /// <summary>
        /// Url of the server.
        /// </summary>
        public string ServerUrl;

        /// <summary>
        /// An Id for the client which is allocated by server.
        /// </summary>
        public string ClientId;

        /// <summary>
        /// Number of ms how long the reuqest will be considered time out.
        /// </summary>
        public int RequestTimeOut;

        /// <summary>
        /// How many channels is running for concurrent requests.
        /// </summary>
        public int ChannelCount = 2;
    }
}