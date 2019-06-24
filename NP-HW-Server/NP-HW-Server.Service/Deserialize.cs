using Newtonsoft.Json;
using NP_HW_Server.DTO;
using System.Net;

namespace NP_HW_Server.Service
{
    public class Deserialize
    {
        public Example Execute(string postcode)
        {
            using(var webClient = new WebClient())
            {
                string json = webClient.DownloadString($"https://api.post.kz/api/byPostcode/{postcode}?from=0");
               
                var result = JsonConvert.DeserializeObject<Example>(json);

                return result;
            }
        }
    }
}
