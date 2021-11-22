using System;

namespace AuthenticationUtility
{
    public partial class ClientConfiguration
    {
        public static ClientConfiguration Default { get { return ClientConfiguration.OneBox; } }

        public static ClientConfiguration OneBox = new ClientConfiguration()
        {
            UriString =
            "https://usnconeboxax1aos.cloud.onebox.dynamics.com/",

            // You only need to populate this section if you are logging on via a native app. For Service to Service scenarios in which you e.g. use a service principal you don't need that.
            UserName =
            "tusr1@TAEOfficial.ccsctp.net",
            // Insert the correct password here for the actual test.
            Password =
            #region ********
            "",
            #endregion

            // You need this only if you logon via service principal using a client secret. See: https://docs.microsoft.com/en-us/dynamics365/unified-operations/dev-itpro/data-entities/services-home-page to get more data on how to populate those fields.
            // You can find that under AAD in the azure portal
            ActiveDirectoryResource = // Don't have a trailing "/". Note: Some of the sample code handles that issue.
            "https://usnconeboxax1aos.cloud.onebox.dynamics.com",
            ActiveDirectoryTenant = // Some samples: https://login.windows.net/yourtenant.onmicrosoft.com, https://login.windows.net/microsoft.com
            "https://login.windows-ppe.net/TAEOfficial.ccsctp.net",
            ActiveDirectoryClientAppId =
            "d8a9a121-b463-41f6-a86c-041272bdb340",
            // Insert here the application secret when authenticating with AAD by the application
            ActiveDirectoryClientAppSecret =
            "",
        };

        public string UriString { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ActiveDirectoryResource { get; set; }
        public String ActiveDirectoryTenant { get; set; }
        public String ActiveDirectoryClientAppId { get; set; }
        public string ActiveDirectoryClientAppSecret { get; set; }
    }
}
