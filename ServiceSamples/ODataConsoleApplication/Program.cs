using AuthenticationUtility;
using Microsoft.OData.Client;
using ODataUtility.Microsoft.Dynamics.DataEntities;
using System;

namespace ODataConsoleApplication
{
    public class Program
    {
        private static readonly string ODataEntityPath = ClientConfiguration.Default.UriString + "data";

        public static void Main(string[] args)
        {
            // To test custom entities, regenerate "ODataClient.tt" file.
            // https://blogs.msdn.microsoft.com/odatateam/2014/03/11/tutorial-sample-how-to-use-odata-client-code-generator-to-generate-client-side-proxy-class/

            Uri oDataUri = new Uri(ODataEntityPath, UriKind.Absolute);
            var context = new Resources(oDataUri);

            context.SendingRequest2 += new EventHandler<SendingRequest2EventArgs>(delegate (object sender, SendingRequest2EventArgs e)
            {
                var authenticationHeader = OAuthHelper.GetAuthenticationHeader(useWebAppAuthentication: false);
                e.RequestMessage.SetHeader(OAuthHelper.OAuthHeader, authenticationHeader);
            });

            //QueryExamples.ProductionOrderEntity(context);
            //QueryExamples.CustomerEntity(context);
            QueryExamples.ReleasedProductsEntity(context);
            //QueryExamples.SalesOrderEntity(context);

            Console.ReadLine();
        }

    }
}
