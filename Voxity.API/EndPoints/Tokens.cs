using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Voxity.API.Models;

namespace Voxity.API.EndPoints
{
    /// <summary>
    /// 
    /// </summary>
    public class Tokens
    {
        #region Public field
        /// <summary>
        /// Redirection URI to ask authentification
        /// </summary>
        public string TokenAuthorization
        {
            get
            {
                return "https://api.voxity.fr/api/v1/dialog/authorize?redirect_uri=" + tokenSession.UriRedirect + "&response_type=code&client_id=" + tokenSession.ClientId;
            }
        }
        #endregion

        private IApiSession tokenSession;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        public Tokens(IApiSession session)
        {
            tokenSession = session;
        }

        /// <summary>
        /// Voxity page asking end-user to allow your app to get there personnal informations. Requirements : HTTP listen server. Refer to : <see href="https://api.voxity.fr/doc/#api-Token-GetAuthorization"/>
        /// </summary>
        /// <returns><c>HTTP/1.1 200 OK</c></returns>
        public Uri AskAuthorization()
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("redirect_uri", tokenSession.UriRedirect);
            values.Add("response_type", "code");
            values.Add("client_id", tokenSession.ClientId);

            HttpResponseMessage response = tokenSession.Request(ApiSession.HttpMethod.Get, "dialog/authorize", urlParams: values, isTokenRequired: false);

            if (response.IsSuccessStatusCode)
            {
                return response.RequestMessage.RequestUri;
            }
            else
            {
                throw new HttpRequestException(response.StatusCode.ToString());
            }

        }

        /// <summary>
        /// Voxity page asking end-user to allow your app to get there personnal informations. Refer to : <see href="https://api.voxity.fr/doc/#api-Token-PostDecision"/>
        /// </summary>
        /// <returns>True if success.</returns>
        public void UserDecision()
        {
            HttpResponseMessage response = tokenSession.Request(ApiSession.HttpMethod.Post, "dialog/authorize/decision", isTokenRequired: false);

            switch (response.StatusCode)
            {
                case HttpStatusCode.Redirect:
                    //TODO : Redirect with code if success

                default:
                    throw new HttpRequestException();
            }
        }

        /* code : First Time - code provide by redirect listen.
         *        Other time, code is refresh token.
         * grant_type : First connection or refresh token expired : use "authorization_code" value
         *              Other time, use "refresh_token" if you have the refresh token
         */
        /// <summary>
        /// Get the first Request to ask access token and refresh token. Refer to : <see href="https://api.voxity.fr/doc/#api-Token-PostToken"/>
        /// </summary>
        /// <param name="code">Code provide by previous Request (User Decision). The code appeared in the redirect URI params : <c>redirect_uri/?code=XXXXX</c></param>
        /// <returns><see cref="AccessToken"/> object.</returns>
        public AccessToken GetAccessToken(string code)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("redirect_uri", tokenSession.UriRedirect);
            values.Add("client_secret", tokenSession.ClientSecret);
            values.Add("client_id", tokenSession.ClientId);
            values.Add("code", code);
            values.Add("grant_type", "authorization_code");

            HttpResponseMessage response = tokenSession.Request(ApiSession.HttpMethod.Post, "oauth/token", urlParams: values, isTokenRequired: false);

            if (response.IsSuccessStatusCode)
            {
                // Affect access_token, refresh_token values
                AccessToken respToken = JsonConvert.DeserializeObject<AccessToken>((response.Content.ReadAsStringAsync().Result));

                tokenSession.AccessToken = respToken.access_token;
                tokenSession.RefreshToken = respToken.refresh_token;

                tokenSession.TokenExpirationDate = DateTime.Now.AddSeconds(respToken.expires_in);

                return respToken;
            }
            else
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        throw new ClientIdSecretException(tokenSession.ClientId, tokenSession.ClientSecret);

                    case HttpStatusCode.Forbidden:
                        throw new RefreshTokenException(tokenSession.RefreshToken);

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

        /// <summary>
        /// Get Request to ask new access token by refresh token. Refer to : <see href="https://api.voxity.fr/doc/#api-Token-PostToken" />
        /// </summary>
        /// <returns><see cref="AccessToken"/> object.</returns>
        public RefreshToken RefreshAccessToken()
        {
            if (tokenSession.RefreshToken == null)
                throw new ApiSessionException("REFRESH_TOKEN not set.");

            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("client_secret", tokenSession.ClientSecret);
            values.Add("client_id", tokenSession.ClientId);
            values.Add("refresh_token", tokenSession.RefreshToken);
            values.Add("grant_type", "refresh_token");

            HttpResponseMessage response = tokenSession.Request(ApiSession.HttpMethod.Post, "oauth/token", urlParams: values, isTokenRequired: false);

            if (response.IsSuccessStatusCode)
            {
                // Affect access_token, refresh_token values
                RefreshToken respToken = JsonConvert.DeserializeObject<RefreshToken>((response.Content.ReadAsStringAsync().Result));

                tokenSession.AccessToken = respToken.access_token;

                tokenSession.TokenExpirationDate = DateTime.Now.AddSeconds(respToken.expires_in);

                return respToken;
            }
            else
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        throw new ClientIdSecretException(tokenSession.ClientId, tokenSession.ClientSecret);

                    case HttpStatusCode.Forbidden:
                        throw new RefreshTokenException(tokenSession.RefreshToken);

                    case HttpStatusCode.NotImplemented:
                        ErrorToken errorToken = JsonConvert.DeserializeObject<ErrorToken>((response.Content.ReadAsStringAsync().Result));
                        throw new ApiErrorResponseException(errorToken.error, errorToken.error_description);

                    default:
                        throw new HttpRequestException();
                }
            }

        }

        /// <summary>
        /// Test if my token is valid and if i'm authenticated.. Refer to : <see href="https://api.voxity.fr/doc/#api-Token-GetStatus"/>
        /// </summary>
        /// <returns><see cref="TokenStatus"/> object.</returns>
        public TokenStatus AuthenticationStatus()
        {
            HttpResponseMessage response = tokenSession.Request(ApiSession.HttpMethod.Get, "oauth/status");

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<TokenStatus>(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        //TODO : Check delta of access token. If 0 -> throw ExpireAccessToken, else BadAccessToken
                        throw new AccessTokenException(tokenSession.AccessToken);

                    case HttpStatusCode.InternalServerError:
                        throw new ApiSessionException("InternalServerError. Please contact Voxity support.", innerException: new HttpRequestException());

                    default:
                        throw new HttpRequestException();
                }
            }
        }

    }
}
