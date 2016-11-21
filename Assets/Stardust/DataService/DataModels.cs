namespace Stardust
{
    using UnityEngine;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    #region Some delegate definitions - this is not used at this moment.
    //public delegate void DataRequestErrorEvent(DataRequestCommon request, DataRequestError error);
    //public delegate void DataResultEvent<in TResult>(TResult result) where TResult : DataRequestCommon;
    //public delegate void DataRequestEvent<in TRequest>(TRequest request) where TRequest : DataResultCommon;
    #endregion

    /// <summary>
    /// Defines types of request errors.
    /// </summary>
    public enum DataRequestErrorType
    {
        /// <summary>
        /// Cannot connect to destination host due to no network.
        /// </summary>
        NoNetwork,
        /// <summary>
        /// The service has timed out waiting for request.
        /// </summary>
        RequestTimeOut,
        /// <summary>
        /// Generic HTTP error.
        /// </summary>
        HttpError,
        /// <summary>
        /// Server returned business logic error.
        /// </summary>
        ServerReturnedError,
        /// <summary>
        /// The client has received response from server, but there is problem parsing the response.
        /// </summary>
        ResponseParsingError
    }
    /// <summary>
    /// The complete error information when a request goes wrong.
    /// </summary>
    public class DataRequestError
    {
        /// <summary>
        /// Type of this error.
        /// </summary>
        public DataRequestErrorType Error;
        /// <summary>
        /// The generic http error code.
        /// </summary>
        public int HttpCode;
        /// <summary>
        /// Business logic error code.
        /// </summary>
        public string ErrorCode;
        /// <summary>
        /// Error message returned by server.
        /// </summary>
        public string ErrorMessage;
        /// <summary>
        /// Whether it is a generic network problem.
        /// </summary>
        public bool IsNetworkProblem
        {
            get
            {
                return Error == DataRequestErrorType.NoNetwork || Error == DataRequestErrorType.RequestTimeOut;
            }
        }

        public override string ToString()
        {
            return string.Format("Error: {0}; Http: {1}; ErrorCode: {2}; Message: {3}", Error, HttpCode, string.IsNullOrEmpty(ErrorCode) ? string.Empty : ErrorCode, string.IsNullOrEmpty(ErrorMessage) ? string.Empty : ErrorMessage);
        }
    }
    /// <summary>
    /// Base class for all data requests.
    /// </summary>
    [Serializable]
    public abstract class DataRequestCommon
    {
        /// <summary>
        /// Unique name of the request.
        /// </summary>
        public abstract string Name { get; }
        /// <summary>
        /// Channel number used by this request. Default is 0.
        /// </summary>
        public int Channel = 0;
        /// <summary>
        /// Relative path of the request url, e.g. "/Client/Login"
        /// </summary>
        public string EndPoint = null;
        /// <summary>
        /// Format and get the full url from provided server url.
        /// </summary>
        public virtual string FormatUrl(string serverUrl)
        {
            return serverUrl + EndPoint;
        }
        /// <summary>
        /// Format the data of the request and fills the specified form in dictionary.
        /// </summary>
        public abstract void FillDataForm(Dictionary<string, string> form);
        /// <summary>
        /// Parses the payload string and gets the result object.
        /// </summary>
        public abstract DataResultCommon GetResult(string payload);
        /// <summary>
        /// Custom data provided by the caller - which will be passed back to the callback in the result object.
        /// </summary>
        [NonSerialized]
        public object customData = null;
    }
    /// <summary>
    /// Base class for all data request results.
    /// </summary>
    [Serializable]
    public class DataResultCommon
    {
        /// <summary>
        /// The request object.
        /// </summary>
        //[NonSerialized]
        //public DataRequestCommon Request;        
        /// <summary>
        /// The error object. It will be null if the request was successful.
        /// </summary>
        [NonSerialized]
        public DataRequestError Error;
        /// <summary>
        /// Custom data provided by the caller
        /// </summary>
        [NonSerialized]
        public object CustomData;
    }
}