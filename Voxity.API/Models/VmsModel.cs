
namespace Voxity.API.Models
{
    /// <summary/>
    public class UploadVms
    {
        /// <summary/>
        public uint status { get; set; }
        /// <summary/>
        public string errors { get; set; }
        /// <summary/>
        public string result { get; set; }
    }

    /// <summary/>
    public class VmsFile
    {
        /// <summary/>
        public string id { get; set; }
        /// <summary/>
        public string send_date { get; set; }
        /// <summary/>
        public string filename { get; set; }
        /// <summary/>
        public string description { get; set; }
    }

    /// <summary/>
    public class ManageVms
    {
        /// <summary/>
        public string status { get; set; }
        /// <summary/>
        public string result { get; set; }
    }

    /// <summary/>
    public class ConvVms
    {
        /// <summary/>
        public string id { get; set; }
        /// <summary/>
        public string send_date { get; set; }
        /// <summary/>
        public string phone_number { get; set; }
        /// <summary/>
        public string status { get; set; }
        /// <summary/>
        public string delivery_date { get; set; }
        /// <summary/>
        public string type_call { get; set; }
        /// <summary/>
        public string emitter { get; set; }
        /// <summary/>
        public string file_id { get; set; }
        /// <summary/>
        public string code { get; set; }
        /// <summary/>
        public string code_message { get; set; }
        /// <summary/>
        public string erreur { get; set; }
        /// <summary/>
        public string decroche { get; set; }
        /// <summary/>
        public string duree { get; set; }
    }
}
