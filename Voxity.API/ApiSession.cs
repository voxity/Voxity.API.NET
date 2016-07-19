using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Voxity.API.Models;

namespace Voxity.API
{
    /// <summary/>
    public interface IApiSession
    {
        /// <summary/>
        string ClientId { get; set; }
        /// <summary/>
        string ClientSecret { get; set; }
        /// <summary/>
        string HostRedirect { get; set; }
        /// <summary/>
        int PortRedirect { get; set; }
        /// <summary/>
        string AccessToken { get; set; }
        /// <summary/>
        string RefreshToken { get; set; }
        /// <summary/>
        DateTime TokenExpirationDate { get; set; }

        /// <summary/>
        string UriRedirect { get; }

        /// <summary/>
        HttpResponseMessage Request(ApiSession.HttpMethod method, string extUrl, Dictionary<string, string> urlParams = null, bool isTokenRequired = true, string contentType = null, string contentValue = null, int retry = 0);
    }

    /// <summary>
    /// Provide methods to authentificate users in the application.
    /// Manage tokens, refresh tokens, HTTP querys with POST, GET, PUT and DELETE.
    /// Give the result Request to standard or Json format.
    /// </summary>
    public class ApiSession : IApiSession
    {
        #region Public propertys

        /// <summary>
        /// This parameter can be found <see href="https://client.voxity.fr/voxity-api/configuration">here</see>.
        /// It's called "Identifiant client".
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// This parameter can be found <see href="https://client.voxity.fr/voxity-api/configuration">here</see>.
        /// It's called "Api Secret".
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// This parameter set when the authentification success.
        /// It is critical for query who need authentification.
        /// </summary>
        /// <remarks>
        /// You can store this token in your local settings of your app for further reuse it.
        /// Keep attention : the token expired after 1 hour. Use the <see cref="RefreshToken"/> for refresh the token.
        /// </remarks>
        public string AccessToken { get; set; }

        /// <summary>
        /// This parameter set when the authentification success.
        /// It permit the auto authentification when the access token expires.
        /// </summary>
        /// <remarks>
        /// You can store this refresh token in your local settings of your app for further reuse it.
        /// </remarks>
        public string RefreshToken { get; set; }


        private string hostRedirect;
        /// <summary>
        /// This parameter can be found <see href="https://client.voxity.fr/voxity-api/configuration">here</see>.
        /// It's called "Url de redirection".
        /// It requires you have a HTTP server for authentification.
        /// </summary>
        public string HostRedirect
        {
            get
            {
                return hostRedirect;
            }
            set
            {
                hostRedirect = value;
            }
        }

        private int portRedirect;
        /// <summary>
        /// This parameter can be found <see href="https://client.voxity.fr/voxity-api/configuration">here</see>.
        /// It's called "Url de redirection".
        /// If nothing is set, you can set this parameter to 80, the standard HTTP listen port.
        /// </summary>
        public int PortRedirect
        {
            get
            {
                return portRedirect;
            }
            set
            {
                portRedirect = value;
            }
        }

        /// <summary/>
        public DateTime TokenExpirationDate { get; set; }

        /// <summary>
        /// This field set when the <see cref="HostRedirect"/> and <see cref="PortRedirect"/> sets.
        /// </summary>
        public string UriRedirect
        {
            get
            {
                return "http://" + HostRedirect + ':' + PortRedirect.ToString();
            }
        }


        /// <summary>
        /// Return the Uri to connect the user in the Voxity API.
        /// </summary>
        public string AskAuthUri
        {
            get
            {
                return apiPrefixe + "dialog/authorize?redirect_uri=" + UriRedirect + "&response_type=code&client_id=" + WebUtility.UrlEncode(ClientId);
            }
        }
        #endregion

        #region Public fields
        /// <summary/>
        public EndPoints.Authentication Authentication { get; private set; }

        /// <summary/>
        public EndPoints.Calls Calls { get; private set; }

        /// <summary/>
        public EndPoints.Contacts Contacts { get; private set; }

        /// <summary/>
        public EndPoints.Devices Devices { get; private set; }

        /// <summary/>
        public EndPoints.Sms Sms { get; private set; }

        /// <summary/>
        public EndPoints.Tokens Tokens { get; private set; }

        /// <summary/>
        public EndPoints.Users Users { get; private set; }

        /// <summary/>
        public EndPoints.Vms Vms { get; private set; }
        #endregion

        #region Private const

        private const string apiPrefixe = "https://api.voxity.fr/api/v1/";

        #endregion

