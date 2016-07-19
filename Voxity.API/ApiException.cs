using System;
using System.Runtime.Serialization;

namespace Voxity.API
{
    /// <summary>
    /// Handle API Voxity exception from <see cref="ApiSession"/>
    /// </summary>
    public class ApiSessionException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public ApiSessionException()
        : base() { }

        /// <summary>
        /// 
        /// </summary>
        public ApiSessionException(string message)
        : base(message) { }

        /// <summary>
        /// 
        /// </summary>
        public ApiSessionException(string format, params object[] args)
        : base(string.Format(format, args)) { }

        /// <summary>
        /// 
        /// </summary>
        public ApiSessionException(string message, Exception innerException)
        : base(message, innerException) { }

        /// <summary>
        /// 
        /// </summary>
        public ApiSessionException(string format, Exception innerException, params object[] args)
        : base(string.Format(format, args), innerException) { }

        /// <summary>
        /// 
        /// </summary>
        protected ApiSessionException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
    }

    /// <summary/>
    [Serializable]
    public class ApiErrorResponseException : ApiSessionException
    {
        const string errorResponseMsg = "Voxity API occures errors.";

        /// <summary>
        /// 
        /// </summary>
        public ApiErrorResponseException(string error, string errorDescription)
            : base(string.Format("Error : {0}. Description : {1}.", error, errorDescription)) { }

        /// <summary>
        /// 
        /// </summary>
        public ApiErrorResponseException(string errors, Exception inner)
            : base(string.Format("{0} - {1}",
                    errorResponseMsg, errors), inner)
        {
            this.HelpLink = "https://api.voxity.fr/doc/";
            this.Source = "ApiResponseError";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ApiTooManyRequestException : ApiSessionException
    {
        /// <summary>
        /// 
        /// </summary>
        public ApiTooManyRequestException()
            : base(string.Format("Too many request. Please wait for 2 seconds, before retrying.")) { }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ApiAdminException : ApiSessionException
    {
        /// <summary>
        /// 
        /// </summary>
        public ApiAdminException()
            : base(string.Format("Only authenticated Admins can access the data.")) { }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ApiAuthenticateException : ApiSessionException
    {
        /// <summary>
        /// 
        /// </summary>
        public ApiAuthenticateException()
            : base(string.Format("User not authenticated")) { }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ApiInternalErrorException : ApiSessionException
    {
        /// <summary>
        /// 
        /// </summary>
        public ApiInternalErrorException()
            : base(string.Format("Please contact Voxity support.")) { }
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ApiArgumentException : ArgumentException
    {
        /// <summary>
        /// 
        /// </summary>
        public ApiArgumentException(string error)
            : base(string.Format("Voxity API parameters errors :", error)) { }
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class AccessTokenException : ApiSessionException
    {
        /// <summary>
        /// 
        /// </summary>
        public AccessTokenException(string token)
            : base(string.Format("Invalid Access Token. Value : {0}", token)){ }
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class RefreshTokenException : ApiSessionException
    {
        /// <summary>
        /// 
        /// </summary>
        public RefreshTokenException(string token)
            : base(string.Format("Invalid Refresh Token. Value : {0}", token)){ }
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ClientIdSecretException : ApiSessionException
    {
        /// <summary>
        /// 
        /// </summary>
        public ClientIdSecretException(string clientId, string clientSecret)
            : base(string.Format("Invalid ClientId and/or ClientSecret." 
                + Environment.NewLine + "ClientId : {0}" 
                + Environment.NewLine + "ClientSecret : {1}", clientId, clientSecret))
        {
            this.HelpLink = "https://client.voxity.fr/voxity-api/configuration";
        }
    }
}
