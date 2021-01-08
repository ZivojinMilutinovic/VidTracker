using Cassandra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CassandraDataLayer.QueryEntities
{
    public class Statistika
    {
        public string Danasnje_mere { get; set; }
        public LocalDate Dan { get; set; }
        public int BrojTestiranih { get; set; }
        public int BrojPregledanih { get; set; }
        public int BrojPozitivnih { get; set; }
    }
}
