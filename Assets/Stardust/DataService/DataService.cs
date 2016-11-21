namespace Stardust
{
    using UnityEngine;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// DataService is responsible to submit requests to backend severs, and process the data received from server. It also manages the global user data.
    /// </summary>
    public class DataService : MonoBehaviour
    {
        private IHttpChannel[] m_channels;
        private List<DataRequestContainer> m_containers = new List<DataRequestContainer>();
        private DataServiceSettings m_settings;

        void Update()
        {
            for (int i = 0; i < m_channels.Length; i ++)
            {
                m_channels[i].Update();
            }
            CheckContainers();
            OnUpdate();
        }

        protected virtual void OnDestroy()
        {
            for (int i = 0; i < m_channels.Length; i++)
            {
                m_channels[i].Shutdown();
            }
        }

        /// <summary>
        /// Initialize the data service.
        /// </summary>
        public void Initialize()
        {
            m_settings = GetSettings();
            DataServiceSettings.Current = m_settings;
            CreateChannels(m_settings.ChannelCount);
            OnInitialize();
        }

        /// <summary>
        /// Enqueues a request.
        /// </summary>
        protected virtual void EnqueueRequest<Result>(DataRequestCommon request, Action<Result> successCallback, Action<DataRequestError> errorCallback)
            where Result: DataResultCommon
        {
            DataRequestContainer emptyContainer = null;
            for (int i = 0; i < m_containers.Count; i ++)
            {
                if (m_containers[i].Status == DataRequestContainerStatus.Empty)
                {
                    emptyContainer = m_containers[i];
                    break;
                }
            }
            if (emptyContainer == null)
            {
                emptyContainer = new DataRequestContainer();
                emptyContainer.RequestTimeOutMs = m_settings.RequestTimeOut;
                emptyContainer.OnComplete += OnRequestCompleted;
                emptyContainer.OnError += OnRequestError;
                m_containers.Add(emptyContainer);
            }

            emptyContainer.FillRequest(m_settings, request);
            emptyContainer.SuccessCallback = (DataResultCommon result) =>
            {
                if (successCallback != null)
                {
                    successCallback((Result)result);
                }
            };
            emptyContainer.ErrorCallback = errorCallback;
        }
        /// <summary>
        /// Gets the settings. Child class can override this to provide custom settings.
        /// </summary>
        /// <returns></returns>
        protected virtual DataServiceSettings GetSettings()
        {
            return DataServiceSettings.Default;
        }
        /// <summary>
        /// Creates the channel. Child class can override this to use a different channel.
        /// </summary>
        protected virtual IHttpChannel CreateChannel()
        {
            return new UnityHttpChannel();
        }
        /// <summary>
        /// Custom initialization logic for child class.
        /// </summary>
        protected virtual void OnInitialize()
        {

        }
        /// <summary>
        /// Update loop for child class.
        /// </summary>
        protected virtual void OnUpdate()
        {
        }
        /// <summary>
        /// Custom logic for child class when request has error.
        /// </summary>
        protected virtual void OnRequestError(DataRequestContainer container, DataRequestError error)
        {
        }
        /// <summary>
        /// Custom logic for child class when request is completed.
        /// </summary>
        protected virtual void OnRequestCompleted(DataRequestContainer container)
        {
        }
        /// <summary>
        /// Check the containers, releases the completed ones, and starts newly ready ones.
        /// </summary>
        private void CheckContainers()
        {
            for (int i = 0; i < m_containers.Count; i ++)
            {
                DataRequestContainer container = m_containers[i];
                if (container.Status == DataRequestContainerStatus.Completed)
                {
                    container.Reset();
                }
                else if (container.Status == DataRequestContainerStatus.Ready)
                {
                    var channel = GetChannel(container.Request.Channel);
                    if (channel.AcceptsNewRequest)
                    {
                        channel.MakeRequest(container);
                    }                    
                }
            }
        }

        private void CreateChannels(int initialChannelCount)
        {
            if (initialChannelCount <= 0)
            {
                Debug.LogErrorFormat("Invalid channel count {0} in data service settings! Will use default value as 2 channels.", initialChannelCount);
                initialChannelCount = 2;
            }
            m_channels = new IHttpChannel[initialChannelCount];
            for (int i = 0; i < initialChannelCount; i ++)
            {
                var channel = CreateChannel();
                channel.Initialize(m_settings);
                m_channels[i] = channel;
            }
        }

        private IHttpChannel GetChannel(int channel)
        {
            if (channel >= 0 && channel < m_channels.Length)
            {
                return m_channels[channel];
            }

            Debug.LogErrorFormat("Invalid HTTP channel number {0}, default channel will be used!", channel);
            return m_channels[m_channels.Length - 1];
        }
    }
}