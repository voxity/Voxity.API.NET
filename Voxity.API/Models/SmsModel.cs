
namespace Voxity.API.Models
{
    /// <summary/>
    public class NewSms
    {
        /// <summary/>
        public string phone_number { get; set; }
        /// <summary/>
        public string content { get; set; }
        /// <summary/>
        public string id { get; set; }
        /// <summary/>
        public string send_date { get; set; }
        /// <summary/>
        public string status { get; set; }
        /// <summary/>
        public string nb_sms { get; set; }
        /// <summary/>
        public string code { get; set; }
        /// <summary/>
        public string code_message { get; set; }
    }

    /// <summary/>
    public class EmitSms
    {
        /// <summary/>
        public string id { get; set; }
        /// <summary/>
        public string send_date { get; set; }
        /// <summary/>
        public string phone_number { get; set; }
        /// <summary/>
        public string content { get; set; }
        /// <summary/>
        public string nb_sms { get; set; }
        /// <summary/>
        public string status { get; set; }
        /// <summary/>
        public string delivery_date { get; set; }
        /// <summary/>
        public string code { get; set; }
        /// <summary/>
        public string code_message { get; set; }
        /// <summary/>
        public string statut { get; set; }
        /// <summary/>
        public string libelle { get; set; }
        /// <summary/>
        public string code_erreur { get; set; }
        /// <summary/>
        public string operateur { get; set; }
    }

    /// <summary/>
    public class DeleteEmitSms
    {
        /// <summary/>
        public string status { get; set; }
        /// <summary/>
        public string result { get; set; }
    }

    /// <summary/>
    public class SmsResponses
    {
        /// <summary/>
        public string id { get; set; }
        /// <summary/>
        public string id_sms_sent { get; set; }
        /// <summary/>
        public string send_date { get; set; }
        /// <summary/>
        public string phone_number { get; set; }
        /// <summary/>
        public string content { get; set; }
    }


    /// <summary/>
    public class DeleteResponseSms
    {
        /// <summary/>
        public string status { get; set; }
        /// <summary/>
        public string result { get; set; }
    }

    /// <summary/>
    public class CreateSms
    {
        /// <summary/>
        public string phone_number { get; set; }
        /// <summary/>
        public string content { get; set; }
        /// <summary/>
        public string emitter { get; set; }
    }
}
