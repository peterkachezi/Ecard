﻿using CrystalDecisions.CrystalReports.Engine;
using Ecard.Models;
using Ecard.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Ecard.Controllers
{
    public class DownloadEcardController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> Download(Guid MemberId)
        {
            try
            {
                WebClient webClient = new WebClient();

                var key = ConfigurationManager.AppSettings["WebsiteUrl"];

                var userLink = key + "/Home/GetPatientDetails/" + "20133";

                byte[] Url = webClient.DownloadData("https://chart.googleapis.com/chart?cht=qr&chl=" + userLink + "&chs=160x160&chld=L|0");

                if (MemberId == null || MemberId == Guid.Empty)
                {
                    return null;
                }
                var data = await DependantsService.GetDependants(MemberId);

                //var ecardDetails = await DependantsService.GetEcardDetails(MemberId);
                var k = new List<Dependant>();

             
                //var k = ecardDetails.ToList();

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

                    rd.SetDataSource(k);

                    rd.SetParameterValue("MemberNo", data.PrincipalNumber);

                    rd.SetParameterValue("Name", data.FullName);

                    rd.SetParameterValue("JobGroup", data.JobGroup);

                    rd.SetParameterValue("Age", data.Age);

                    rd.SetParameterValue("Gender", data.Gender);

                    rd.SetParameterValue("Relation", data.Relation);

                    rd.SetParameterValue("Initials", data.Initials);

                    Response.Buffer = false;

                    Response.ClearContent();

                    Response.ClearHeaders();

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
    }
}