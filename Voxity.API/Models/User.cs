
namespace Voxity.API.Models
{
    /// <summary/>
    public class User
    {
        /// <summary/>
        public long id { get; set; }
        /// <summary/>
        public string last_login { get; set; }
        /// <summary/>
        public string name { get; set; }
        /// <summary/>
        public string mail { get; set; }
        /// <summary/>
        public string internalExtension { get; set; }
        /// <summary/>
        public string mobilePhoneNumber { get; set; }
        /// <summary/>
        public string otherPhoneNumber { get; set; }
        /// <summary/>
        public string description { get; set; }
        /// <summary/>
        public string country { get; set; }
        /// <summary/>
        public int is_active { get; set; }
        /// <summary/>
        public int is_admin { get; set; }
        /// <summary/>
        public string creation_date { get; set; }
        /// <summary/>
        public string telephoneNumber_id { get; set; }
    }
}
