using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using Voxity.API.Models;

namespace Voxity.API.EndPoints
{
    /// <summary>
    /// 
    /// </summary>
    public class Users
    {
        private IApiSession userSession;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        public Users(IApiSession session)
        {
            userSession = session;
        }

        /// <summary>
        /// This endpoint return information concerning current user. Refer to : <see href="https://api.voxity.fr/doc/#api-Users-UsersSelf"/>
        /// </summary>
        /// <returns>User object</returns>
        public User WhoAmI()
        {
            HttpResponseMessage response = userSession.Request(ApiSession.HttpMethod.Get, "users/self");

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResponse<User>>(response.Content.ReadAsStringAsync().Result).Results();
            }
            else
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        throw new HttpRequestException(response.StatusCode.ToString());

                    default:
                        throw new HttpRequestException(response.StatusCode.ToString());
                }
            }
        }
    }
}
