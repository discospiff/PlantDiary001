using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PlantDiary001.Pages
{
    public class ConsumeXMLModel : PageModel
    {
        private IHostingEnvironment environment;
        private string result;

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

            ValidateXML(file);
        }

        /// <summary>
        /// Validate our XML against a known XSD.
        /// </summary>
        /// <param name="file"></param>
        private void ValidateXML(string file)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;

            // reading in the XSD file that will be used to validate the incoming XML.
            var xsdPath= Path.Combine(environment.ContentRootPath, "uploads", "plants.xsd");

            // give this XSD file to our validation processor.
            settings.Schemas.Add(null, xsdPath);

            settings.ValidationFlags |= System.Xml.Schema.XmlSchemaValidationFlags.ProcessSchemaLocation;
            settings.ValidationFlags |= System.Xml.Schema.XmlSchemaValidationFlags.ReportValidationWarnings;

            settings.ValidationEventHandler += new System.Xml.Schema.ValidationEventHandler(this.ValidationEventHandler);

            XmlReader xmlReader = XmlReader.Create(file, settings);

            try { 
                while (xmlReader.Read())
                {

                }
                result = "validation passed!";
                ViewData["result"] = "Validation Passed!";
            } catch (Exception e)
            {
                // we only get here if there is a validation error.
                ViewData["result"] = e.Message;
            }
        }

        public void ValidationEventHandler(object sender, ValidationEventArgs args)
        {
            result = "Validation failed.  Message: " + args.Message;
            throw new Exception("Validation failed.  Message: " + args.Message);
        }
    }
}