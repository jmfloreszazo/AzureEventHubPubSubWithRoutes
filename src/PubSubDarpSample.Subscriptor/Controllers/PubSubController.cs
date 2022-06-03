using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using PubSubRouting.Models;
using System.Text.Json;
using Azure.Messaging;
using Dapr.Client;


namespace PubSubAzureEventHub.Controllers
{
    [ApiController]
    public class PubSubController : ControllerBase
    {
        [HttpPost("widgets")]
        public IActionResult Widget([FromBody] CloudEvent<Widget> domainEvent)
        {
            Console.WriteLine("WIDGET");
            return this.Ok();
        }

        [HttpPost("gadgets")]
        public IActionResult Gadget(CloudEvent<Gadget> domainEvent)
        {
            Console.WriteLine("GADGET");
            return this.Ok();
        }

        [HttpPost("products")]
        public IActionResult Subscribe([FromBody] CloudEvent<Product> domainEvent)
        {
            Console.WriteLine("PRODUCT (default)");
            return this.Ok();
        }

        [HttpPost("publish")]
        async public Task<IActionResult> Publish()
        {
            byte[] content;
            using (var ms = new MemoryStream(2048))
            {
                await Request.Body.CopyToAsync(ms);
                content = ms.ToArray();
            }

            WebRequest request =
                WebRequest.Create("http://127.0.0.1:3500/v1.0/publish/pubsub/inventory");
            request.Method = "POST";
            request.ContentType = "application/cloudevents+json";
            request.ContentLength = content.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(content, 0, content.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            Console.WriteLine(responseFromServer);
            reader.Close();
            dataStream.Close();
            response.Close();
            return this.Ok("OK");

        }


    }
}