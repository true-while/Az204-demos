# AZ-204 Demo: Azure Monitor

In the demo you will use Azure Monitor to monitor performance and activity

## Before delivery:

- none

## In class:

1. Start VM or Web App
1. Make several requests to the Web App to have some output in the metric
1. From the first page demonstrate the base telemetry.
 
![VM](diag.png)

1. From the `monitoring` select `metrics` add custom metrics: CPU % and Request Count. Change the time frame on top corner.
 
![Logs](diag2.png)

1. From the `web app` select `Audit Log` and demonstrate log tracked by Azure Monitor.

![Logs](diag3.png)
 
1. Find `Monitor` from the service list and open `Logs`. 
1. Select `Last Metrics` from the available query examples 
 
![Logs](diag3.png)

1. Run the query. Demonstrate results. 

![LogWorkspace](diag4.png)

```SQL
AzureMetrics 
| summarize arg_max(TimeGenerated, UnitName, Total, Count, Maximum, Minimum, Average) by MetricName
```
![LogWorkspace](diag5.png)