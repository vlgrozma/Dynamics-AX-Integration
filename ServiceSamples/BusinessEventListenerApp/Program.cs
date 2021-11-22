using Azure.Messaging.ServiceBus;
using System;
using System.Net;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace BusinessEventListenerApplication
{
    public class Program
    {
        private static void ListenHttp()
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:12340/");
            listener.Start();

            Console.WriteLine("Listening...");

            while (true)
            {
                // Note: The GetContext method blocks while waiting for a request.
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;

                using (Stream stream = request.InputStream)
                {
                    using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8))
                    {
                        Console.WriteLine(streamReader.ReadToEnd());
                    }
                }

                // Obtain a response object.
                HttpListenerResponse response = context.Response;

                // Construct a response.
                string responseString = "<html><body>Hello world!</body></html>";
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);

                // Get a response stream and write the response to it.
                response.ContentLength64 = buffer.Length;
                Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);

                // You must close the output stream.
                output.Close();
            }

            listener.Stop();
        }

        private static async Task ListenServiceBus()
        {
            ServiceBusClient client = null;
            ServiceBusProcessor processor = null;

            try
            {
                client = new ServiceBusClient("Endpoint=sb://servicebustest051021.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=");

                processor = client.CreateProcessor("mybusinesseventqueue");
                processor.ProcessMessageAsync += MessageHandler;
                processor.ProcessErrorAsync += ErrorHandler;

                await processor.StartProcessingAsync();

                Console.WriteLine("Listening, press any key to stop.");
                Console.ReadKey();

                await processor.StopProcessingAsync();
            }
            finally
            {
                if (processor != null)
                {
                    await processor.DisposeAsync();
                }
                if (client != null)
                {
                    await client.DisposeAsync();
                }
            }
        }

        private class BusinessEventBase
        {
            public string BusinessEventId { get; set; }
        }

        private static async Task MessageHandler(ProcessMessageEventArgs args)
        {
            var body = args.Message.Body;
            var messageBase = body.ToObjectFromJson<BusinessEventBase>();
            Console.WriteLine($"Received message of type {messageBase.BusinessEventId}: {body}");

            // Complete the message. Message is deleted from the queue. 
            await args.CompleteMessageAsync(args.Message);
        }

        private static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception);
            return Task.CompletedTask;
        }

        public static async Task Main(string[] args)
        {
            //ListenHttp();

            await ListenServiceBus();
        }

    }
}
