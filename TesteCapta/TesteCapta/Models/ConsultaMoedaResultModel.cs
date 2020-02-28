using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TesteCapta.Models
{
    public class ConsultaMoedaResultModel
    {
        [JsonProperty("value")]
        public List<Value> Values { get; set; }
    }
}
