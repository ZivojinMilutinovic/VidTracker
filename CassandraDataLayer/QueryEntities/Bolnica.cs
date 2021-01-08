using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CassandraDataLayer.QueryEntities
{
    public class Bolnica
    {
		public int BolnicaID { get; set; }
		public string Naziv { get; set; }
		public string Adresa { get; set; }
		public string Grad { get; set; }
		public int BrojSlobodnihKreveta { get; set; }
		public int BrojLjudiNaRespiratoru { get; set; }
		public int BrojZarazenihLekara { get; set; }
	}
}
