using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TesteCapta.Models
{
    public class MoedaDataBaseModel
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Simbolo { get; set; }

        public string NomeFormatado { get; set; }

        public string TipoMoeda { get; set; }

        public DateTime Date { get; set; }
    }
}
