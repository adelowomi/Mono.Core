using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Mono.Core.Miscellaneous
{
    public class InstitutionResponseModel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("icon")]
        public Uri Icon { get; set; }

        [JsonPropertyName("coverage")]
        public Coverage Coverage { get; set; }

        [JsonPropertyName("products")]
        public List<string> Products { get; set; }

        [JsonPropertyName("website")]
        public string Website { get; set; }
    }

    public class Coverage
    {
        [JsonPropertyName("countries")]
        public List<string> Countries { get; set; }

        [JsonPropertyName("business")]
        public bool Business { get; set; }

        [JsonPropertyName("personal")]
        public bool Personal { get; set; }
    }
}
