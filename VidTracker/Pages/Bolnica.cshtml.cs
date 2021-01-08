using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CassandraDataLayer;
using CassandraDataLayer.QueryEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VidTracker.Pages
{
    public class BolnicaModel : PageModel
    {
        [BindProperty(SupportsGet =true)]
        public Bolnica Bolnica { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<Bolnica> ListBolnica { get; set; }
        [BindProperty]
        public string ErrorMessage { get; set; }

        public void OnGet(string grad,int radno_mesto)
        {
            Bolnica = DataProvider.GetBolnica(radno_mesto, grad);
            ListBolnica = (List<Bolnica>)DataProvider.GetBolnice();
            ListBolnica.Remove(Bolnica);
        }
        public IActionResult OnPost()
        {
           
            if(Bolnica.BrojLjudiNaRespiratoru<0 || Bolnica.BrojSlobodnihKreveta<0 || Bolnica.BrojZarazenihLekara < 0)
            {
                Bolnica = DataProvider.GetBolnica(Bolnica.BolnicaID, Bolnica.Grad);
                ListBolnica = (List<Bolnica>)DataProvider.GetBolnice();
                ListBolnica.Remove(Bolnica);
                ErrorMessage = "Uneli ste pogresne vrenosti molimo pokusajte ponovo";
                return Page();
            }
            DataProvider.UpdateBolnica(Bolnica.BolnicaID, Bolnica.Grad, Bolnica.BrojLjudiNaRespiratoru, Bolnica.BrojSlobodnihKreveta, Bolnica.BrojZarazenihLekara);
            return RedirectToPage("/Bolnica", new { grad = Bolnica.Grad, radno_mesto = Bolnica.BolnicaID });
        }
    }
}
