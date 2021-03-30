using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.ApplicationInsights;

namespace ContosoAdsWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Trace.TraceError("Hello diagnostics world!!!");
            Trace.WriteLine("Verbose Message"); // Write a verbose message
            Trace.TraceInformation("Information Message"); // Write an information message
            Trace.TraceWarning("Warning Message");
            Trace.TraceError("Error Message");

            var tc = new TelemetryClient();
            // Set up some properties:
            var properties = new Dictionary<string, string> { { "Game", "GameName" }, { "Difficulty", "Hard" } };
            var measurements = new Dictionary<string, double> { { "GameScore", 20 }, { "Opponents", 1 } };
            tc.TrackEvent("WinGame", properties, measurements);
            tc.TrackMetric("GameScore", 20, properties);

            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";


            var tc = new TelemetryClient();
            // Set up some properties:
            var properties = new Dictionary<string, string> { { "Game", "GameName" }, { "Difficulty", "Hard" } };
            var measurements = new Dictionary<string, double> { { "GameScore", 20 }, { "Opponents", 1 } };
            tc.TrackEvent("WinGame", properties, measurements);
            tc.TrackMetric("GameScore", 20, properties);

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            int i = 0;

            TelemetryClient tr = new TelemetryClient();
                 
            try
            {
                int b = 5/i;
            }
            catch (Exception ex)
            {
                tr.TrackException(ex,
                    new Dictionary<string, string>() { { "Agent", HttpContext.Request.UserAgent } },
                    new Dictionary<string, double>(){{"Metric", HttpContext.Request.Url.ToString().Length}});
            }
            return View();
        }
    }
}