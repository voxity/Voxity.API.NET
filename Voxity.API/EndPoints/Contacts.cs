using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Voxity.API.Models;

namespace Voxity.API.EndPoints
{
    /// <summary>
    /// 
    /// </summary>
    public class Contacts
    {
        private IApiSession contactSession;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        public Contacts(IApiSession session)
        {
            contactSession = session;
        }

        /// <summary>
        /// Create a contact. Refer to : <see href="https://api.voxity.fr/doc/#api-Contact-CreateContact" />
        /// </summary>
        /// <param name="cn">Contact's name</param>
        /// <param name="telNum">Contact's phone number</param>
        /// <param name="telNumRac">[FORMAT: /^*[0-9]{1,4}$/] Phone number shortcut. Requirement <c>telNum</c></param>
        /// <param name="mobile">Contact's mobile number</param>
        /// <param name="racMobile">[FORMAT: /^*[0-9]{1,4}$/] Mobile phone number shortcut. Requirement <c>mobile</c></param>
        /// <param name="mail">Contact's mail address</param>
        /// <returns><see cref="ManageContact"/> object</returns>
        public ManageContact CreateContact(string cn, string telNum = null, string telNumRac = null, string mobile = null, string racMobile = null, string mail = null)
        {
            Contact c = new Contact();

            if (!string.IsNullOrWhiteSpace(cn))
                c.cn = cn;
            else
                throw new ApiSessionException("[Voxity API] - Create Contact\nContact name is null or empty, please give a valid contact name.");

            if (!string.IsNullOrWhiteSpace(telNum))
            {
                if (Utils.Filter.ValidPhone(telNum) != true)
                    throw new ApiSessionException("Invalid phone number.");
                else
                    c.telephoneNumber = telNum;
            }

            if (!string.IsNullOrWhiteSpace(telNumRac))
            {
                if (Utils.Filter.ValidRac(telNumRac) != true)
                    throw new ApiSessionException("Invalid phone alias.");
                else
                    c.phoneNumberRaccourci = telNumRac;
            }

            if (!string.IsNullOrWhiteSpace(mobile))
            {
                if (Utils.Filter.ValidPhone(mobile) != true)
                    throw new ApiSessionException("Invalid mobile number.");
                else
                    c.mobile = mobile;
            }

            if (!string.IsNullOrWhiteSpace(racMobile))
            {
                if (Utils.Filter.ValidRac(racMobile) != true)
                    throw new ApiSessionException("Invalid mobile alias.");
                else
                    c.employeeNumber = racMobile;
            }

            if (!string.IsNullOrWhiteSpace(mail))
                c.mail = mail;

            if (c.telephoneNumber == null && c.mobile == null)
                throw new ApiSessionException("[Voxity API] - Create Contact\nNo phone number and mobile set. Please add one of them.");


            HttpResponseMessage response = contactSession.Request(ApiSession.HttpMethod.Post, "contacts", contentType: "application/json", contentValue: JsonConvert.SerializeObject(c, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

            if (response.IsSuccessStatusCode)
            {
                // Affect access_token, refresh_token values
                return JsonConvert.DeserializeObject<ApiResponse<ManageContact>>(response.Content.ReadAsStringAsync().Result).Results();
            }
            else
            {
                switch (response.StatusCode)
                {
                    default:
                        throw new HttpRequestException(response.StatusCode.ToString());
                }
            }
        }

        /// <summary>
        /// List all contacts. Refer to : <see href="https://api.voxity.fr/doc/#api-Contact-GetContactList"/>
        /// </summary>
        /// <param name="phoneNumber">Applies a filter on phone number to the research</param>
        /// <param name="cn">Applies a filter on contact name to the research</param>
        /// <param name="mobile">Applies a filter on mobile number to the research</param>
        /// <returns>List of <see cref="Contact"/> object</returns>
        public List<Contact> ContactList(string phoneNumber = null, string cn = null, string mobile = null)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("phoneNumber", phoneNumber);
            values.Add("cn", cn);
            values.Add("mobile", mobile);

            HttpResponseMessage response = contactSession.Request(ApiSession.HttpMethod.Get, "contacts", urlParams: values);

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResponse<List<Contact>>>(response.Content.ReadAsStringAsync().Result).Results();
            }
            else
            {
                switch (response.StatusCode)
                {
                    default:
                        throw new HttpRequestException(response.StatusCode.ToString());
                }
            }
        }

