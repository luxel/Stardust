namespace Stardust
{
    using UnityEngine;
    using System.Collections;

    public interface IHttpChannel
    {
        /// <summary>
        /// Initialize a channel with specified settings.
        /// </summary>
        void Initialize(DataServiceSettings settings);
        /// <summary>
        /// Update loop.
        /// </summary>
        void Update();
        /// <summary>
        /// Whether the channel is available to accept new requests.
        /// </summary>
        bool AcceptsNewRequest { get; }
        /// <summary>
        /// Makes a request.
        /// </summary>
        void MakeRequest(DataRequestContainer container);
        /// <summary>
        /// Shutdown the channel.
        /// </summary>
        void Shutdown();
    }
}