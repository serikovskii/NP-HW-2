using Newtonsoft.Json;
using System.Collections.Generic;

namespace NP_HW_Server.DTO
{
    public class Datum
    {
        [JsonProperty("parts")]
        public IList<Part> Parts { get; set; }

        
    }
}
