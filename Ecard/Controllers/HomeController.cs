using CrystalDecisions.CrystalReports.Engine;
using Ecard.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Ecard.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GenerateEcard(Guid Id)
        {
            try
            {
                if (Id == null || Id == Guid.Empty)
                {
                    return null;
                }
                var data = await DependantsService.GetDependants(Id);

                if (data == null)
                {
                    return null;
                }
                var accessToken = "sdsdsdsdsdsdsdsdsdsd";

                if (string.IsNullOrEmpty(accessToken))
                {
                    return null;
                }
                else
                {
                    ReportDocument rd = new ReportDocument();

                    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "Ecard.rpt"));

                    // show report

                    //rd.SetDataSource(list);

                    rd.SetParameterValue("MemberNo", data.MemberNumber);

                    rd.SetParameterValue("Name", data.FullName);

                    rd.SetParameterValue("JobGroup", data.JobGroup);

                    rd.SetParameterValue("Age", data.Age);

                    rd.SetParameterValue("Gender", data.Gender);

                    rd.SetParameterValue("Relation", data.Relation);

                    Response.Buffer = false;

                    Response.ClearContent();

                    Response.ClearHeaders();

                    rd.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;

                    rd.PrintOptions.ApplyPageMargins(new CrystalDecisions.Shared.PageMargins(5, 5, 5, 5));

                    rd.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA5;

                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

                    stream.Seek(0, SeekOrigin.Begin);

                    return File(stream, "application/pdf", "Ecard.pdf");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}