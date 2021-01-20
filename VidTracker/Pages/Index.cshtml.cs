using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using CassandraDataLayer;
using CassandraDataLayer.QueryEntities;
using Cassandra;

namespace VidTracker.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        [BindProperty(SupportsGet =true)]
        public Statistika Statistika { get; set; }
        [BindProperty]
        public string ErrorMessageKorisnik { get; set; }
        [BindProperty]
        public string Password{ get; set; }
        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public int BrojKnjizice { get; set; }
        [BindProperty(SupportsGet =true)]
        public Pacijent Pacijent { get; set; }
        [BindProperty]
        public string ErrorMessagePacijent { get; set; }
        [BindProperty(SupportsGet = true)]
        public DateTime IzabraniDan { get; set; }



        public void OnGet()
        {
            Pacijent.Godine = -1;
            IzabraniDan = DateTime.Today;
            Statistika = DataProvider.GetStatistika(LocalDate.Parse(DateTime.Today.ToString("yyyy-MM-dd")));
        }
        public IActionResult OnPostPacijent()
        {
            Statistika = DataProvider.GetStatistika(LocalDate.Parse(IzabraniDan.ToString("yyyy-MM-dd")));
            Pacijent = DataProvider.GetPacijent(BrojKnjizice);
            
            return Page();
        }
        public IActionResult OnPostKorisnik()
        {
            Pacijent.Godine = -1;
            IzabraniDan = DateTime.Today;
            Statistika = DataProvider.GetStatistika(LocalDate.Parse(DateTime.Today.ToString("yyyy-MM-dd")));
            Korisnik korisnik = DataProvider.GetKorisnik(Username);
            if (korisnik.Password != Password)
            {
                ErrorMessageKorisnik = "Sifre nisu jednake";
                return Page();
            }
            if (korisnik.Rola_Id == "Bolnicar")
            {
                return RedirectToPage("/Bolnica",new { grad=korisnik.Grad, radno_mesto=korisnik.IdRadnogMesta });
            } else if (korisnik.Rola_Id == "Laborant")
            {
                return RedirectToPage("/Ambulanta", new { grad = korisnik.Grad, radno_mesto = korisnik.IdRadnogMesta });
            }
            else if (korisnik.Rola_Id == "Admin")
                return RedirectToPage("/Admin");
            else
            {
                ErrorMessageKorisnik = "Ne postoji korisnik sa tim username-om!";
                return Page();
            }
        }
        public IActionResult OnPostStatistika()
        {
            
            Statistika = DataProvider.GetStatistika(LocalDate.Parse(IzabraniDan.ToString("yyyy-MM-dd")));
            
            Pacijent.Godine = -1;
            return Page();
        }
        public string pozitivanTest(bool pozitivan)
        {
            return pozitivan ? "pozitivan" : "negativan";
        }
        public string izracunajInt(int broj)
        {
            return broj < 0 ? "" : broj.ToString();
        }
        

    }
}
