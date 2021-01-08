using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CassandraDataLayer.QueryEntities
{
    public class AmbulantaTestovi
    {
        public int Ambulanta_id { get; set; }
        public string Grad { get; set; }
        public DateTimeOffset Test_timestamp { get; set; }
        public bool Pozitivan { get; set; }
    }
}
