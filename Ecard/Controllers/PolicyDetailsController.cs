using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Ecard.Controllers
{
    public class PolicyDetailsController : Controller
    {
        // GET: PolicyDetails
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Download(string fileName)
        {
            try
            {

              
                fileName = "Brochure.pdf";

                string path = Server.MapPath("~/Content/PolicyDetails");

                byte[] fileBytes = System.IO.File.ReadAllBytes(path + @"\" + fileName);

                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
             
            }
        }
    }
}