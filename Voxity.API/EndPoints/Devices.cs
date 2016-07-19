using Newtonsoft.Json;
using System.Net;
using System.Collections.Generic;
using System.Net.Http;
using Voxity.API.Models;

namespace Voxity.API.EndPoints
{
    /// <summary>
    /// 
    /// </summary>
    public class Devices
    {
        private IApiSession deviceSession;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        public Devices(IApiSession session)
        {
            deviceSession = session;
        }

        /// <summary>
        /// List all devices and gave, for each one status of it. Status can be ringing, available, unavailable, in-use. Important note : Only admin user can be access to all devices of a pool. Refer to : <see href="https://api.voxity.fr/doc/#api-Device-ListDevice"/>
        /// </summary>
        /// <returns>List of <see cref="Device"/> object.</returns>
        public List<Device> DeviceList()
        {
            HttpResponseMessage response = deviceSession.Request(ApiSession.HttpMethod.Get, "devices");

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResponse<List<Device>>>(response.Content.ReadAsStringAsync().Result).Results();
            }
            else
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        throw new ApiAuthenticateException();

                    case HttpStatusCode.InternalServerError:
                        throw new ApiInternalErrorException();

                    default:
                        throw new HttpRequestException();
                }
            }
        }

        /// <summary>
        /// [BETA] Get one device by ID and gave status of it. Status can be ringing, available, unavailable, in-use. refer to : <see href="https://api.voxity.fr/doc/#api-Device-GetDevice"/>
        /// </summary>
        /// <param name="id">Device ID</param>
        /// <returns><see cref="Device"/> object.</returns>
        public Device DeviceById(string id)
        {
            HttpResponseMessage response = deviceSession.Request(ApiSession.HttpMethod.Get, "devices/" + id);

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResponse<Device>>(response.Content.ReadAsStringAsync().Result).Results();
            }
            else
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        throw new ApiAuthenticateException();

                    case HttpStatusCode.InternalServerError:
                        throw new ApiInternalErrorException();

                    default:
                        throw new HttpRequestException();
                }
            }
        }
    }
}
