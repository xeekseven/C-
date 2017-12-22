using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AsyncIO
{
    public class AsyncHttpServer
    {
        readonly HttpListener _listener;
        const string RESPONSE_TEMPLATE =
            "<html><head><title>Test</title></head><body><h2>testc</h2>Today is {0}</body></html>";

        public AsyncHttpServer(int portNumber)
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add(string.Format("http://+:{0}/", portNumber));
            _listener.Prefixes.Add(string.Format("http://+:{0}/msg/", portNumber));
        }
        public async Task Start()
        {
            _listener.Start();
            while (true)
            {
                var ctx = await _listener.GetContextAsync();
                Console.WriteLine("Client content ,,");
                string response = string.Format(RESPONSE_TEMPLATE, DateTime.Now);
                using (var sw = new StreamWriter(ctx.Response.OutputStream))
                {
                    await sw.WriteAsync(response);
                    await sw.FlushAsync();
                }
            }
        }
        public async Task Stop()
        {
           _listener.Abort();
        }
    }
}
