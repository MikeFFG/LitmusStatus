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
using System.Xml;
using System.Xml.Linq;

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

        public ActionResult Browsers()
        {
            return View();
        }

        public ActionResult Foo()
        {
            var model = new Client { ClientName = "bar" };
            return View(model);
        }

        public List<Client> ParseXML(string xmlString)
        {
            //XmlDocument xml = new XmlDocument;
            //xml.LoadXml(xmlString);
            List<Client> clients = new List<Client>();

            XDocument doc;
            using (StringReader s = new StringReader(xmlString))
            {
                doc = XDocument.Load(s);
            }

            var applications = doc.Descendants("testing_application");

            foreach (var application in applications)
            {

                clients.Add(new Client()
                {
                    TimeInS = application.Element("average_time_to_process").Value,
                    AppCode = application.Element("application_code").Value,
                    ClientName = application.Element("application_long_name").Value,
                    CurrentStatus = application.Element("status").Value
                });
            }
            
            return clients;
        }

        public async Task<ActionResult> EmailClients()
        {
            string xml = await GetEmailTestStatus();
            List<Client> clients = new List<Client>();
            clients = ParseXML(xml);
            return View(clients);
        }

        public async Task<string> GetEmailTestStatus()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            
            // Add Authorization header.
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic", "bWlrZS5kLmJ1cmtlQGdtYWlsLmNvbTpCYWRQYXNzd29yZA==");

            // Get response.
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;
            if (response.IsSuccessStatusCode)
            {
                String responseString = await response.Content.ReadAsStringAsync();
                return responseString;
            }
            else
            {
                return response.ReasonPhrase;
            }
        }


    }
}