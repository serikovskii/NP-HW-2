using Newtonsoft.Json;
using System.Collections.Generic;

namespace NP_HW_Server.DTO
{
    public class Example
    {

        [JsonProperty("data")]
        public IList<Datum> Data { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("from")]
        public int From { get; set; }
    }
}
