using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PlantDiary001.Pages
{
    public class ConsumeXMLModel : PageModel
    {
        private IHostingEnvironment environment;

        public ConsumeXMLModel(IHostingEnvironment _environment)
        {
            environment = _environment;
        }

        [BindProperty]
        public IFormFile Upload { get; set; }
        public void OnGet()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public void OnPost()
        {
            string fileName = Upload.FileName;
            // the path where our app is running + upload subdirectory
            var file = Path.Combine(environment.ContentRootPath, "uploads", fileName);
            // copy the contents of the uploaded file to this location.
            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                Upload.CopyTo(fileStream);
            }

            // Read the document into memory using DOM
            XmlDocument doc = new XmlDocument();
            doc.Load(file);

            // XPATH
            XmlNode node = doc.SelectSingleNode("/plant/specimens/specimen[latitude>0]");
            int foo = 1 + 1;
        }
    }
}