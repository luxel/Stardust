namespace Stardust
{
    using UnityEngine;
    using UnityEngine.Networking;
    using System.Collections;
    using System.Collections.Generic;
    using System;

    /// <summary>
    /// An implementation of IHttpChannel with UnityWebRequest. Simple request queue is provided.
    /// </summary>
    public class UnityHttpChannel : IHttpChannel
    {
        private Queue<DataRequestContainer> m_queue = new Queue<DataRequestContainer>();

        private UnityWebRequest m_www;

        private DataRequestContainer m_currentRequest;

        private DataServiceSettings m_settings;

        public bool AcceptsNewRequest
        {
            get
            {
                // This rule stops the queuing.
                return m_www == null;
            }
        }

        /// <summary>
        /// Initialize the channel.
        /// </summary>
        public void Initialize(DataServiceSettings settings)
        {
            m_settings = settings;
        }
        /// <summary>
        /// Makes a request.
        /// </summary>
        public void MakeRequest(DataRequestContainer container)
        {
            if (container.Request == null)
            {
                Debug.LogError("Ignoring container with null request!");
                return;
            }
            m_queue.Enqueue(container);
            container.MarkAsSubmitted();
            if (m_www == null)
            {
                StartNextRequest();
            }            
        }
        /// <summary>
        /// Checks and starts next avaiable request.
        /// </summary>
        private void StartNextRequest()
        {
            if (m_queue.Count > 0)
            {
                StartRequest(m_queue.Dequeue());
            }
        }
        /// <summary>
        /// Starts the specified request.
        /// </summary>
        private void StartRequest(DataRequestContainer container)
        {            
            if (container.Request == null)
            {
                Debug.LogError("Ignoring container with null request!");
                return;
            }
            m_www = UnityWebRequest.Post(container.FullUrl, container.DataForm);
            m_www.Send();
            m_currentRequest = container;
            m_currentRequest.MarkAsStarted();
        }

        public void Shutdown()
        {
            m_queue.Clear();
        }

        /// <summary>
        /// Update loop.
        /// </summary>
        public void Update()
        {
            if (m_www == null)
            {
                StartNextRequest();
                return;
            }
            if (m_www.isDone)
            {
                if (m_www.isError || m_www.responseCode != 200)
                {
                    var error = new DataRequestError() { HttpCode = (int)m_www.responseCode, ErrorMessage = m_www.error };
                    if (string.Equals(error.ErrorMessage, "Cannot connect to destination host"))
                    {
                        error.Error = DataRequestErrorType.NoNetwork;
                    }
                    else
                    {
                        error.Error = DataRequestErrorType.HttpError;
                    }
                    m_currentRequest.InvokeErrorCallback(error);
                    AbortCurrentRequest();
                }
                else
                {
                    m_currentRequest.CompleteRequest(m_www.downloadHandler.text);
                    ClearCurrentRequest();
                    StartNextRequest();
                }
            }
            else
            {
                if (m_currentRequest.IsRequestTimeOut)
                {
                    m_currentRequest.InvokeErrorCallback(new DataRequestError() { Error = DataRequestErrorType.RequestTimeOut });
                    AbortCurrentRequest();
                }
            }
        }
        /// <summary>
        /// Abort the current request.
        /// </summary>
        private void AbortCurrentRequest()
        {
            if (m_www != null)
            {
                m_www.Abort();
            }
            ClearCurrentRequest();
        }
        /// <summary>
        /// Clears the current web request objects, so it will be available for next request.
        /// </summary>
        private void ClearCurrentRequest()
        {
            m_www = null;
            m_currentRequest = null;
        }
    }
}