using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.IO;

namespace AsyncIO
{
    public class OwnHttpServerTools
    {
        public static async Task GetResponseAsync(string url)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage responseMsg = await client.GetAsync(url);
                string responseHeader = responseMsg.Headers.ToString();
                string response = await
                    responseMsg.Content.ReadAsStringAsync();
                Console.WriteLine("Response headers:{0}", responseHeader);
                Console.WriteLine("Response body:{0}", response);
            }
        }
        public static void InvokeHttpS()
        {
            var server = new AsyncHttpServer(portNumber:1234);
            var t = Task.Run(() => server.Start());
            Console.WriteLine("Listening on port 1234 open http://localhost:1234 in your browser");
            Console.WriteLine("Trying to connect");
            Console.WriteLine();
            GetResponseAsync("http://localhost:1234").GetAwaiter().GetResult();
            Console.WriteLine();
            Console.WriteLine("Press enter to stop the server");
            Console.ReadLine();

            server.Stop().GetAwaiter().GetResult();
        }
    }
}
