
namespace Voxity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Log
    {
        /// <summary/>
        public string source { get; set; }
        /// <summary/>
        public string destination { get; set; }
        /// <summary/>
        public string sens { get; set; }
        /// <summary/>
        public string status { get; set; }
        /// <summary/>
        public string station { get; set; }
        /// <summary/>
        public string calldate { get; set; }
        /// <summary/>
        public string tag { get; set; }
        /// <summary/>
        public string duration { get; set; }
        /// <summary/>
        public string billsec { get; set; }
    }

    /// <summary/>
    public class NewChannel
    {
        /// <summary/>
        public string channel_id { get; set; }
    }

    /// <summary/>
    public class Channel
    {
        /// <summary/>
        public string id { get; set; }
        /// <summary/>
        public int channel_id { get; set; }
        /// <summary/>
        public string channel_state { get; set; }
        /// <summary/>
        public string channelstatedesc { get; set; }
        /// <summary/>
        public string caller_num { get; set; }
        /// <summary/>
        public string caller_name { get; set; }
        /// <summary/>
        public string exten { get; set; }
        /// <summary/>
        public bool originated_by_incomming_call { get; set; }
        /// <summary/>
        public bool is_external_channel { get; set; }
        /// <summary/>
        public bool has_music_onhold { get; set; }
        /// <summary/>
        public bool transfer_to { get; set; }
        /// <summary/>
        public bool transfer_type { get; set; }
        /// <summary/>
        public string protocol { get; set; }
    }

    /// <summary/>
    public class CreateChannel
    {
        /// <summary/>
        public string exten { get; set; }
    }

    /// <summary/>
    public class LogError
    {
        /// <summary/>
        public string param { get; set; }
        /// <summary/>
        public string msg { get; set; }
        /// <summary/>
        public string value { get; set; }
    }
}
