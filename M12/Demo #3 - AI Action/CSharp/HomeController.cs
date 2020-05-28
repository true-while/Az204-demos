using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DiagnosticWeb.Models;
using FastStart;
using Microsoft.ApplicationInsights.DataContracts;

namespace DiagnosticWeb.Controllers
{
    public class HomeController : Controller
    {
        

        public ActionResult Index()
        {
                return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            /*
             * Write  messages
             */

            Trace.WriteLine("Write a verbose message"); 
            Trace.TraceInformation("Write an information message"); 
            Trace.TraceWarning("Write an Warning message");
            Trace.TraceError("Error message");

            /* AI track */
            TelemetryHelper.TelemetryClient.TrackTrace("About Request");

            return View();
        }

        public ActionResult Contact()
        {
            try
            {
                ViewBag.Message = "Your contact page.";

                Parallel.Invoke(() => ExecuteDumyFunction("Some state"));

                return View();
            }
            catch (Exception ex)
            {
                TelemetryHelper.TelemetryClient.TrackException(ex);
                throw;
            }
        }


        private void ExecuteDumyFunction(string state)
        {
            throw new ExecutionEngineException("You better move on .Net Core");
        }

        public ActionResult GetFile()
        {
            var file = "dg-slides.zip";

            /*AI track */
            TelemetryHelper.TelemetryClient.TrackTrace("File Requested: " + file);

            /*register request*/
            TelemetryHelper.TelemetryClient.Context.Operation.Id = Guid.NewGuid().ToString();
            TelemetryHelper.TelemetryClient.Context.Operation.Name = file;
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            if (String.IsNullOrEmpty(file))
            {
                HttpContext.Response.StatusCode = 404;
                TelemetryHelper.TelemetryClient.TrackRequest(file, DateTime.Now, stopwatch.Elapsed, "404", true);
                return null;
            }

            using (MemoryStream ms = new MemoryStream())
            {

                /*track dependency*/
                var result = TelemetryHelper.TrackDependency(() =>
                        BlobRepository.GetInstance.DownLoadFile(ms, "workshop", file)
                    , "AzureBlob", "container:workshop", "Download", file);


                if (result)
                {

                    HttpContext.Response.Clear();
                    HttpContext.Response.AddHeader("content-disposition", string.Format("attachment; filename=\"{0}\"", file));
                    HttpContext.Response.ContentType = GetMIMI(Path.GetExtension(file));

                    ms.Position = 0;
                    var readSize = 1024;
                    var buffer = new byte[readSize];
                    int count = ms.Read(buffer, 0, readSize);
                    while (count > 0)
                    {
                        HttpContext.Response.BinaryWrite(buffer);
                        count = ms.Read(buffer, 0, readSize);
                    }

                    stopwatch.Stop();

                    /* track load time */
                    TelemetryHelper.TelemetryClient.TrackRequest(file, DateTime.Now, stopwatch.Elapsed, "200", true);

                    /*track metric of data transfer */
                    TelemetryHelper.TelemetryClient.TrackMetric("DataTransfer", ms.Length);

                    TelemetryHelper.TelemetryClient.TrackEvent(new EventTelemetry()
                    {
                        Name = file,
                        Metrics =
                        {
                            {"size", ms.Length},
                            {"process-time", stopwatch.Elapsed.TotalMilliseconds}
                        },
                        Properties =
                        {
                            {"IP", HttpContext.Request.UserHostAddress},
                            {"Ref", HttpContext.Request.UrlReferrer?.ToString()},
                            {"Agent", HttpContext.Request.UserAgent}
                        }
                    }); /*track file request */
                }
                else
                {
                    HttpContext.Response.StatusCode = 404;
                    stopwatch.Stop();
                    /* track 404 */
                    TelemetryHelper.TelemetryClient.TrackRequest(file, DateTime.Now, stopwatch.Elapsed, "404", true);
                    /* track exception */
                    TelemetryHelper.TelemetryClient.TrackException(new FileNotFoundException(file));
                }
                ms.Close();
            }
            return null;
        }

        private string GetMIMI(string extension)
        {
            switch (extension)
            {
                case ".zip":
                    return "application/zip";
                case ".rar":
                    return "application/x-rar-compressed";
                case ".pdf":
                    return "application/pdf";
                case ".docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case ".xlsx":
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                default:
                    return "application/octet-stream";
            }

        }
    }
}