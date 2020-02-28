using System;
using System.Collections.Generic;
using System.Text;

namespace TesteCapta.Services
{
    public class HttpUtils
    {
        public static string CONTENT_TYPE = "application/json";

        /** URL **/
        public static string URL_DEFAULT = "https://olinda.bcb.gov.br/olinda/servico/PTAX/versao/v1/odata";

        /** End Points **/
        public static string CONSULTA_MOEDAS = $"{URL_DEFAULT}/Moedas?";

    }
}
