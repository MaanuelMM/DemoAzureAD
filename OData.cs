using System;
using Newtonsoft.Json;

namespace DemoAzureAD
{
    public class OData
    {
        [JsonProperty("odata.context")]
        public string Metadata { get; set; }
        public Object Value { get; set; }
    }
}
