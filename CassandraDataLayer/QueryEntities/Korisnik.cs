using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CassandraDataLayer.QueryEntities
{
  public  class Korisnik
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Rola_Id { get; set; }
        public string Grad { get; set; }
        public int IdRadnogMesta { get; set; }
    }
}
