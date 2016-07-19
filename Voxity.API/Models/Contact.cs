
namespace Voxity.API.Models
{
    /// <summary/>
    public class ManageContact
    {
        /// <summary/>
        public string uid { get; set; }
    }

    /// <summary/>
    public class Contact
    {
        /// <summary/>
        public string cn { get; set; }
        /// <summary/>
        public string description { get; set; }
        /// <summary/>
        public string info { get; set; }
        /// <summary/>
        public string mail { get; set; }
        /// <summary/>
        public string mobile { get; set; }
        /// <summary/>
        public string telephoneNumber { get; set; }
        /// <summary/>
        public string uid { get; set; }

        // Details contact
        /// <summary/>
        public string phoneNumberRaccourci { get; set; }
        /// <summary/>
        public string employeeNumber { get; set; }
    }


    /// <summary/>
    public class ContactName
    {
        /// <summary/>
        public string name { get; set; }
        /// <summary/>
        public string surname { get; set; }
    }
}
