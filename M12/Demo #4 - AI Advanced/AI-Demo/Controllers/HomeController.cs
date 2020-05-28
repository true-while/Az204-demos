using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AI_Demo.Models;
using System.IO;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace AI_Demo.Controllers
{
    public class HomeController : Controller
    {
        static TelemetryClient telemetryClient = new TelemetryClient() { InstrumentationKey = "e52566ba-42d2-462a-903c-c28a503e8316" };
        private BlobRepository blobRepository;

        public object BlobRepository { get; private set; }

        static HomeController()
        {
            telemetryClient = new Microsoft.ApplicationInsights.TelemetryClient();
            telemetryClient.Context.Component.Version =
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

        }

        public HomeController(BlobRepository rep)
        {
            blobRepository = rep;
        }

        public static T TrackDependency<T>(Func<T> caller, string depName, string depTarget, string functionName,
            string key)
        {
            bool sucess = false;
            Stopwatch sw = new Stopwatch();
            DateTime start = DateTime.UtcNow;
            sw.Start();

            try
            {
                T result = caller.Invoke();
                sucess = true;
                return result;
            }
            finally
            {
                telemetryClient.TrackDependency(new DependencyTelemetry(depName, depTarget, functionName, key, start,
                    sw.Elapsed, sucess ? "1" : "0", sucess));

            }
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            try
            {
                ViewBag.Message = "Your contact page.";

                Parallel.Invoke(() => ExecuteDumyFunction("Some state"));

                return View();
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                throw;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private void ExecuteDumyFunction(string state)
        {
            throw new ExecutionEngineException("You better move on .Net Core");
        }

        public ActionResult GetFile()
        {
            var file = "test.zip";

            /*AI track */
            telemetryClient.TrackTrace("File Requested: " + file);

            /*register request*/
            telemetryClient.Context.Operation.Id = Guid.NewGuid().ToString();
            telemetryClient.Context.Operation.Name = file;
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            if (String.IsNullOrEmpty(file))
            {
                HttpContext.Response.StatusCode = 404;
                telemetryClient.TrackRequest(file, DateTime.Now, stopwatch.Elapsed, "404", true);
                return null;
            }

            using (MemoryStream ms = new MemoryStream())
            {

                /*track dependency*/
                var result = TrackDependency(() =>
                        blobRepository.DownLoadFile(ms, "files", file)
                    , "AzureBlob", "container:files", "Download", file);


                if (result)
                {

                    HttpContext.Response.Headers.Add("content-disposition", string.Format("attachment; filename=\"{0}\"", file));
                    HttpContext.Response.ContentType = GetMIMI(Path.GetExtension(file));

                    ms.Position = 0;
                    var readSize = 1024;
                    var buffer = new byte[readSize];
                    int count = ms.Read(buffer, 0, readSize);
                    while (count > 0)
                    {
                        HttpContext.Response.Body.Write(buffer);
                        count = ms.Read(buffer, 0, readSize);
                    }

                    stopwatch.Stop();

                    /* track load time */
                    telemetryClient.TrackRequest(file, DateTime.Now, stopwatch.Elapsed, "200", true);

                    /*track metric of data transfer */
                    telemetryClient.TrackMetric("DataTransfer", ms.Length);

                    telemetryClient.TrackEvent(new EventTelemetry()
                    {
                        Name = file,
                        Metrics =
                        {
                            {"size", ms.Length},
                            {"process-time", stopwatch.Elapsed.TotalMilliseconds}
                        },
                        Properties =
                        {
                            {"IP", HttpContext.Connection.RemoteIpAddress.ToString()},
                            {"Ref", HttpContext.Request.Headers["Referer"].ToString()},
                            {"Agent", HttpContext.Request.Headers["User-Agent"].ToString()}
                        }
                    }); /*track file request */
                }
                else
                {
                    HttpContext.Response.StatusCode = 404;
                    stopwatch.Stop();
                    /* track 404 */
                    telemetryClient.TrackRequest(file, DateTime.Now, stopwatch.Elapsed, "404", true);
                    /* track exception */
                    telemetryClient.TrackException(new FileNotFoundException(file));
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
