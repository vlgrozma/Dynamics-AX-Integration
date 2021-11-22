using AuthenticationUtility;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace JsonConsoleApplication
{
    public class Program
    {
        // In the AOT you will find SysMessageServices in Service Groups and SysMessageService under Services.
        private const string messageUrl = "/api/services/SysMessageServices/SysMessageService/SendMessage";
        
        public static async Task Main(string[] args)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation(OAuthHelper.OAuthHeader, OAuthHelper.GetAuthenticationHeader(useWebAppAuthentication: false));

            string messagePath = string.Format("{0}{1}", ClientConfiguration.Default.UriString.TrimEnd('/'), messageUrl);

            // See https://docs.microsoft.com/en-us/dynamics365/supply-chain/production-control/mes-integration
            // for the various message types and their supported parameters.
            var messageContent = new
            {
                ProductionOrderNumber = "P000210"
            };

            var message = new
            {
                _companyId = "USMF",
                _messageQueue = "JmgMES3P",
                _messageType = "ProdProductionOrderStart",
                _messageContent = JsonSerializer.Serialize(messageContent)
            };

            var postRequest = new HttpRequestMessage(HttpMethod.Post, messagePath)
            {
                Content = JsonContent.Create(message)
            };

            var postResponse = await client.SendAsync(postRequest);

            postResponse.EnsureSuccessStatusCode();

            Console.WriteLine("Message posted.");

            Console.ReadLine();
        }

    }
}
