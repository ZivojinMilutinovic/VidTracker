using Cassandra;
using CassandraDataLayer.QueryEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CassandraDataLayer
{
    public static class DataProvider
    {

        #region Pacijent
        public static Pacijent GetPacijent(int brojKnjizice)
        {
            ISession session = SessionManager.GetSession();
            Pacijent pacijent = new Pacijent();

            if (session == null)
                return null;

            Row pacijentData = session.Execute("select * from \"Pacijent\" where \"broj_knjizice\"=" + brojKnjizice).FirstOrDefault();

            if (pacijentData != null)
            {
                pacijent.BrojKnjizice = (int)(pacijentData["broj_knjizice"] ?? 0);
                pacijent.Ime = pacijentData["ime"] != null ? pacijentData["ime"].ToString() : string.Empty;
                pacijent.Prezime = pacijentData["prezime"] != null ? pacijentData["prezime"].ToString() : string.Empty;
                pacijent.Godine = (int)(pacijentData["godine"] ?? 0);
                pacijent.TestTimestamp = (DateTimeOffset)(pacijentData["test_timestamp"] ?? DateTimeOffset.MinValue);
                pacijent.Pozitivan = (bool)(pacijentData["pozitivan"] ?? false);
            }

            return pacijent;
        }

        public static List<Pacijent> GetPacijente()
        {
            ISession session = SessionManager.GetSession();
            List<Pacijent> pacijenti = new List<Pacijent>();


            if (session == null)
                return null;

            var pacijentiData = session.Execute("select * from \"Pacijent\"");


            foreach (var pacijentData in pacijentiData)
            {
                Pacijent pacijent = new Pacijent();
                pacijent.BrojKnjizice = (int)(pacijentData["broj_knjizice"] ?? 0);
                pacijent.Ime = pacijentData["ime"] != null ? pacijentData["ime"].ToString() : string.Empty;
                pacijent.Prezime = pacijentData["prezime"] != null ? pacijentData["prezime"].ToString() : string.Empty;
                pacijent.Godine = (int)(pacijentData["godine"] ?? 0);
                pacijent.TestTimestamp = (DateTimeOffset)(pacijentData["test_timestamp"] ?? DateTimeOffset.MinValue);
                pacijent.Pozitivan = (bool)(pacijentData["pozitivan"] ?? false);
                pacijenti.Add(pacijent);
            }



            return pacijenti;
        }

        public static void AddPacijent(int brojKnjizice, string ime, string prezime, int godine,bool pozitivan,DateTimeOffset test_timestamp)
        {
            ISession session = SessionManager.GetSession();

            if (session == null)
                return;

            DateTime vreme = new DateTime(test_timestamp.Ticks);
            string[] vremeZaBrisanje = vreme.GetDateTimeFormats('u');

            RowSet pacijentData = session.Execute($"insert into \"Pacijent\" (broj_knjizice, ime, prezime, godine, test_timestamp, pozitivan)   values ({brojKnjizice}, '{ime}', '{prezime}', {godine}, '{vremeZaBrisanje[0]}', {pozitivan})");
           
        }

        public static void DeletePacijent(int brojKnjizice)
        {
            ISession session = SessionManager.GetSession();
            Pacijent pacijent = new Pacijent();

            if (session == null)
                return;

            RowSet pacijentData = session.Execute("delete from \"Pacijent\" where broj_knjizice = " + brojKnjizice);

        }

        #endregion

        #region Bolnica
        public static Bolnica GetBolnica(int bolnicaID, string grad)
        {
            ISession session = SessionManager.GetSession();
            Bolnica bolnica = new Bolnica();

            if (session == null)
                return null;

            Row bolnicaData = session.Execute("select * from \"Bolnica\" where bolnica_id=" + bolnicaID + " and grad='" + grad + "'").FirstOrDefault();

            if (bolnicaData != null)
            {
                bolnica.BolnicaID = (int)(bolnicaData["bolnica_id"] ?? 0);
                bolnica.Naziv = bolnicaData["naziv"] != null ? bolnicaData["naziv"].ToString() : string.Empty;
                bolnica.Adresa = bolnicaData["adresa"] != null ? bolnicaData["adresa"].ToString() : string.Empty;
                bolnica.Grad = bolnicaData["grad"] != null ? bolnicaData["grad"].ToString() : string.Empty;
                bolnica.BrojLjudiNaRespiratoru = (int)(bolnicaData["broj_ljudi_na_respiratoru"] ?? 0);
                bolnica.BrojSlobodnihKreveta = (int)(bolnicaData["broj_slobodnih_kreveta"] ?? 0);
                bolnica.BrojZarazenihLekara = (int)(bolnicaData["broj_zarazenih_lekara"] ?? 0);
            }

            return bolnica;
        }

        public static List<Bolnica> GetBolnice()
        {
            ISession session = SessionManager.GetSession();
            List<Bolnica> bolnice = new List<Bolnica>();


            if (session == null)
                return null;

            var bolniceData = session.Execute("select * from \"Bolnica\"");


            foreach (var bolnicaData in bolniceData)
            {
                Bolnica bolnica = new Bolnica();
                bolnica.BolnicaID = (int)(bolnicaData["bolnica_id"] ?? 0);
                bolnica.Naziv = bolnicaData["naziv"] != null ? bolnicaData["naziv"].ToString() : string.Empty;
                bolnica.Adresa = bolnicaData["adresa"] != null ? bolnicaData["adresa"].ToString() : string.Empty;
                bolnica.Grad = bolnicaData["grad"] != null ? bolnicaData["grad"].ToString() : string.Empty;
                bolnica.BrojLjudiNaRespiratoru = (int)(bolnicaData["broj_ljudi_na_respiratoru"] ?? 0);
                bolnica.BrojSlobodnihKreveta = (int)(bolnicaData["broj_slobodnih_kreveta"] ?? 0);
                bolnica.BrojZarazenihLekara = (int)(bolnicaData["broj_zarazenih_lekara"] ?? 0);
                bolnice.Add(bolnica);
            }

            return bolnice;
        }

        public static void AddBolnica(int bolnicaID, string naziv, string adresa, string grad)
        {
            ISession session = SessionManager.GetSession();

            if (session == null)
                return;

            RowSet bolnicaData = session.Execute($"insert into \"Bolnica\" (bolnica_id, naziv, adresa, grad, broj_slobodnih_kreveta, broj_ljudi_na_respiratoru, broj_zarazenih_lekara) values ({bolnicaID}, '{naziv}', '{adresa}', '{grad}', 50, 0, 0)");

        }

        public static void DeleteBolnica(int bolnicaID, string grad)
        {
            ISession session = SessionManager.GetSession();
            Bolnica bolnica = new Bolnica();

            if (session == null)
                return;

            RowSet bolnicaData = session.Execute("delete from \"Bolnica\" where bolnica_id=" + bolnicaID + " and grad='" + grad + "'");

        }
        public static void UpdateBolnica(int bolnicaID, string grad,int broj_ljudi_na_respiratoru,int broj_slobodnih_kreveta,int broj_zarazenih_lekara)
        {
            ISession session = SessionManager.GetSession();
            if (session == null)
                return;
            session.Execute($"UPDATE \"Bolnica\" SET broj_ljudi_na_respiratoru={broj_ljudi_na_respiratoru}," +
                $"broj_slobodnih_kreveta={broj_slobodnih_kreveta},broj_zarazenih_lekara={broj_zarazenih_lekara}" +
                $" where bolnica_id={bolnicaID}  and grad='{grad}'");
        }
        #endregion

        #region Ambulanta
        public static Ambulanta GetAmbulanta(int ambulantaId,string grad)
        {
            ISession session = SessionManager.GetSession();
            Ambulanta ambulanta = new Ambulanta();

            if (session == null)
                return null;

            //Row ambulantaData = session.Execute("select * from \"Ambulanta\" where \"ambulanta_id\"=" + ambulantaId
            //    + "and grad='"+grad + "'").FirstOrDefault();
            Row ambulantaData = session.Execute($"select * from \"Ambulanta\" where ambulanta_id={ambulantaId} and grad='{grad}'").FirstOrDefault();

            if (ambulantaData != null)
            {
                ambulanta.Ambulanta_id = (int)(ambulantaData["ambulanta_id"] ?? 0);
                ambulanta.Naziv = ambulantaData["naziv"] != null ? ambulantaData["naziv"].ToString() : string.Empty;
                ambulanta.Adresa = ambulantaData["adresa"] != null ? ambulantaData["adresa"].ToString() : string.Empty;
                ambulanta.Grad = ambulantaData["grad"] != null ? ambulantaData["grad"].ToString() : string.Empty;
                ambulanta.Dan = (LocalDate)(ambulantaData["dan"] ?? null);
                ambulanta.BrojPozitivnih = (int)(ambulantaData["broj_pozitivnih"] ?? 0);
                ambulanta.BrojPregledanih = (int)(ambulantaData["broj_pregledanih"] ?? 0);
                ambulanta.BrojTestiranih = (int)(ambulantaData["broj_testiranih"] ?? 0);
                ambulanta.BrojLekara = (int)(ambulantaData["broj_lekara"] ?? 0);
            }

            return ambulanta;
        }
        public static void AddAmbulanta(int ambulanta_id,string naziv,string adresa,string grad,LocalDate dan,int broj_testiranih, int broj_pregledanih, int broj_lekara, int broj_pozitivnih)
        {
            ISession session = SessionManager.GetSession();

            if (session == null)
                return;

            RowSet ambulantaData = session.Execute($"insert into \"Ambulanta\" (ambulanta_id,naziv,adresa,grad,dan,broj_testiranih,broj_pregledanih,broj_lekara,broj_pozitivnih) values ({ambulanta_id},'{naziv}','{adresa}','{grad}','{dan}',{broj_testiranih},{broj_pregledanih},{broj_lekara},{broj_pozitivnih})");

        }
        public static List<Ambulanta> GetAmbulantas()
        {
            ISession session = SessionManager.GetSession();
            List<Ambulanta> ambulantas = new List<Ambulanta>();

            if (session == null)
                return null;

            var ambulantasData = session.Execute("select * from \"Ambulanta\"");

            foreach (Row ambulantaData in ambulantasData)
            {
                Ambulanta ambulanta = new Ambulanta();
                ambulanta.Ambulanta_id = (int)(ambulantaData["ambulanta_id"] ?? 0);
                ambulanta.Naziv = ambulantaData["naziv"] != null ? ambulantaData["naziv"].ToString() : string.Empty;
                ambulanta.Adresa = ambulantaData["adresa"] != null ? ambulantaData["adresa"].ToString() : string.Empty;
                ambulanta.Grad = ambulantaData["grad"] != null ? ambulantaData["grad"].ToString() : string.Empty;
                ambulanta.Dan = (LocalDate)(ambulantaData["dan"] ?? null);
                ambulanta.BrojPozitivnih = (int)(ambulantaData["broj_pozitivnih"] ?? 0);
                ambulanta.BrojPregledanih = (int)(ambulantaData["broj_pregledanih"] ?? 0);
                ambulanta.BrojTestiranih = (int)(ambulantaData["broj_testiranih"] ?? 0);
                ambulanta.BrojLekara = (int)(ambulantaData["broj_lekara"] ?? 0);

                ambulantas.Add(ambulanta);
            }
            return ambulantas;
        }
        public static void DeleteAmbulanta(int ambulanta_id,string grad)
        {
            ISession session = SessionManager.GetSession();

            if (session == null)
                return;

            RowSet reservationData = session.Execute($"delete from \"Ambulanta\" where ambulanta_id={ambulanta_id} and grad='{grad}'");

        }
        public static Ambulanta GetAmbulantaPoDanu(int ambulantaId, string grad, LocalDate dan)
        {
            ISession session = SessionManager.GetSession();
            Ambulanta ambulanta = new Ambulanta();

            if (session == null)
                return null;

            Row ambulantaData = session.Execute($"select * from \"Ambulanta\" where ambulanta_id={ambulantaId} and grad='{grad}' and dan='{dan}'").FirstOrDefault();

            if (ambulantaData != null)
            {
                ambulanta.Ambulanta_id = (int)(ambulantaData["ambulanta_id"] ?? 0);
                ambulanta.Naziv = ambulantaData["naziv"] != null ? ambulantaData["naziv"].ToString() : string.Empty;
                ambulanta.Adresa = ambulantaData["adresa"] != null ? ambulantaData["adresa"].ToString() : string.Empty;
                ambulanta.Grad = ambulantaData["grad"] != null ? (ambulantaData["grad"].ToString()) : string.Empty;
                ambulanta.Dan = ambulantaData["dan"] != null ? (LocalDate)(ambulantaData["dan"]) : null;
                ambulanta.BrojPozitivnih = (int)(ambulantaData["broj_pozitivnih"] ?? 0);
                ambulanta.BrojPregledanih = (int)(ambulantaData["broj_pregledanih"] ?? 0);
                ambulanta.BrojTestiranih = (int)(ambulantaData["broj_testiranih"] ?? 0);
                ambulanta.BrojLekara = ambulantaData["broj_lekara"] != null ? (int)ambulantaData["broj_lekara"] : 0;


            }

            return ambulanta;
        }
        public static List<string> GetAmbulantaDani(int ambulanta_id, string grad)
        {
            ISession session = SessionManager.GetSession();

            if (session == null)
                return null;

            List<string> dani = new List<string>();

            RowSet daniData = session.Execute($"select dan from \"Ambulanta\" where ambulanta_id={ambulanta_id} and grad='{grad}'");

            foreach (Row danData in daniData)
            {
                LocalDate dan = danData["dan"] != null ? (LocalDate)(danData["dan"]) : null;
                if (dan != null)
                {
                    string datumString = ($"{dan.Day}/{dan.Month}/{dan.Year}");
                    dani.Add(datumString);
                }
            }

            return dani;
        }
        #endregion

        #region Korisnik
        public static Korisnik GetKorisnik(string username)
        {
            ISession session = SessionManager.GetSession();
            Korisnik korisnik = new Korisnik();

            if (session == null)
                return null;


            Row korisnikData = session.Execute($"select * from \"Korisnik\" where username='{username}'").FirstOrDefault();

            if (korisnikData != null)
            {
                korisnik.Username = korisnikData["username"] != null ? korisnikData["username"].ToString() : string.Empty;
                korisnik.Password = korisnikData["password"] != null ? korisnikData["password"].ToString() : string.Empty;
                korisnik.Rola_Id = korisnikData["rola_id"] != null ? korisnikData["rola_id"].ToString() : string.Empty;
                korisnik.Grad = korisnikData["grad"] != null ? korisnikData["grad"].ToString() : string.Empty;
                korisnik.IdRadnogMesta = (int)(korisnikData["id_radnog_mesta"] ?? 0);
            }
            return korisnik;
        }
        public static void AddKorisnik(string username,string password,string rola_id,string grad,int id_radnog_mesta)
        {
            ISession session = SessionManager.GetSession();

            if (session == null)
                return;

            RowSet korisnikData = session.Execute($"insert into \"Korisnik\" (username,password,rola_id,grad,id_radnog_mesta) values ('{username}','{password}','{rola_id}','{grad}',{id_radnog_mesta})");

        }
        public static List<Korisnik> GetKorisnike()
        {
            ISession session = SessionManager.GetSession();
            List<Korisnik> korisnici = new List<Korisnik>();

            if (session == null)
                return null;

            var korisniciData = session.Execute("select * from \"Korisnik\"");

            foreach (Row korisnikData in korisniciData)
            {
                Korisnik korisnik = new Korisnik();
                korisnik.Username = korisnikData["username"] != null ? korisnikData["username"].ToString() : string.Empty;
                korisnik.Password = korisnikData["password"] != null ? korisnikData["password"].ToString() : string.Empty;
                korisnik.Rola_Id = korisnikData["rola_id"] != null ? korisnikData["rola_id"].ToString() : string.Empty;
                korisnik.Grad = korisnikData["grad"] != null ? korisnikData["grad"].ToString() : string.Empty;
                korisnik.IdRadnogMesta = (int)(korisnikData["id_radnog_mesta"] ?? 0);
                korisnici.Add(korisnik);
            }
            return korisnici;
        }
        public static void DeleteKorisnik(string username)
        {
            ISession session = SessionManager.GetSession();

            if (session == null)
                return;

            RowSet korisniknData = session.Execute($"delete from \"Korisnik\" where username='{username}'");
        }
        #endregion

        #region AmbulantaTestovi
        public static void AddAmbulantaTestovi(int ambulanta_id, string grad, DateTimeOffset test_timestamp, bool pozitivan)
        {
            ISession session = SessionManager.GetSession();

            if (session == null)
                return;

            DateTime vreme = new DateTime(test_timestamp.Ticks);

            string[] vremeZaBrisanje = vreme.GetDateTimeFormats('u'); //format koji cassandra podrzava

            RowSet ambulantaTestoviData = session.Execute($"insert into \"AmbulantaTestovi\" (ambulanta_id,grad,test_timestamp,pozitivan)" +
                                                          $" values ({ambulanta_id},'{grad}','{vremeZaBrisanje[0]}',{pozitivan})");
        }

        public static List<AmbulantaTestovi> GetAmbulantaTestovi(int ambulanta_id, string grad)
        {
            ISession session = SessionManager.GetSession();
            List<AmbulantaTestovi> testovi = new List<AmbulantaTestovi>();

            if (session == null)
                return null;

            var ambulantaTestoviData = session.Execute($"select * from \"AmbulantaTestovi\" where ambulanta_id={ambulanta_id} and grad='{grad}'");

            foreach (Row test in ambulantaTestoviData)
            {
                AmbulantaTestovi novTest = new AmbulantaTestovi();

                novTest.Ambulanta_id = test["ambulanta_id"] != null ? (int)test["ambulanta_id"] : int.MinValue;
                novTest.Grad = test["grad"] != null ? test["grad"].ToString() : string.Empty;
                novTest.Test_timestamp = test["test_timestamp"] != null ? (DateTimeOffset)test["test_timestamp"] : DateTimeOffset.MinValue;
                novTest.Pozitivan = test["pozitivan"] != null ? (bool)test["pozitivan"] : false;

                testovi.Add(novTest);
            }

            return testovi;

        }

        public static void DeleteAmbulantaTestovi(int ambulanta_id, string grad, DateTimeOffset test_timestamp)
        {
            ISession session = SessionManager.GetSession();

            if (session == null)
                return;

            DateTime vreme = new DateTime(test_timestamp.Ticks);

            string[] vremeZaBrisanje = vreme.GetDateTimeFormats('u');

            RowSet gradAmbulantaData = session.Execute($"delete from \"AmbulantaTestovi\" where grad='{grad}' and ambulanta_id={ambulanta_id} and test_timestamp='{vremeZaBrisanje[0]}'");

        }
        public static List<AmbulantaTestovi> GetAmbulantaTestoviZaDanas(int ambulanta_id, string grad, LocalDate dan)
        {
            ISession session = SessionManager.GetSession();
            List<AmbulantaTestovi> testovi = new List<AmbulantaTestovi>();

            if (session == null)
                return null;

            var ambulantaTestoviData = session.Execute($"select * from \"AmbulantaTestovi\" where ambulanta_id={ambulanta_id} and grad='{grad}' and test_timestamp > '{dan} 00:00:00'");

            foreach (Row test in ambulantaTestoviData)
            {
                AmbulantaTestovi novTest = new AmbulantaTestovi();

                novTest.Ambulanta_id = test["ambulanta_id"] != null ? (int)test["ambulanta_id"] : int.MinValue;
                novTest.Grad = test["grad"] != null ? test["grad"].ToString() : string.Empty;
                novTest.Test_timestamp = test["test_timestamp"] != null ? (DateTimeOffset)test["test_timestamp"] : DateTimeOffset.MinValue;
                novTest.Pozitivan = test["pozitivan"] != null ? (bool)test["pozitivan"] : false;

                testovi.Add(novTest);
            }

            return testovi;

        }
        #endregion

        #region Statistika
        public static void AddStatistika(LocalDate dan, string danasnje_mere, int broj_testiranih, int broj_pregledanih, int broj_pozitivnih)
        {
            ISession session = SessionManager.GetSession();

            if (session == null)
                return;

            RowSet ambulantaTestoviData = session.Execute($"insert into \"Statistika\" (dan,broj_pozitivnih,broj_pregledanih,broj_testiranih,danasnje_mere)" +
                                                          $" values ('{dan}',{broj_pozitivnih},{broj_pregledanih},{broj_testiranih},'{danasnje_mere}')");
        }

        public static Statistika GetStatistika(LocalDate dan)
        {
            ISession session = SessionManager.GetSession();
            Statistika statistika = new Statistika();

            if (session == null)
                return null;

            Row statistikaData = session.Execute($"select * from \"Statistika\" where dan='{dan}'").FirstOrDefault();
            if (statistikaData == null)
            {
                statistika.Dan =  new LocalDate(1960, 1, 1);
                statistika.Danasnje_mere = string.Empty;
                statistika.BrojTestiranih =  int.MinValue;
                statistika.BrojPozitivnih =  int.MinValue;
                statistika.BrojPregledanih = int.MinValue;
            }
            else
            {
                statistika.Dan = statistikaData["dan"] != null ? (LocalDate)statistikaData["dan"] : new LocalDate(1960, 1, 1);
                statistika.Danasnje_mere = statistikaData["danasnje_mere"] != null ? statistikaData["danasnje_mere"].ToString() : string.Empty;
                statistika.BrojTestiranih = statistikaData["broj_testiranih"] != null ? (int)statistikaData["broj_testiranih"] : int.MinValue;
                statistika.BrojPozitivnih = statistikaData["broj_pozitivnih"] != null ? (int)statistikaData["broj_pozitivnih"] : int.MinValue;
                statistika.BrojPregledanih = statistikaData["broj_pregledanih"] != null ? (int)statistikaData["broj_pregledanih"] : int.MinValue;
            }
           

            return statistika;
        }

        public static void UpdateStatistika(LocalDate dan, string danasnje_mere, int broj_testiranih, int broj_pregledanih, int broj_pozitivnih)
        {
            ISession session = SessionManager.GetSession();

            if (session == null)
                return;

            RowSet gradAmbulantaData = session.Execute($"update \"Statistika\" set broj_pozitivnih={broj_pozitivnih}, broj_testiranih={broj_testiranih}, " +
                                                                                 $"broj_pregledanih={broj_pregledanih}, danasnje_mere='{danasnje_mere}' " +
                                                                                 $"where dan='{dan}'");
        }

        public static void DeleteStatistika(LocalDate dan)
        {
            ISession session = SessionManager.GetSession();

            if (session == null)
                return;

            RowSet gradAmbulantaData = session.Execute($"delete from \"Statistika\" where dan='{dan}'");
        }

        #endregion

        #region Counter
            public static int getID(string vrsta_radnog_mesta)
        {
            ISession session = SessionManager.GetSession();
            if (session == null || !(vrsta_radnog_mesta.Equals("BOLNICA") || vrsta_radnog_mesta.Equals("AMBULANTA")))
                return 0;

            session.Execute($"UPDATE \"Counter\" set id = id + 1 where vrsta_radnog_mesta = '{vrsta_radnog_mesta}'").FirstOrDefault();
            Row data = session.Execute($"SELECT * from  \"Counter\"  where vrsta_radnog_mesta = '{vrsta_radnog_mesta}'").FirstOrDefault();
            return (int)(long)data["id"];  
        }
        #endregion

    }
}
