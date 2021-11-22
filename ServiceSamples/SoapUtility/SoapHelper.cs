using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace SoapUtility
{
    public class SoapHelper
    {
        public static string GetSoapServiceUriString(string serviceName, string aosUriString)
        {
            string soapServiceUriStringTemplate = "{0}/soap/services/{1}";
            string soapServiceUriString = string.Format(soapServiceUriStringTemplate, aosUriString.TrimEnd('/'), serviceName);
            return soapServiceUriString;
        }

        public static Binding GetBinding()
        {
            var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);

            // Set binding timeout and other configuration settings
            binding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            binding.ReaderQuotas.MaxArrayLength = int.MaxValue;
            binding.ReaderQuotas.MaxNameTableCharCount = int.MaxValue;

            binding.ReceiveTimeout = TimeSpan.MaxValue;
            binding.SendTimeout = TimeSpan.MaxValue;
            binding.MaxReceivedMessageSize = int.MaxValue;

            var httpsTransportBindingElement = binding.CreateBindingElements().OfType<HttpsTransportBindingElement>().FirstOrDefault();
            if (httpsTransportBindingElement != null)
            {
                httpsTransportBindingElement.MaxPendingAccepts = 10000; // Largest posible is 100000, otherwise throws
            }

            var httpTransportBindingElement = binding.CreateBindingElements().OfType<HttpTransportBindingElement>().FirstOrDefault();
            if (httpTransportBindingElement != null)
            {
                httpTransportBindingElement.MaxPendingAccepts = 10000; // Largest posible is 100000, otherwise throws
            }

            return binding;
        }
    }
}
