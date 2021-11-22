using AuthenticationUtility;
using SoapUtility.UserSessionServiceReference;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace SoapConsoleApplication
{
    public class Program
    {
        public const string UserSessionServiceName = "UserSessionService";

        [STAThread]
        public static void Main(string[] args)
        {
            string aosUriString = ClientConfiguration.Default.UriString;

            string oauthHeader = OAuthHelper.GetAuthenticationHeader();
            string serviceUriString = SoapUtility.SoapHelper.GetSoapServiceUriString(UserSessionServiceName, aosUriString);

            var endpointAddress = new EndpointAddress(serviceUriString);
            Binding binding = SoapUtility.SoapHelper.GetBinding();

            var client = new UserSessionServiceClient(binding, endpointAddress);
            IClientChannel channel = client.InnerChannel;

            UserSessionInfo sessionInfo = null;

            using (OperationContextScope operationContextScope = new OperationContextScope(channel))
            {
                 HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                 requestMessage.Headers[OAuthHelper.OAuthHeader] = oauthHeader;
                 OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;
                 sessionInfo = ((UserSessionService)channel).GetUserSessionInfo(new GetUserSessionInfo()).result;
            }

            Console.WriteLine();
            Console.WriteLine("User ID: {0}", sessionInfo.UserId);
            Console.WriteLine("Is Admin: {0}", sessionInfo.IsSysAdmin);
            Console.ReadLine();
        }
    }
}

