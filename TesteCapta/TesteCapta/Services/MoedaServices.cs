using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TesteCapta.Models;

namespace TesteCapta.Services
{
    public class MoedaServices
    {
        public async Task<ConsultaMoedaResultModel> ConsultaMoedasAsync(int qtdMax)
        {
            ConsultaMoedaResultModel moedaList = new ConsultaMoedaResultModel();

            HttpClient oHttpClient = new HttpClient();
            oHttpClient.Timeout = new TimeSpan(0, 1, 0);

            var url = string.Format("{0}top={1}", HttpUtils.CONSULTA_MOEDAS, qtdMax);

            var ret = await oHttpClient.GetAsync(url);

            if (ret.IsSuccessStatusCode)
            {
                var request = ret.Content.ReadAsStringAsync().Result;
                var a = JObject.Parse(request);
                moedaList = a.ToObject<ConsultaMoedaResultModel>();
            }

            return moedaList;
        }
    }
}
