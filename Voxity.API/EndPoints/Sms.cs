using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using Voxity.API.Models;

namespace Voxity.API.EndPoints
{
    /// <summary>
    /// 
    /// </summary>
    public class Sms
    {
        private IApiSession smsSession;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        public Sms(IApiSession session)
        {
            smsSession = session;
        }

        /// <summary>
        /// Send SMS. Refer to : <see href="https://api.voxity.fr/doc/#api-SMS-SendSms"/>
        /// </summary>
        /// <param name="content">Message content</param>
        /// <param name="phoneNumber">[FORMAT: /^[0-9]{ 10}$/] Phone number for the message receiver</param>
        /// <param name="emitter">(Optional) [FORMAT: /^([a-z]|[A-Z]){4,11}$/] The emitter's name. Be aware that the receiver cannot respond to the message if this field is specified.</param>
        /// <returns><see cref="NewSms"/> object.</returns>
        public NewSms SendMessage(string content, string phoneNumber, string emitter = null)
        {
            if (Utils.Filter.ValidPhone(phoneNumber) != true || Utils.Filter.ValidRac(phoneNumber) != true)
                throw new ApiSessionException("Invalid phone number.");

            CreateSms cs = new CreateSms();

            if (!string.IsNullOrWhiteSpace(phoneNumber))
                cs.phone_number = phoneNumber;

            if (!string.IsNullOrWhiteSpace(content))
                cs.content = content;

            if (!string.IsNullOrWhiteSpace(emitter))
                cs.emitter = emitter;

            HttpResponseMessage response = smsSession.Request(ApiSession.HttpMethod.Post, "sms", contentType: "application/json", contentValue: JsonConvert.SerializeObject(cs));

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResponse<NewSms>>(response.Content.ReadAsStringAsync().Result).Results();
            }
            else
            {
                switch (response.StatusCode)
                {
                    default:
                        throw new HttpRequestException();
                }
            }
        }

        /// <summary>
        /// Get list of emit messages (SMS). Refert to : <see href="https://api.voxity.fr/doc/#api-SMS-GetMessages"/>
        /// </summary>
        /// <param name="status">(Optional) All messages with the given status (Values : {PENDING,DELIVERED,ERROR})</param>
        /// <param name="sendDate">(Optional) The date of the sending</param>
        /// <param name="phoneNumber">(Optional) [FORMAT: /^[0-9]{10}$/] The receiver phone number</param>
        /// <returns>List of <see cref="EmitSms"/> object (SMS).</returns>
        public List<EmitSms> EmitMessageList(string status = null, string sendDate = null, string phoneNumber = null)
        {
            HttpResponseMessage response = smsSession.Request(ApiSession.HttpMethod.Get, "sms");

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResponse<List<EmitSms>>>(response.Content.ReadAsStringAsync().Result).Results();
            }
            else
            {
                switch (response.StatusCode)
                {
                    default:
                        throw new HttpRequestException();
                }
            }
        }

        /// <summary>
        /// Get infos of one emit message by ID. Refer to : <see href="https://api.voxity.fr/doc/#api-SMS-GetMessage"/>
        /// </summary>
        /// <param name="msgId">The message id returned after the sending.</param>
        /// <returns><see cref="EmitSms"/> object (SMS).</returns>
        public EmitSms EmitMessageById(string msgId)
        {
            HttpResponseMessage response = smsSession.Request(ApiSession.HttpMethod.Get, "sms/" + msgId);

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResponse<EmitSms>>(response.Content.ReadAsStringAsync().Result).Results();
            }
            else
            {
                switch (response.StatusCode)
                {
                    default:
                        throw new HttpRequestException();
                }
            }
        }

        /// <summary>
        /// [BETA] Deletes the emitted message by ID and all its responses. Refer to : <see href="https://api.voxity.fr/doc/#api-SMS-GetVms"/>
        /// </summary>
        /// <param name="msgId">The message id returned after the sending</param>
        /// <returns><see cref="DeleteEmitSms"/> object (SMS).</returns>
        public DeleteEmitSms DeleteMessage(string msgId)
        {
            HttpResponseMessage response = smsSession.Request(ApiSession.HttpMethod.Delete, "sms/" + msgId);

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResponse<DeleteEmitSms>>(response.Content.ReadAsStringAsync().Result).Results();
            }
            else
            {
                switch (response.StatusCode)
                {
                    default:
                        throw new HttpRequestException();
                }
            }
        }

        /// <summary>
        /// [BETA] Get one message response of one emit message by ID. Refer to : <see href="https://api.voxity.fr/doc/#api-SMS-GetSmsResponses"/>
        /// </summary>
        /// <param name="msgEmitId">The sms id returned after the sending</param>
        /// <returns><see cref="SmsResponses">Response</see> message of one emit <see cref="EmitSms">message</see>.</returns>
        public SmsResponses ResponseOfEmitId(string msgEmitId)
        {
            HttpResponseMessage response = smsSession.Request(ApiSession.HttpMethod.Get, "sms/" + msgEmitId + "/responses");

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResponse<SmsResponses>>(response.Content.ReadAsStringAsync().Result).Results();
            }
            else
            {
                switch (response.StatusCode)
                {
                    default:
                        throw new HttpRequestException();
                }
            }
        }

        /// <summary>
        /// [BETA] Get one message response by ID. Refer to : <see href="https://api.voxity.fr/doc/#api-SMS-GetResponse"/>
        /// </summary>
        /// <param name="respId">The response id</param>
        /// <returns><see cref="SmsResponses"/> object.</returns>
        public SmsResponses ResponseById(string respId)
        {
            HttpResponseMessage response = smsSession.Request(ApiSession.HttpMethod.Get, "sms/responses/" + respId);

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResponse<SmsResponses>>(response.Content.ReadAsStringAsync().Result).Results();
            }
            else
            {
                switch (response.StatusCode)
                {
                    default:
                        throw new HttpRequestException();
                }
            }
        }

        /// <summary>
        /// Get list of all response messages (SMS). Refert to : <see href="https://api.voxity.fr/doc/#api-SMS-GetResponses"/>
        /// </summary>
        /// <returns>List of <see cref="SmsResponses"/> object.</returns>
        public List<SmsResponses> ResponsesMessagesList()
        {
            HttpResponseMessage response = smsSession.Request(ApiSession.HttpMethod.Get, "sms/responses");

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResponse<List<SmsResponses>>>(response.Content.ReadAsStringAsync().Result).Results();
            }
            else
            {
                switch (response.StatusCode)
                {
                    default:
                        throw new HttpRequestException();
                }
            }
        }

        /// <summary>
        /// [BETA] Delete response message (SMS) by ID. Refert to : <see href="https://api.voxity.fr/doc/#api-SMS-DeleteResponse"/>
        /// </summary>
        /// <param name="resp_id">The response message id</param>
        /// <returns><see cref="DeleteResponseSms"/> object.</returns>
        public DeleteResponseSms DeleteResponse(string resp_id)
        {
            HttpResponseMessage response = smsSession.Request(ApiSession.HttpMethod.Delete, "sms/responses/:id" + resp_id);

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResponse<DeleteResponseSms>>(response.Content.ReadAsStringAsync().Result).Results();
            }
            else
            {
                switch (response.StatusCode)
                {
                    default:
                        throw new HttpRequestException();
                }
            }
        }
    }
}