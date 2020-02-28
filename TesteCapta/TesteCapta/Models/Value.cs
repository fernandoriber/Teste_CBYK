using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TesteCapta.Models
{
    public class Value
    {
        [JsonProperty("simbolo")]
        public string Simbolo { get; set; }

        [JsonProperty("nomeFormatado")]
        public string NomeFormatado { get; set; }

        [JsonProperty("tipoMoeda")]
        public string TipoMoeda { get; set; }
    }
}
