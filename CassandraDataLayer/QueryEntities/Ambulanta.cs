using Cassandra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CassandraDataLayer.QueryEntities
{
  public  class Ambulanta
    {
		public int Ambulanta_id { get; set; }
        public string Naziv { get; set; }
        public string Adresa { get; set; }
        public string Grad { get; set; }
        public LocalDate Dan { get; set; }
        public int BrojTestiranih { get; set; }
        public int BrojPregledanih { get; set; }
        public int BrojLekara { get; set; }
        public int BrojPozitivnih { get; set; }

    }
	
}
