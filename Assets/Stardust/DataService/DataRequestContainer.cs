namespace Stardust
{
    using UnityEngine;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Status of the DataRequestContainer.
    /// </summary>
    public enum DataRequestContainerStatus
    {
        /// <summary>
        /// The container is initialized and empty for fill any request.
        /// </summary>
        Empty,
        /// <summary>
        /// The container is prepared with request and ready for use.
        /// </summary>
        Ready,
        /// <summary>
        /// The container has been submitted to http channel.
        /// </summary>
        Submitted,
        /// <summary>
        /// The request is started and in progress.
        /// </summary>
        InProgress,
        /// <summary>
        /// The request is completed.
        /// </summary>
        Completed
    }

    /// <summary>
    /// A container for data request. Holds all neccessary information for processing a request.
    /// </summary>
    public class DataRequestContainer
    {
        /// <summary>
        /// Status of the request. The status cannot be directly changed.
        /// </summary>
        public DataRequestContainerStatus Status { get; private set; }
        /// <summary>
        /// The full url of the request.
        /// </summary>
        public string FullUrl { get; private set; }
        /// <summary>
        /// The request object.
        /// </summary>
        public DataRequestCommon Request { get; private set; }
        /// <summary>
        /// The result object.
        /// </summary>
        public DataResultCommon Result { get; private set; }    
        /// <summary>
        /// Callback when the request is successfully completed with no error returned.
        /// </summary>
        public Action<DataResultCommon> SuccessCallback;
        /// <summary>
        /// Callback when error happened.
        /// </summary>
        public Action<DataRequestError> ErrorCallback;
        /// <summary>
        /// Triggers when the request is completed, before calling success callback.
        /// </summary>
        public event Action<DataRequestContainer> OnComplete;
        /// <summary>
        /// Triggers when the request is completed with error, before calling error callback.
        /// </summary>
        public event Action<DataRequestContainer, DataRequestError> OnError;
        /// <summary>
        /// The data form extracted from the request object.
        /// </summary>
        public Dictionary<string, string> DataForm { get; private set; }
        /// <summary>
        /// Time out in ms.
        /// </summary>
        public int RequestTimeOutMs = 2000;
        /// <summary>
        /// Checks whether the request has timed out since its started time.
        /// </summary>
        public bool IsRequestTimeOut
        {
            get
            {
                return (DateTime.Now - timeStartedRequest).TotalMilliseconds >= RequestTimeOutMs;
            }
        }
        private DateTime timeStartedRequest;

        public DataRequestContainer()
        {
            DataForm = new Dictionary<string, string>();
        }
        public void MarkAsSubmitted()
        {
            if (Status != DataRequestContainerStatus.Ready)
            {
                throw new InvalidOperationException();
            }
            Status = DataRequestContainerStatus.Submitted;
        }
        /// <summary>
        /// Marks the request as started, and logs its start time.
        /// </summary>
        public void MarkAsStarted()
        {
            if (Status != DataRequestContainerStatus.Ready && Status != DataRequestContainerStatus.Submitted)
            {
                throw new InvalidOperationException();
            }
            Status = DataRequestContainerStatus.InProgress;
            timeStartedRequest = DateTime.Now;
        }
        /// <summary>
        /// Fills the container with specified settings and request, and prepare it for process.
        /// </summary>
        public void FillRequest(DataServiceSettings settings, DataRequestCommon request)
        {
            if (Status != DataRequestContainerStatus.Empty)
            {
                throw new InvalidOperationException();
            }
            Request = request;
            FullUrl = Request.FormatUrl(settings.ServerUrl);
            Request.FillDataForm(DataForm);
#if UNITY_EDITOR
            DumpRequest();
#endif
            Status = DataRequestContainerStatus.Ready;
        }

        /// <summary>
        /// Completes the request, mark it as completed and invoke success or error callbacks.
        /// </summary>
        public void CompleteRequest(string resultString)
        {
#if UNITY_EDITOR
            Debug.LogFormat("[{0}] Result: {1}", Request.Name, resultString);
#endif
            try
            {
                Result = Request.GetResult(resultString);
            }
            catch(Exception e)
            {
                Debug.LogErrorFormat("Error happened parsing result for request {0}: {1}", Request.Name, e.Message);
                Debug.LogException(e);
                InvokeErrorCallback(new DataRequestError() { Error = DataRequestErrorType.ResponseParsingError });
                return;
            }
            Result.CustomData = Request.customData;
            Status = DataRequestContainerStatus.Completed;
            if (Result.Error != null)
            {
                if (OnError != null)
                {
                    OnError(this, Result.Error);
                }
                SafeInvokeErrorCallback(Result.Error);
            }
            else
            {
                if (OnComplete != null)
                {
                    OnComplete(this);
                }
                SafeInvokeSuccessCallback(Result);
            }
        }

        /// <summary>
        /// Mark the request as completed and invoke error callback.
        /// </summary>
        public void InvokeErrorCallback(DataRequestError error)
        {
            Status = DataRequestContainerStatus.Completed;
            if (OnError != null)
            {
                OnError(this, error);
            }
            SafeInvokeErrorCallback(error);
        }

        private void DumpRequest()
        {
            using (PoolableStringBuilder poolable = Pools.StringBuilderPool.Borrow())
            {
                System.Text.StringBuilder builder = poolable.Value;
                builder.AppendFormat("[{0}] Url: {1}\n", Request.Name, FullUrl).AppendLine();
                foreach (var key in DataForm.Keys)
                {
                    builder.Append(key).Append('=').Append(DataForm[key]).AppendLine();
                }
                Debug.Log(builder.ToString());
            }
        }

        /// <summary>
        /// Safely calls the success callback, and capture any exception.
        /// </summary>
        private void SafeInvokeSuccessCallback(DataResultCommon result)
        {
            try
            {
                if (SuccessCallback != null)
                {
                    SuccessCallback(result);
                }
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("Exception invoking success callback for request {0}", Request.Name);
                Debug.LogException(e);
            }
        }

        /// <summary>
        /// Safely calls the error callback, and capture any exception.
        /// </summary>
        private void SafeInvokeErrorCallback(DataRequestError error)
        {
            try
            {
                if (ErrorCallback != null)
                {
                    ErrorCallback(error);
                }
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("Exception invoking error callback for request {0}", Request.Name);
                Debug.LogException(e);
            }
        }

        /// <summary>
        /// Resets and clears the container, so it could be used for another request.
        /// </summary>
        public void Reset()
        {
            FullUrl = null;
            Request = null;
            Result = null;
            SuccessCallback = null;
            ErrorCallback = null;
            DataForm.Clear();
            Status = DataRequestContainerStatus.Empty;
        }
    }
}