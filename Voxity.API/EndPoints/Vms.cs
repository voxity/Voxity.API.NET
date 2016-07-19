using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using Voxity.API.Models;

namespace Voxity.API.EndPoints
{
    /// <summary>
    /// 
    /// </summary>
    public class Vms
    {

        private IApiSession vmsSession;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        public Vms(IApiSession session)
        {
            vmsSession = session;
        }

        /// <summary>
        /// [BETA] Upload a vocal message in wave/wav/wma format, 5 megabytes max.
        /// Refer to : <see href="https://api.voxity.fr/doc/#api-VMS-UploadFile"/>
        /// </summary>
        /// <param name="file">The sound file to upload. Be aware that the file must not exceed 5 megabytes.
        ///     Example : <Example><c>@/home/path/to/my/file.wav;type=audio/wav</c></Example>
        /// </param>
        /// <param name="description">A file description. Example : <Example>My file</Example></param>
        /// <returns><see cref="UploadVms"/> object.</returns>
        public UploadVms UploadSoundFile(string file, string description)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("uploadedfile", file);
            values.Add("description", description);

            HttpResponseMessage response = vmsSession.Request(ApiSession.HttpMethod.Post, "vms/files", urlParams: values);

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<UploadVms>(response.Content.ReadAsStringAsync().Result);
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
        /// [BETA] Get list of files
        /// Refer to : <see href="https://api.voxity.fr/doc/#api-VMS-GetFileIds"/>
        /// </summary>
        /// <returns>List of <see cref="UploadVms"/> object.</returns>
        public List<VmsFile> FilesList()
        {
            HttpResponseMessage response = vmsSession.Request(ApiSession.HttpMethod.Get, "vms/files");

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResponse<List<VmsFile>>>(response.Content.ReadAsStringAsync().Result).Results();
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
        /// [BETA] Get a file by ID
        /// Refer to : <see href="https://api.voxity.fr/doc/#api-VMS-GetFile"/>
        /// </summary>
        /// <param name="id">File ID</param>
        /// <returns><see cref="UploadVms"/> object.</returns>
        public VmsFile FileById(string id)
        {
            HttpResponseMessage response = vmsSession.Request(ApiSession.HttpMethod.Get, "vms/files/" + id);

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResponse<VmsFile>>(response.Content.ReadAsStringAsync().Result).Results();
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

        //optionnal filename, description
        /// <summary>
        /// [BETA] Modify a file by ID
        /// Refer to : <see href="https://api.voxity.fr/doc/#api-VMS-GetFile"/>
        /// </summary>
        /// <param name="id">File ID</param>
        /// <param name="filename">(Optional) new file name</param>
        /// <param name="description">(Optional) new file description</param>
        /// <returns><see cref="ManageVms"/> object.</returns>
        public ManageVms ModifyFile(string id, string filename = null, string description = null)
        {
            VmsFile vf = new VmsFile();

            if (!string.IsNullOrWhiteSpace(id))
                vf.id = id;

            if (!string.IsNullOrWhiteSpace(filename))
                vf.filename = filename;

            if (!string.IsNullOrWhiteSpace(description))
                vf.description = description;

            if (vf.id == null)
                throw new ApiSessionException("File ID invalid.");

            HttpResponseMessage response = vmsSession.Request(ApiSession.HttpMethod.Put, "vms/files/" + id, contentType: "application/json", contentValue: JsonConvert.SerializeObject(vf));

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ManageVms>(response.Content.ReadAsStringAsync().Result);
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
        /// [BETA] Delete a file by ID
        /// Refer to : <see href="https://api.voxity.fr/doc/#api-VMS-DeleteFile"/>
        /// </summary>
        /// <param name="id">File ID</param>
        /// <returns><see cref="ManageVms"/> object.</returns>
        public ManageVms DeleteFile(string id)
        {
            HttpResponseMessage response = vmsSession.Request(ApiSession.HttpMethod.Delete, "vms/files/" + id);

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ManageVms>(response.Content.ReadAsStringAsync().Result);
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
        /// [BETA] Send a vocal message.
        /// Refer to : <see href="https://api.voxity.fr/doc/#api-VMS-SendVms"/>
        /// </summary>
        /// <param name="fileId">The sound file id. See : <see cref="Vms.UploadSoundFile(string, string)"/></param>
        /// <param name="phoneNumber">[FORMAT: /^[0-9]{10}$/] The receiver's phone number</param>
        /// <param name="emitter">The emitter's phone number</param>
        /// <param name="typeCall">Type of message : 0 --> for a phone call message | 1 --> for a vocal message on the answering machine</param>
        /// <returns><see cref="ConvVms"/> object.</returns>
        public ConvVms SendMessage(string fileId, string phoneNumber, string emitter, string typeCall)
        {
            if (Utils.Filter.ValidPhone(phoneNumber) != true || Utils.Filter.ValidRac(phoneNumber) != true)
                throw new ApiSessionException("Invalid phone number.");

            ConvVms cv = new ConvVms();

            if (!string.IsNullOrWhiteSpace(fileId))
                cv.id = fileId;
            else
                throw new ApiSessionException("File ID invalid.");

            if (!string.IsNullOrWhiteSpace(phoneNumber))
                cv.phone_number = phoneNumber;
            else
                throw new ApiSessionException("File phone_number invalid.");

            if (!string.IsNullOrWhiteSpace(emitter))
                cv.emitter = emitter;
            else
                throw new ApiSessionException("File emitter invalid.");
            if (!string.IsNullOrWhiteSpace(typeCall))
                cv.type_call = typeCall;
            else
                throw new ApiSessionException("File type_call invalid.");
                

            HttpResponseMessage response = vmsSession.Request(ApiSession.HttpMethod.Post, "vms", contentType: "application/json", contentValue: JsonConvert.SerializeObject(cv));

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResponse<ConvVms>>(response.Content.ReadAsStringAsync().Result).Results();
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
        /// [BETA] Get vocal message.
        /// Refer to : <see href="https://api.voxity.fr/doc/#api-VMS-GetVms"/>
        /// </summary>
        /// <param name="id">(Optional) The message id returned after the sending</param>
        /// <param name="sendDate">(Optional) The date of the sending</param>
        /// <param name="phoneNumber">(Optional) [FORMAT: /^[0-9]{10}$/] The receiver phone number</param>
        /// <param name="status">(Optional) Gives information on the message status. Values {PENDING,DELIVERED,ERROR}</param>
        /// <param name="typeCall">(Optional) type_call: 0 for a call, type_call: 1 for a message on the messaging</param>
        /// <param name="emitter">(Optional) [FORMAT: /^[0-9]{10}$/] The phone number emitter</param>
        /// <returns>List of <see cref="ConvVms"/> object.</returns>
        public List<ConvVms> MessagesList(string id = null, string sendDate = null, string phoneNumber = null, string status = null, string typeCall = null, string emitter = null)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("id", id);
            values.Add("send_date", sendDate);
            values.Add("phone_number", phoneNumber);
            values.Add("status", status);
            values.Add("type_call", typeCall);
            values.Add("emitter", emitter);

            HttpResponseMessage response = vmsSession.Request(ApiSession.HttpMethod.Get, "vms", urlParams: values);

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResponse<List<ConvVms>>>(response.Content.ReadAsStringAsync().Result).Results();
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