        /// <summary>
        /// Get one contact info. Refer to : <see href="https://api.voxity.fr/doc/#api-Contact-GetContact"/>
        /// </summary>
        /// <param name="uid">Contact unique id</param>
        /// <returns><see cref="Contact"/> object</returns>
        public Contact ContactById(string uid)
        {
            HttpResponseMessage response = contactSession.Request(ApiSession.HttpMethod.Get, "contacts/" + uid);

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResponse<Contact>>(response.Content.ReadAsStringAsync().Result).Results();
            }
            else
            {
                switch (response.StatusCode)
                {
                    default:
                        throw new HttpRequestException(response.StatusCode.ToString());
                }
            }
        }

        /// <summary>
        /// Update a contact. Refer to : <see href="https://api.voxity.fr/doc/#api-Contact-UpdateContact"/>
        /// </summary>
        /// <param name="uid">Contact unique id</param>
        /// <param name="cn">Contact's name</param>
        /// <param name="telNum">Contact's name</param>
        /// <param name="telNumRac">[FORMAT: /^*[0-9]{1,4}$/] Phone number shortcut. Requirement <c>telNum</c></param>
        /// <param name="mobile">Contact's mobile number</param>
        /// <param name="racMobile">[FORMAT: /^*[0-9]{1,4}$/] Mobile phone number shortcut. Requirement <c>mobile</c></param>
        /// <param name="mail">Contact's mail address</param>
        /// <returns><see cref="ManageContact"/> object</returns>
        public ManageContact UpdateContact(string uid, string cn, string telNum, string telNumRac = null, string mobile = null, string racMobile = null, string mail = null)
        {
            Contact c = new Contact();

            if (!string.IsNullOrWhiteSpace(cn))
                c.uid = uid;
            else
                throw new ApiSessionException("[Voxity API] - Create Contact\nContact ID is null or empty, please give a valid contact ID.");

            if (!string.IsNullOrWhiteSpace(cn))
                c.cn = cn;
            else
                throw new ApiSessionException("[Voxity API] - Create Contact\nContact name is null or empty, please give a valid contact name.");

            if (!string.IsNullOrWhiteSpace(telNum))
            {
                if (Utils.Filter.ValidPhone(telNum) != true)
                    throw new ApiSessionException("Invalid phone number.");
                else
                    c.telephoneNumber = telNum;
            }

            if (!string.IsNullOrWhiteSpace(telNumRac))
            {
                if (Utils.Filter.ValidRac(telNumRac) != true)
                    throw new ApiSessionException("Invalid phone alias.");
                else
                    c.phoneNumberRaccourci = telNumRac;
            }

            if (!string.IsNullOrWhiteSpace(mobile))
            {
                if (Utils.Filter.ValidPhone(mobile) != true)
                    throw new ApiSessionException("Invalid mobile number.");
                else
                    c.mobile = mobile;
            }

            if (!string.IsNullOrWhiteSpace(racMobile))
            {
                if (Utils.Filter.ValidRac(racMobile) != true)
                    throw new ApiSessionException("Invalid mobile alias.");
                else
                    c.employeeNumber = racMobile;
            }

            if (!string.IsNullOrWhiteSpace(mail))
                c.mail = mail;


            if (c.telephoneNumber == null && c.mobile == null)
                throw new ApiSessionException("[Voxity API] - Create Contact\nNo phone number and mobile set. Please add one of them.");

            HttpResponseMessage response = contactSession.Request(ApiSession.HttpMethod.Put, "contacts/" + uid, contentType: "application/json", contentValue: JsonConvert.SerializeObject(c, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResponse<ManageContact>>(response.Content.ReadAsStringAsync().Result).Results();
            }
            else
            {
                switch (response.StatusCode)
                {
                    default:
                        throw new HttpRequestException(response.StatusCode.ToString());
                }
            }
        }

        /// <summary>
        /// Delete a contact. Refer to : <see href="https://api.voxity.fr/doc/#api-Contact-DeleteContact"/>
        /// </summary>
        /// <param name="uid">Contact unique id</param>
        /// <returns><see cref="ManageContact"/> object</returns>
        public ManageContact DeleteContact(string uid)
        {
            HttpResponseMessage response = contactSession.Request(ApiSession.HttpMethod.Delete, "contacts/" + uid);
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResponse<ManageContact>>(response.Content.ReadAsStringAsync().Result).Results();
            }
            else
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        throw new ApiAdminException();

                    case HttpStatusCode.InternalServerError:
                        throw new ApiInternalErrorException();

                    default:
                        throw new HttpRequestException(response.StatusCode.ToString());
                }
            }
        }
    }
}
