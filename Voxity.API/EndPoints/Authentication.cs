using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System;
using System.Runtime.InteropServices;

namespace Voxity.API.EndPoints
{
    /// <summary>
    /// 
    /// </summary>
    public class Authentication
    {
        private IApiSession authenticationSession;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        public Authentication(IApiSession session)
        {
            authenticationSession = session;
        }

        /// <summary>
        /// [BETA] Authentificate user. Log user designated by username and password provided in params. Refer to : <see href="https://api.voxity.fr/doc/#api-Authentication-PostLogin"/>
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns>
        /// <c>HTTP 302 Redirect /</c> if success
        /// <c>HTTP 302 Redirect /login</c> if error
        /// </returns>
        public void Authenticate(string user, System.Security.SecureString password)
        {
            string decrypt = null;

            if (password == null)
                throw new ArgumentNullException("securePassword");

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(password);
                decrypt = Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }

            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("username", user);
            values.Add("password", decrypt);

            HttpResponseMessage response = authenticationSession.Request(ApiSession.HttpMethod.Post, "login", urlParams: values, isTokenRequired: false);

            switch (response.StatusCode)
            {
                case HttpStatusCode.Redirect:
                    //TODO : Redirect to / if credential valid, /login if bad
                    break;

                default:
                    throw new HttpRequestException(response.StatusCode.ToString());
            }
        }

        /// <summary>
        /// [BETA] Check authentification of current user. Refer to : <see href="https://api.voxity.fr/doc/#api-Authentication-GetLoginStatus"/>
        /// </summary>
        /// <returns>
        /// <c>HTTP/1.1 200 OK /</c> if success
        /// <c>HTTP/1.1 401</c> if error
        /// </returns>
        public bool CheckUserLogged()
        {
            HttpResponseMessage response = authenticationSession.Request(ApiSession.HttpMethod.Get, "login/status", isTokenRequired: false);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return true;

                case HttpStatusCode.Unauthorized:
                    return false;

                default:
                    throw new HttpRequestException();
            }
        }

        // Logout current user
        /// <summary>
        /// [BETA] Logout user designated by token. This token will be deleted. Refer to <see href="https://api.voxity.fr/doc/#api-Authentication-GetLogout"/>
        /// </summary>
        /// <returns><c>HTTP 301 Redirect</c> if success</returns>
        public void Logout()
        {
            HttpResponseMessage response = authenticationSession.Request(ApiSession.HttpMethod.Get, "logout");

            switch (response.StatusCode)
            {
                case HttpStatusCode.Redirect:
                    // TODO : Clean ApiSession Tokens.
                    break;

                default:
                    throw new HttpRequestException(response.StatusCode.ToString());
            }
        }
    }
}
