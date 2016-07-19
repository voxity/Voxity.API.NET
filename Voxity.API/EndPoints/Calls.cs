using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using Voxity.API.Models;

namespace Voxity.API.EndPoints
{
    /// <summary>
    /// 
    /// </summary>
    public class Calls
    {
        private IApiSession callSession;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        public Calls(IApiSession session)
        {
            callSession = session;
        }

        /// <summary>
        /// Call logs. Refer to : <see href="https://api.voxity.fr/doc/#api-Calls-GetCallLogs"/>
        /// </summary>
        /// <param name="fromDate">[FORMAT : YYYY-MM-DD HH:MM:SS] Date from which the research begins</param>
        /// <param name="toDate">[FORMAT : YYYY-MM-DD HH:MM:SS] Date to which the research ends</param>
        /// <param name="filterSrc">Caller number. Example : <c>String[] src = new String[] { "0627071995" };
        /// filter_src.AddRange(src);</c></param>
        /// <param name="filterDest">Called number. Example : <c>String[] dest = new String[] { "0627071995" };
        /// filter_dest.AddRange(dest);</c></param>
        /// <param name="sens">Applies a filter on the call sense : "incoming"|"outgoing"</param>
        /// <param name="lastDays">Retrieves all calls events of the X last_days. The use of this parameter unables the two other parameters : from_date, to_date.</param>
        /// <returns>List of <see cref="Log"/> object</returns>
        public List<Log> Logs(string fromDate = null, string toDate = null, StringCollection filterSrc = null, StringCollection filterDest = null, string sens = null, string lastDays = null)
        {
            string fSrc = null;
            string fDest = null;

            Dictionary<string, string> values = new Dictionary<string, string>();
            if (fromDate != null)
                values.Add("from_date", fromDate);
            if (toDate != null)
                values.Add("to_date", toDate);

            if (filterSrc != null)
            {
                foreach (string src in filterSrc)
                {
                    if (filterSrc.Count > 1)
                        fSrc += ",";
                    fSrc += "source==" + src;
                }
            }

            if (filterDest != null)
            {
                foreach (string dest in filterDest)
                {
                    if (filterDest.Count > 1)
                        fDest += ",";
                    fDest += "destination==" + dest;
                }
            }

            if (filterDest == null && filterSrc != null)
                values.Add("filter", fSrc);
            if (filterDest != null && filterSrc == null)
                values.Add("filter", fDest);
            if (filterDest != null && filterSrc != null)
                values.Add("filter", fSrc + "," + fDest);

            if (sens != null)
                values.Add("sens", sens);
            if (lastDays != null)
                values.Add("last_days", lastDays);

            HttpResponseMessage response = callSession.Request(ApiSession.HttpMethod.Get, "calls/logs", urlParams: values);

            if (response.IsSuccessStatusCode)
            {
                // Affect access_token, refresh_token values
                return JsonConvert.DeserializeObject<ApiResponse<List<Log>>>(response.Content.ReadAsStringAsync().Result).Results();
            }
            else
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        string msgError = null;

                        List<LogError> errors = JsonConvert.DeserializeObject<ApiResponse<List<LogError>>>(response.Content.ReadAsStringAsync().Result).Errors();

                        foreach(LogError error in errors)
                        {
                            switch (error.param)
                            {
                                case "from_date" :
                                    msgError += "Argument error : fromDate";
                                    break;

                                case "to_date":
                                    msgError += "Argument error : toDate";
                                    break;

                                case "filter":
                                    msgError += "Argument error : filterSrc/filterDest";
                                    break;

                                case "sens":
                                    msgError += "Argument error : sens";
                                    break;

                                case "last_days":
                                    msgError += "Argument error : last_days";
                                    break;
                            }

                            msgError += ", Description: " + error.msg + ", Value: " + error.value + Environment.NewLine;
                        }

                        throw new ApiArgumentException(msgError);

                    default:
                        throw new HttpRequestException();
                }
            }
        }

        /// <summary>
        /// create_channel : call the extention number phone. Refer to : <see href="https://api.voxity.fr/doc/#api-Calls-PostChannel"/>
        /// </summary>
        /// <param name="exten">Extention number. Example : "+33627071995"</param>
        /// <returns><see cref="NewChannel"/> Object</returns>
        public NewChannel CreateChannel (string exten)
        {
            if (Utils.Filter.ValidPhone(exten) != true && Utils.Filter.ValidRac(exten) != true)
                throw new ApiSessionException("Invalid phone number.");

            CreateChannel cc = new CreateChannel();
            cc.exten = exten;

            HttpResponseMessage response = callSession.Request(ApiSession.HttpMethod.Post, "channels", contentType: "application/json", contentValue: JsonConvert.SerializeObject(cc));

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResponse<NewChannel>>(response.Content.ReadAsStringAsync().Result).Results();
            }
            else
            {
                switch (response.StatusCode)
                {
                    case (HttpStatusCode)429 :
                        throw new ApiTooManyRequestException();

                    // This error can appear in some case of call.
                    case HttpStatusCode.InternalServerError:
                        throw new ApiInternalErrorException();

                    default:
                        throw new HttpRequestException(response.StatusCode.ToString());
                }
            }
        }

        /// <summary>
        /// [BETA] List all current channel for my account. Refer to : <see href="https://api.voxity.fr/doc/#api-Calls-ListChannel"/>
        /// </summary>
        /// <param name="channelId">Local channel id.</param>
        /// <returns>List of <see cref="Channel"/> object</returns>
        public List<Channel> ChannelList(string channelId = null)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();

            if (channelId != null)
                values.Add("from_date", channelId);

            HttpResponseMessage response = callSession.Request(ApiSession.HttpMethod.Get, "channels", urlParams: values);

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResponse<List<Channel>>>(response.Content.ReadAsStringAsync().Result).Results();
            }
            else
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        throw new ApiAdminException();

                    // Number of request is set (1 per 3 seconds). Wait a moment to re-send another request (per user).
                    case (HttpStatusCode)429:
                        throw new ApiTooManyRequestException();

                    default:
                        throw new HttpRequestException();
                }
            }
        }

        /// <summary>
        /// [BETA] Get information on one particular channel, selected by his id. Refer to : <see href="https://api.voxity.fr/doc/#api-Calls-GetChannel"/>
        /// </summary>
        /// <param name="channelId">Channel id.</param>
        /// <returns><see cref="Channel"/> object</returns>
        public Channel ChannelById(string channelId)
        {
            HttpResponseMessage response = callSession.Request(ApiSession.HttpMethod.Get, "channel/" + channelId);

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResponse<Channel>>(response.Content.ReadAsStringAsync().Result).Results();
            }
            else
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        throw new ApiAdminException();

                    // Number of request is set (1 per 3 seconds). Wait a moment to re-send another request (per user).
                    case (HttpStatusCode)429:
                        throw new ApiTooManyRequestException();

                    default:
                        throw new HttpRequestException();
                }
            }
        }
    }
}
