using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace TranslateToPirate
{
    public static class Function
    {
        [FunctionName("TranslateToPiratish")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");


            string toTranslate = req.Query["text"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            toTranslate = toTranslate ?? data?.text;

            if (toTranslate == null)
            {
                var err = JsonConvert.SerializeObject(new { error = "field 'text' must be provided!" });
                return new BadRequestObjectResult(err) { ContentTypes = { "application/json" } };
            }
            try
            {
                // Call  API
                HttpClient newClient = new HttpClient();
                var json = JsonConvert.SerializeObject(new { text = toTranslate });

                var content = new StringContent(
                      json,
                      System.Text.Encoding.UTF8,
                      "application/json"
                      );

                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage result =
                        httpClient.PostAsync("https://api.funtranslations.com/translate/pirate.json",
                        content).Result;

                    if (result.IsSuccessStatusCode)
                    {
                        JObject msg = JObject.Parse(result.Content.ReadAsStringAsync().Result);

                        string translated = (string)msg["contents"]["translated"];

                        //Return response
                        return new OkObjectResult(translated);
                    }else
                    {
                        JObject error = JObject.Parse(result.Content.ReadAsStringAsync().Result);
                        return new BadRequestObjectResult(error);
                    }
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }

        }
    }

    public class PostData
    {
        public string text { get; set; }
    }
}
