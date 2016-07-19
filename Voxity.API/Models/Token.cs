
namespace Voxity.API.Models
{
    /// <summary/>
    public class AccessToken
    {
        /// <summary/>
        public string access_token { get; set; }
        /// <summary/>
        public string refresh_token { get; set; }
        /// <summary/>
        public int expires_in { get; set; }
        /// <summary/>
        public string token_type { get; set; }
    }

    /// <summary/>
    public class RefreshToken
    {
        /// <summary/>
        public string access_token { get; set; }
        /// <summary/>
        public int expires_in { get; set; }
        /// <summary/>
        public string token_type { get; set; }
    }

    /// <summary/>
    public class TokenStatus
    {
        /// <summary/>
        public string message { get; set; }
    }

    /// <summary/>
    public class ErrorToken
    {
        /// <summary/>
        public string error { get; set; }
        /// <summary/>
        public string error_description { get; set; }
    }
}
