using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace ContosoAdsWeb.Models
{
    public class MyTelemetryInitializer : Microsoft.ApplicationInsights.Extensibility.ITelemetryInitializer
    {
        public void Initialize(TelemetryContext context)
        {
            context.Properties["AppVersion"] = "c2.1";
        }

        public void Initialize(ITelemetry telemetry)
        {
           
        }
    }
}