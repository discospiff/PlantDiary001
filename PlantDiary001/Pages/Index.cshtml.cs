using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using QuickType;
using QuickTypePlants;

namespace PlantDiary001.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            
            using (WebClient webClient = new WebClient())
            {
                string plantJson = webClient.DownloadString("http://plantplaces.com/perl/mobile/viewplantsjsonarray.pl?WetTolerant=on");
                Plant[] allPlants = Plant.FromJson(plantJson);

                string jsonString = webClient.DownloadString("https://www.plantplaces.com/perl/mobile/viewspecimenlocations.pl?Lat=39.14455075&Lng=-84.5093939666667&Range=0.5&Source=location&Version=2");
                Welcome welcome = Welcome.FromJson(jsonString);
                Specimen[] allSpecimens = welcome.Specimens;

                // this new array will hold only specimens that like water.
                List<Specimen> waterLovingSpecimens = new List<Specimen>();

                // iterate over the specimens, to find which ones like water.
                foreach(Specimen specimen in allSpecimens)
                {
                    // find the matching plant record for this specimen.
                    foreach(Plant plant in allPlants)
                    {
                        if (plant.Id == specimen.PlantId)
                        {
                            // we have a match!
                            waterLovingSpecimens.Add(specimen);
                        }
                    }
                }

                // TODO only show water-loving specimens
                ViewData["allSpecimens"] = waterLovingSpecimens;
            }

            int yearStarted = 2006;
            string name = "My Plant Diary";
            ViewData["Name"] = name;
        }
    }
}
