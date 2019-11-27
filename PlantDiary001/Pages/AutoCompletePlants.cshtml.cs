using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PlantDiary001.Pages
{
    public class AutoCompletePlantsModel : PageModel
    {
        private IList<string> plantNames = new List<string>();

        public JsonResult OnGet()
        {
            plantNames.Add("Redbud");
            plantNames.Add("Red Maple");
            plantNames.Add("Red Oak");
            plantNames.Add("Red Rose");
            plantNames.Add("Red Lily");

            return new JsonResult(plantNames);
        }
    }
}