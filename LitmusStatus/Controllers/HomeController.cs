using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using LitmusStatus.Models;
using System.Net;
using System.IO;

namespace LitmusStatus.Controllers
{
    public class DataObject
    {
        public string Name { get; set; }
    }

    public class HomeController : Controller
    {
        public string myString { get; set; }
        private const string URL = "https://mikedburke.litmus.com";
        private string urlParameters = "/tests/29308020.xml";

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EmailClients()
        {
            return View();
        }

        public ActionResult Browsers()
        {
            return View();
        }

        public async Task<string> GetEmailTestStatus()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic", "bWlrZS5kLmJ1cmtlQGdtYWlsLmNvbTpCYWRQYXNzd29yZA==");


            // List data response.
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;  // Blocking call!
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
                //var dataObjects = response.Content.ReadAsStringAsync<IEnumerable<DataObject>>().Result;
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return response.ReasonPhrase;
            }
        }


    }
}