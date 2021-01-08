using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CassandraDataLayer.QueryEntities
{
    public class Pacijent
    {
		public int BrojKnjizice { get; set; }
		public string Ime { get; set; }
		public string Prezime { get; set; }
		public int Godine { get; set; }
		public DateTimeOffset TestTimestamp { get; set; }
		public bool Pozitivan { get; set; }
	}
}
