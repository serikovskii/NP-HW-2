using Newtonsoft.Json;

namespace NP_HW_Server.DTO
{
    public class Part
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("nameRus")]
        public string NameRus { get; set; }

    }

}