        /// <summary>
        /// ApiSession constructor. Check if all parameters seems good.
        /// </summary>
        public ApiSession(string clientId, string clientSecret, string hostRedirect, int portRedirect,
                          string accessToken = null, string refreshToken = null)
        {
            // Set properties
            ClientId = clientId;
            ClientSecret = clientSecret;
            HostRedirect = hostRedirect;
            PortRedirect = portRedirect;
            AccessToken = accessToken;
            RefreshToken = refreshToken;

            // Set fields
            Authentication = new EndPoints.Authentication(this);
            Calls = new EndPoints.Calls(this);
            Contacts = new EndPoints.Contacts(this);
            Devices = new EndPoints.Devices(this);
            Sms = new EndPoints.Sms(this);
            Tokens = new EndPoints.Tokens(this);
            Users = new EndPoints.Users(this);
            Vms = new EndPoints.Vms(this);
        }

        /// <summary>
        /// HttpMethod API enum. Enum all HTTP method allowed.
        /// </summary>
        public enum HttpMethod
        {
            /// <summary>
            /// GET HTTP method.
            /// </summary>
            Get,

            /// <summary>
            /// POST HTTP method.
            /// </summary>
            Post,

            /// <summary>
            /// PUT HTTP method.
            /// </summary>
            Put,

            /// <summary>
            /// DELETE HTTP method.
            /// </summary>
            Delete
        }

        /// <summary>
        /// Connect allow you to have full access on Endpoints by get the tokens.
        /// </summary>
        /// <remarks>
        /// You can get the code at : redirect_uri/?code=XXXXX
        /// Refer to : <see href="https://api.voxity.fr/doc/#api-Token-PostDecision"/>
        /// </remarks>
        /// <param name="code">
        /// This parameter set when the user allow your app tu use there personnal informations.
        /// It's necessary to get <see cref="AccessToken"/> and <see cref="RefreshToken"/>.
        /// </param>
        public AccessToken Connect(string code)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("redirect_uri", UriRedirect);
            values.Add("client_secret", ClientSecret);
            values.Add("client_id", ClientId);
            values.Add("code", code);
            values.Add("grant_type", "authorization_code");

            HttpResponseMessage response = null;
            string url = apiPrefixe + "oauth/token";

            using (HttpClient client = new HttpClient())
            {
                response = PostRequest(client, url, values, isJson: false);
            }

            if (response.IsSuccessStatusCode)
            {
                // Affect access_token, refresh_token values
                AccessToken respToken = JsonConvert.DeserializeObject<AccessToken>((response.Content.ReadAsStringAsync().Result));

                AccessToken = respToken.access_token;
                RefreshToken = respToken.refresh_token;

                TokenExpirationDate = DateTime.Now.AddSeconds(respToken.expires_in);

                return respToken;
            }
            else
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        throw new ClientIdSecretException(ClientId, ClientSecret);

                    case HttpStatusCode.Forbidden:
                        // too many request
                        Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                        throw new RefreshTokenException(RefreshToken);

                    case HttpStatusCode.NotImplemented:
                        ErrorToken errorToken = JsonConvert.DeserializeObject<ErrorToken>((response.Content.ReadAsStringAsync().Result));
                        throw new ApiErrorResponseException(errorToken.error, errorToken.error_description);

                    // Really bad case... NginX answers 502 error when unsuported case is test
                    case HttpStatusCode.BadGateway:
                        throw new ApiSessionException("Unknown error response. Check if you pass the correct code. If persist, please contact Voxity support.");

