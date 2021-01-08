using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cassandra;
using CassandraDataLayer;
using CassandraDataLayer.QueryEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VidTracker.Pages
{
    public class AmbulantaModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public Ambulanta Ambulanta { get; set; }
        [BindProperty]
        public string ErrorMessage { get; set; } = "";
        [BindProperty]
        public string SuccessMessage { get; set; } = "";
        [BindProperty]
        public int BrojZdravstvene { get; set; }
        [BindProperty]
        public bool Pozitivan { get; set; }
        [BindProperty]
        public string Ime { get; set; }
        [BindProperty]
        public string Prezime { get; set; }
        [BindProperty]
        public int Godine { get; set; }
        [BindProperty]
        public DateTime Dan { get; set; }
        public SelectList ListaDana { get; set; }
        [BindProperty]
        public string IzabranDan { get; set; }
        [BindProperty]
        public int BrojTestiranih { get; set; }
        [BindProperty]
        public int BrojPregledanih { get; set; }
        [BindProperty]
        public int BrojPozitivnih { get; set; }
        [BindProperty]
        public int BrojLekara { get; set; }
        [BindProperty]
        public string KorisnickoIme { get; set; }
        [BindProperty]
        public string Sifra { get; set; }
        [BindProperty]
        public string ErrorMessageZaRacunanje { get; set; } = "";
        [BindProperty]
        public int BrojPregledanihZaDanas { get; set; }
        [BindProperty]
        public int BrojLekaraZaDanas { get; set; }


        public void OnGet(string grad, int radno_mesto)
        {
            Ambulanta = DataProvider.GetAmbulanta(radno_mesto, grad);
            ListaDana = new SelectList(DataProvider.GetAmbulantaDani(radno_mesto, grad));
            Dan = new DateTime(Ambulanta.Dan.Year, Ambulanta.Dan.Month, Ambulanta.Dan.Day);
            BrojPozitivnih = Ambulanta.BrojPozitivnih;
            BrojPregledanih = Ambulanta.BrojPregledanih;
            BrojTestiranih = Ambulanta.BrojTestiranih;
            BrojLekara = Ambulanta.BrojLekara;
        }

        public IActionResult OnPostPacijent(int id, string grad, DateTime dan)
        {
            LocalDate datum = new LocalDate(dan.Year, dan.Month, dan.Day);
            Ambulanta = DataProvider.GetAmbulantaPoDanu(id, grad, datum);
            BrojPozitivnih = Ambulanta.BrojPozitivnih;
            BrojPregledanih = Ambulanta.BrojPregledanih;
            BrojTestiranih = Ambulanta.BrojTestiranih;
            BrojLekara = Ambulanta.BrojLekara;
            ListaDana = new SelectList(DataProvider.GetAmbulantaDani(id, grad));
            Dan = dan;

            if (BrojZdravstvene == 0)
            {
                ErrorMessage = "* Oznacava obavezna polja";
                return Page();
            }
            else
            {
                DateTimeOffset vreme = DateTimeOffset.Now;
                DataProvider.AddPacijent(BrojZdravstvene, Ime, Prezime, Godine, Pozitivan, vreme);
                DataProvider.AddAmbulantaTestovi(Ambulanta.Ambulanta_id, Ambulanta.Grad, vreme, Pozitivan);
                SuccessMessage = "Uspesno dodat test!";
                return Page();
            }
        }

        public IActionResult OnPostOsvezi()
        {
            if (!string.IsNullOrEmpty(IzabranDan))
            {
                string[] splitString = IzabranDan.Split('/');

                LocalDate datum = new LocalDate(Int32.Parse(splitString[2]), Int32.Parse(splitString[1]), Int32.Parse(splitString[0]));
                Ambulanta = DataProvider.GetAmbulantaPoDanu(Ambulanta.Ambulanta_id, Ambulanta.Grad, datum);
                ListaDana = new SelectList(DataProvider.GetAmbulantaDani(Ambulanta.Ambulanta_id, Ambulanta.Grad));
                Dan = new DateTime(Ambulanta.Dan.Year, Ambulanta.Dan.Month, Ambulanta.Dan.Day);
                BrojPozitivnih = Ambulanta.BrojPozitivnih;
                BrojPregledanih = Ambulanta.BrojPregledanih;
                BrojTestiranih = Ambulanta.BrojTestiranih;
                BrojLekara = Ambulanta.BrojLekara;
            }
            return Page();
        }

        public IActionResult OnPostIzracunaj(int id, string grad, DateTime dan)
        {
            LocalDate datum = new LocalDate(dan.Year, dan.Month, dan.Day);
            Ambulanta = DataProvider.GetAmbulantaPoDanu(id, grad, datum);
            BrojPozitivnih = Ambulanta.BrojPozitivnih;
            BrojPregledanih = Ambulanta.BrojPregledanih;
            BrojTestiranih = Ambulanta.BrojTestiranih;
            BrojLekara = Ambulanta.BrojLekara;
            ListaDana = new SelectList(DataProvider.GetAmbulantaDani(id, grad));
            Dan = dan;
            if (string.IsNullOrEmpty(KorisnickoIme))
            {
                ErrorMessageZaRacunanje = "Molimo Vas unesite korisnicko ime";
                return Page();
            }
            if (string.IsNullOrEmpty(Sifra))
            {
                ErrorMessageZaRacunanje = "Molimo Vas unesite sifru";
                return Page();
            }
            Korisnik korisnik = DataProvider.GetKorisnik(KorisnickoIme);
            if (korisnik == null)
            {
                ErrorMessageZaRacunanje = "Ne postoji ovaj korisnik u bazi";
                return Page();
            }
            if (korisnik.Password != Sifra)
            {
                ErrorMessageZaRacunanje = "Pogresna lozinka";
                return Page();
            }
            if (korisnik.IdRadnogMesta != Ambulanta.Ambulanta_id || korisnik.Grad != Ambulanta.Grad)
            {
                ErrorMessageZaRacunanje = "Unet korisnik ne radi u ovoj ambulanti";
                return Page();
            }

            DateTime datumDateTime = DateTime.Now;
            LocalDate danas = new LocalDate(datumDateTime.Year, datumDateTime.Month, datumDateTime.Day);
            Ambulanta ambulantaDanas = DataProvider.GetAmbulantaPoDanu(korisnik.IdRadnogMesta, korisnik.Grad, danas);

            if (ambulantaDanas.Ambulanta_id != 0)
            {
                ErrorMessageZaRacunanje = "Za danas je vec izracunata statistika";
                return Page();
            }

            Ambulanta ambulantaZaUnos = DataProvider.GetAmbulanta(korisnik.IdRadnogMesta, korisnik.Grad);

            List<AmbulantaTestovi> testovi = DataProvider.GetAmbulantaTestoviZaDanas(ambulantaZaUnos.Ambulanta_id, ambulantaZaUnos.Grad, danas);

            int brojTestiranih = 0;
            int brojPozitivnih = 0;

            foreach (AmbulantaTestovi test in testovi)
            {
                if (test.Pozitivan == true)
                    brojPozitivnih++;
                brojTestiranih++;
            }

            DataProvider.AddAmbulanta(korisnik.IdRadnogMesta, ambulantaZaUnos.Naziv, ambulantaZaUnos.Adresa,
                                      ambulantaZaUnos.Grad, danas, brojTestiranih, BrojPregledanihZaDanas,
                                      BrojLekaraZaDanas, brojPozitivnih);

            return RedirectToPage($"/Ambulanta", new { grad = korisnik.Grad, radno_mesto = korisnik.IdRadnogMesta });

        }

    }
}