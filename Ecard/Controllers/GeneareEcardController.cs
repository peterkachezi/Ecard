using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;

namespace Ecard.Controllers
{
    public class GeneareEcardController : ApiController
    {
        //GET: api/GeneareEcard
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        //GET: api/GeneareEcard/5
        public string Get(int id)
        {
            return "value";
        }

        //POST: api/GeneareEcard
        public void Post([FromBody] string value)
        {
        }

        [AllowAnonymous]
        [Route("Report/SendReport")]
        [HttpGet]
        public HttpResponseMessage ExportReport(string format)
        {
            try
            {                       
                var rd = new ReportDocument();

                rd.Load(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Reports"), "Ecard.rpt"));

                rd.SetParameterValue("MemberNo", "20133");

                rd.SetParameterValue("Name", "Peter Kachezi");

                rd.SetParameterValue("JobGroup", "LD");

                rd.SetParameterValue("Age", "16");

                rd.SetParameterValue("Gender", "M");

                rd.SetParameterValue("Relation", "Self");               

                Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);

                var k = stream.ReadByte();

                stream.Seek(0, SeekOrigin.Begin);

                HttpResponseMessage response1 = Request.CreateResponse(HttpStatusCode.OK, k);             

                return response1;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                HttpResponseMessage res = Request.CreateResponse(HttpStatusCode.NotFound, ex);

                return res;
            }
        }       
        
        
        [AllowAnonymous]
        [Route("GenerateEcard1")]
        [HttpPost]
        public HttpResponseMessage GenerateEcard()
        {
            try
            {
                var Host = System.Configuration.ConfigurationManager.AppSettings["SMTPServer"];

                var Port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["SMTPPort"]);

                bool EnableSsl = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["SMTPUseSSL"]);

                var senderId = System.Configuration.ConfigurationManager.AppSettings["SMTPEmail"];

                var senderPassword = System.Configuration.ConfigurationManager.AppSettings["SMTPPassword"];

                //var data = appointmentRepository.GetTransaction(Id);

                //if (data == null)
                //{
                //    var ErrorMessage = string.Format("Data not found.");

                //    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.NotFound, ErrorMessage);

                //    return response;
                //}

                var rd = new ReportDocument();

                rd.Load(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Reports"), "Ecard.rpt"));

                //rd.SetDataSource(data);

                rd.SetParameterValue("MemberNo", "20133");

                rd.SetParameterValue("Name", "Peter Kachezi");

                rd.SetParameterValue("JobGroup", "LD");

                rd.SetParameterValue("Age", "16");

                rd.SetParameterValue("Gender", "M");

                rd.SetParameterValue("Relation", "Self");

                using (var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat))
                {
                    SmtpClient smtp = new SmtpClient
                    {
                        Port = Port,

                        UseDefaultCredentials = true,

                        Host = Host,

                        EnableSsl = EnableSsl
                    };

                    smtp.UseDefaultCredentials = false;

                    smtp.Credentials = new NetworkCredential(senderId, senderPassword);

                    var message = new MailMessage(senderId, "peterkachezi@gmail.com", "Fertility Point Receipt", "Hello " + "Peter Kachezi" + ",  please find  attached .");

                    message.Attachments.Add(new Attachment(stream, "Receipt.pdf"));

                    smtp.Send(message);
                }

                var Message = string.Format("Receipt Created and sent to your Mail.");

                HttpResponseMessage response1 = Request.CreateResponse(HttpStatusCode.OK, Message);

                return response1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                HttpResponseMessage res = Request.CreateResponse(HttpStatusCode.NotFound, ex);

                return res;
            }
        }


        [Route("Ebook/GetBookForHRM")]
        [HttpGet]
        public HttpResponseMessage GetBookForHRM(string format)
        {
            string bookPath_Pdf = @"C:\MyWorkSpace\SelfDev\UserAPI\UserAPI\Books\sample.pdf";
            string bookPath_xls = @"C:\MyWorkSpace\SelfDev\UserAPI\UserAPI\Books\sample.xls";
            string bookPath_doc = @"C:\MyWorkSpace\SelfDev\UserAPI\UserAPI\Books\sample.doc";
            string bookPath_zip = @"C:\MyWorkSpace\SelfDev\UserAPI\UserAPI\Books\sample.zip";

            string reqBook = format.ToLower() == "pdf" ? bookPath_Pdf : (format.ToLower() == "xls" ? bookPath_xls : (format.ToLower() == "doc" ? bookPath_doc : bookPath_zip));
            string bookName = "sample." + format.ToLower();
            //converting Pdf file into bytes array  
            var dataBytes = File.ReadAllBytes(reqBook);
            //adding bytes to memory stream   
            var dataStream = new MemoryStream(dataBytes);

            HttpResponseMessage httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(dataStream);
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = bookName;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            return httpResponseMessage;
        }


        // PUT: api/GeneareEcard/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/GeneareEcard/5
        public void Delete(int id)
        {
        }
    }
}
