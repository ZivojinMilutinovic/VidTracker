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
    public class AdminModel : PageModel
    {
        [BindProperty]
        public string Message { get; set; } = "";
        [BindProperty]
        public string Mere { get; set; } = "";
        [BindProperty]
        public Bolnica Bolnica { get; set; }
        [BindProperty]
        public string MessageBolnica { get; set; } = "";
        [BindProperty]
        public Ambulanta Ambulanta { get; set; }
        [BindProperty]
        public string MessageAmbulanta { get; set; } = "";
        [BindProperty]
        public Korisnik Laborant { get; set; }
        [BindProperty]
        public string MessageLaborant { get; set; } = "";
        [BindProperty]
        public Korisnik Bolnicar { get; set; }
        [BindProperty]
        public string MessageBolnicar { get; set; } = "";
        [BindProperty]
        public string UsernameZaBrisanje { get; set; } = "";
        [BindProperty]
        public string MessageBrisanje { get; set; } = "";

        [BindProperty]
        public bool PrikaziGradStatistiku { get; set; } = false;
        [BindProperty]
        public string IzabraniGrad { get; set; } = "";
        [BindProperty]
        public int[] GradStatistika { get; set; } = new int[6];
        [BindProperty(SupportsGet =true)]
        public List<string> Gradovi { get; set; }

        public void OnGet()
        {
            Gradovi = DataProvider.GetAmbulantas().Select(x => x.Grad).Distinct().ToList();
        }

        public IActionResult OnPost()
        {
            Gradovi = DataProvider.GetAmbulantas().Select(x => x.Grad).Distinct().ToList();
            IzracunajStatistiku();
            return Page();
        }
        public IActionResult OnPostGrad()
        {
            Gradovi = DataProvider.GetAmbulantas().Select(x => x.Grad).Distinct().ToList();
            PrikaziGradStatistiku = true;
            var listaAmbulantas = DataProvider.GetAmbulantas();
            GradStatistika = new int[6];
            if (listaAmbulantas != null)
            {
                GradStatistika[0] = listaAmbulantas.Where(x => x.Grad == IzabraniGrad)
                                                    .Sum(x => x.BrojPregledanih);
                GradStatistika[1] = listaAmbulantas.Where(x => x.Grad == IzabraniGrad)
                                                    .Sum(x => x.BrojTestiranih);
                GradStatistika[2] = listaAmbulantas.Where(x => x.Grad == IzabraniGrad)
                                                    .Sum(x => x.BrojPozitivnih);
                GradStatistika[3] = listaAmbulantas.Where(x => x.Grad == IzabraniGrad && x.Dan == LocalDate.Parse(DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd")))
                                                    .Sum(x => x.BrojPregledanih);
                GradStatistika[4] = listaAmbulantas.Where(x => x.Grad == IzabraniGrad && x.Dan == LocalDate.Parse(DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd")))
                                                    .Sum(x => x.BrojTestiranih);
                GradStatistika[5] = listaAmbulantas.Where(x => x.Grad == IzabraniGrad && x.Dan == LocalDate.Parse(DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd")))
                                                    .Sum(x => x.BrojPozitivnih);
            }
            else
            {
            }

            return Page();
        }

        public IActionResult OnPostBolnica()
        {
           
                 Gradovi = DataProvider.GetAmbulantas().Select(x => x.Grad).Distinct().ToList();
                int id= DataProvider.getID("BOLNICA");
            
                DataProvider.AddBolnica(id, Bolnica.Naziv, Bolnica.Adresa, Bolnica.Grad);
                MessageBolnica = "Uspesno kreirana bolnica sa ID-em " + id + "!";
                return Page();
          
        }

        public IActionResult OnPostAmbulanta()
        {
          
            Gradovi = DataProvider.GetAmbulantas().Select(x => x.Grad).Distinct().ToList();
            int id = DataProvider.getID("AMBULANTA");
            DataProvider.AddAmbulanta(id, Ambulanta.Naziv, Ambulanta.Adresa, Ambulanta.Grad,
                                            LocalDate.Parse(DateTime.Today.ToString("yyyy-MM-dd")), 0, 0, 0, 0);
            MessageAmbulanta = "Uspesno kreirana ambulanta sa ID-em " + id + "!";
            return Page();
          
        }

        public IActionResult OnPostBolnicar()
        {
            Gradovi = DataProvider.GetAmbulantas().Select(x => x.Grad).Distinct().ToList();
            var korisnik = DataProvider.GetKorisnik(Bolnicar.Username);
            var radnoMestoBolnica = DataProvider.GetBolnica(Bolnicar.IdRadnogMesta, Bolnicar.Grad);
            if (korisnik.Username != null)
            {
                MessageBolnicar = "Vec postoji korisnik sa tim korisnickim imenom!";
                return null;
            }
            else if (radnoMestoBolnica.BolnicaID == 0)
            {
                MessageBolnicar = "Ne postoji ta bolnica!";
                return null;
            }
            else if (Bolnicar.Password == null)
            {
                MessageLaborant = "Sifra je obavezna!";
                return null;
            }
            else
            {
                DataProvider.AddKorisnik(Bolnicar.Username, Bolnicar.Password, "Bolnicar", Bolnicar.Grad, Bolnicar.IdRadnogMesta);
                MessageBolnicar = "Uspesno kreiran korisnik " + Bolnicar.Username + "!";
                return Page();
            }
        }
        public IActionResult OnPostLaborant()
        {
            Gradovi = DataProvider.GetAmbulantas().Select(x => x.Grad).Distinct().ToList();
            var korisnik = DataProvider.GetKorisnik(Laborant.Username);
            var radnoMestoAmbulanta = DataProvider.GetAmbulanta(Laborant.IdRadnogMesta, Laborant.Grad);
            if (korisnik.Username != null)
            {
                MessageLaborant = "Vec postoji korisnik sa tim korisnickim imenom!";
                return null;
            }
            else if (radnoMestoAmbulanta.Ambulanta_id == 0)
            {
                MessageLaborant = "Ne postoji ta ambulanta!";
                return null;
            }
            else if (Laborant.Password == null)
            {
                MessageLaborant = "Sifra je obavezna!";
                return null;
            }
            else
            {
                DataProvider.AddKorisnik(Laborant.Username, Laborant.Password, "Laborant", Laborant.Grad, Laborant.IdRadnogMesta);
                MessageLaborant = "Uspesno kreiran korisnik " + Laborant.Username + "!";
                return Page();
            }
        }
        public IActionResult OnPostObrisi()
        {
            Gradovi = DataProvider.GetAmbulantas().Select(x => x.Grad).Distinct().ToList();
            var korisnik = DataProvider.GetKorisnik(UsernameZaBrisanje);
            if (korisnik.Username != null)
            {
                DataProvider.DeleteKorisnik(UsernameZaBrisanje);
                if (korisnik.Rola_Id == "Bolnicar")
                {
                    MessageBrisanje = "Obrisan bolnicar " + UsernameZaBrisanje;
                }
                else if (korisnik.Rola_Id == "Laborant")
                {
                    MessageBrisanje = "Obrisan laborant " + UsernameZaBrisanje;
                }
                return null;
            }
            else
            {
                MessageBrisanje = "Ne postoji korisnik sa tim korisnickim imenom";
                return Page();
            }
        }


        public void IzracunajStatistiku()
        {
            var listaAmbulantas = DataProvider.GetAmbulantas();
            var statistika = DataProvider.GetStatistika(LocalDate.Parse(DateTime.Today.ToString("yyyy-MM-dd")));
            if (statistika.Dan.Equals(new Cassandra.LocalDate(1960, 1, 1)))
            {
                var brojTestiranih = listaAmbulantas.Where(x => x.Dan == LocalDate.Parse(DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd")))
                                                    .Sum(x => x.BrojTestiranih);
                var brojPregledanih = listaAmbulantas.Where(x => x.Dan == LocalDate.Parse(DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd")))
                                                    .Sum(x => x.BrojPregledanih);
                var brojPozitivnih = listaAmbulantas.Where(x => x.Dan == LocalDate.Parse(DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd")))
                                                    .Sum(x => x.BrojPozitivnih);
                DataProvider.AddStatistika(LocalDate.Parse(DateTime.Today.ToString("yyyy-MM-dd")), Mere, brojTestiranih, brojPregledanih, brojPozitivnih);
                Message = "Uspesno izracunata statistika!";
            }
            else
            {
                Message = "Danas vec izracunata statistika!";
            }
        }
    }
}