                    default:
                        throw new HttpRequestException(response.StatusCode.ToString());
                }
            }
        }

        /// <summary/>
        public void Disconnect()
        {
            EndPoints.Authentication auth = new EndPoints.Authentication(this);
            auth.Logout();
        }

        /// <summary/>
        public RefreshToken AutoRefresh()
        {
            if (RefreshToken == null)
                throw new ApiSessionException("REFRESH_TOKEN not set.");

            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("client_secret", ClientSecret);
            values.Add("client_id", ClientId);
            values.Add("refresh_token", RefreshToken);
            values.Add("grant_type", "refresh_token");

            HttpResponseMessage response = null;
            string url = apiPrefixe + "oauth/token";

            using (HttpClient client = new HttpClient())
            {

                response = PostRequest(client, url, values, isJson: false);

            }

            if (response.IsSuccessStatusCode)
            {
                // Affect access_token, refresh_token values
                RefreshToken respToken = JsonConvert.DeserializeObject<RefreshToken>((response.Content.ReadAsStringAsync().Result));

                AccessToken = respToken.access_token;

                TokenExpirationDate = DateTime.Now.AddSeconds(respToken.expires_in);

                return respToken;
            }
            else
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        throw new ClientIdSecretException(ClientId, ClientSecret);

                    case HttpStatusCode.Forbidden:
                        throw new RefreshTokenException(response.Content.ReadAsStringAsync().Result);

                    case HttpStatusCode.NotImplemented:
                        ErrorToken errorToken = JsonConvert.DeserializeObject<ErrorToken>((response.Content.ReadAsStringAsync().Result));
                        throw new ApiErrorResponseException(errorToken.error, errorToken.error_description);

                    default:
                        throw new HttpRequestException(response.StatusCode.ToString());
                }
            }
        }

        /// <summary>
        /// Do a HTTP request to a specific <a href="https://api.voxity.fr/doc/">API Voxity</a> url, with optional params query.
        /// </summary>
        /// <param name="method">HTTP method Request : "GET", "POST", "PUT", "DELETE"</param>
        /// <param name="extUrl">API extension path. Example : "calls/logs". Return : base + ext_url = "https://api.voxity.fr/api/v1/calls/logs"</param>
        /// <param name="urlParams">Dictionary for URL parameters. Example : <c>values.Add("channel_id", "2707");</c>. Return : "https://api.voxity.fr/api/v1/channels?channel_id=2707"</param>
        /// <param name="isTokenRequired">Specify if token is required for the Request or not.</param>
        /// <param name="contentType">What is type of datas sent. Example : "application/json"</param>
        /// <param name="contentValue">Datas content type to sent. Example : <c>"{\"exten\":\"" + exten + "\"}"</c></param>
        /// <param name="retry">Retry request counter. Use into the case of 429 HTTP error.</param>
        /// <returns>
        /// Result of the request.
        /// </returns>
        public HttpResponseMessage Request(HttpMethod method, string extUrl, Dictionary<string, string> urlParams = null, bool isTokenRequired = true, string contentType = null, string contentValue = null, int retry = 0)
        {
            // store to local
            if (DateTime.Now >= TokenExpirationDate)
                AutoRefresh();

            HttpResponseMessage response = null;
            string url = apiPrefixe + extUrl;

            using (HttpClient client = new HttpClient())
            {

                if (isTokenRequired == true)
                {

                    if (AccessToken == null)
                        throw new ApiSessionException("ACCESS_TOKEN not set.");

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
                }

                if (contentType != null)
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                }

                switch (method)
                {
                    case HttpMethod.Get:
                        response = GetRequest(client, url, urlParams);
                        break;

                    case HttpMethod.Post:
                        if (contentValue == null)
                            response = PostRequest(client, url, urlParams, isJson:false);
                        else
                            response = PostRequest(client, url, contentValue: contentValue, isJson: true);
                        break;

                    case HttpMethod.Put:
                        if (contentValue == null)
                            response = PutRequest(client, url, urlParams, isJson: false);
                        else
                            response = PutRequest(client, url, contentValue: contentValue, isJson: true);
                        break;

                    case HttpMethod.Delete:
                        if (contentValue == null)
                            response = DeleteRequest(client, url);
                        break;

                    default:
                        throw new ApiSessionException("[ERROR : HTTP Method unknown] - Please give a method for the request.");

                }

                return response;
                
            }
        }

        private HttpResponseMessage GetRequest(HttpClient client, string url, Dictionary<string, string> url_params = null)
        {
            // if exist url params
            if (url_params != null)
            {
                url += "?";
                foreach (KeyValuePair<string, string> parameter in url_params)
                {
                    if (parameter.Key == "filter")
                        url += parameter.Key + "=" + parameter.Value;
                    else
                        url += "\"" + parameter.Key + "\"=\"" + parameter.Value + "\"";
                }
            }

            return client.GetAsync(url).Result;
        }

        private HttpResponseMessage PostRequest(HttpClient client, string url, Dictionary<string, string> postParams = null, string contentValue = null, bool isJson = true)
        {
            if (isJson == false)
                return client.PostAsync(url, new FormUrlEncodedContent(postParams)).Result;
            else
                return client.PostAsync(url, new StringContent(contentValue, Encoding.UTF8, "application/json")).Result;
        }

        private HttpResponseMessage PutRequest(HttpClient client, string url, Dictionary<string, string> postParams = null, string contentValue = null, bool isJson = true)
        {
            if (isJson == false)
                return client.PutAsync(url, new FormUrlEncodedContent(postParams)).Result;
            else
                return client.PutAsync(url, new StringContent(contentValue, Encoding.UTF8, "application/json")).Result;
        }

        private HttpResponseMessage DeleteRequest(HttpClient client, string url)
        {
            return client.DeleteAsync(url).Result;
        }
    }
}
