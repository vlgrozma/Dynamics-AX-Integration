# Dynamics AX Integration (MES Third party)
Dynamics AX MES 3P Integration samples and demos.

## Building and running
1. Build the solution. Any warnings can usually be ignored.
2. Unload the Utilities/ODataUtility project (right-click -> Unload Project) to enable code completion and highlighting for OData entities.
This project contains a 100 MB auto-generated source file, which is too big for Visual Studio. The build should also be much faster with ODataUtility unloaded.
3. Insert the endpoints and credentials into Utilities/AuthenticationUtility/ClientConfiguration.cs. Update useWebAppAuthentication in ODataConsoleApplication and JsonConsoleApplication for the authentication method you want to use.
4. Open the Feature management workspace in Dynamics and enable the "Manufacturing execution system integration" feature.
5. Change the default company of the Dynamics user to USMF. To retrieve data outside of the default company through OData, you need to add the cross-company query parameter, e.g. using myLinqQuery.AddQueryOption("cross-company", true);

## Configuring AAD authentication (Dynamics development environment)
1. Assuming you're an external developer (and can't create an app in the Microsoft tenant), add your AAD Tenant to Aad.TrustedIssuers in web.config on your Dynamics development environment and restart IIS.
2. Go to App registrations in the Azure Portal and click + New registration. Give your app a name (any will do) and click Register.
3. Open the app in Azure Portal and navigate to Certificates & secrets. Click + New client secret, give it a description and click Add.
4. Copy the generated Value to the ActiveDirectoryClientAppSecret field in ClientConfiguration.cs and the Application (client) ID to ActiveDirectoryClientAppId.
5. Set UriString and ActiveDirectoryResource to your Dynamics URL (the latter without trailing /) and ActiveDirectoryTenant to https://login.windows.net/yourtenant.onmicrosoft.com
6. Change useWebAppAuthentication to true in ODataConsoleApplication and JsonConsoleApplication.
7. Open the Azure Active Directory applications form in Dynamics (SysAADClientTable) and map the Application ID of your application to a Dynamics user.

## Configuring Business Events using Azure Service Bus queues
1. Create another app using steps 2-3 above.
2. Go to Service Bus in the Azure Portal and click + Create. Give it a globally unique Namespace name and choose Standard or Premium for the Pricing tier. Click Review + create, then Create.
3. Wait for deployment to finish, then open the resource, navigate to Queues and click + Queue. Give it a name and click Create.
4. Navigate to Shared access policies, click RootManageSharedAccessKey (or create a new policy with the necessary permissions), and copy the Primary or Secondary Connection String.
5. Go to Key vaults in the Azure Portal and click + Create. Give it a name and click Next: Access policy. Click + Add Access Policy. Under Configure from template, select Secret management. Under Principal, select the Application ID of the app from step 1. Click Select, Add, Review + create and Create.
6. Wait for deployment to finish, then open the resource, navigate to Secrets and click + Generate/Import. Give it a name, paste the connection string from step 4 into the Value field and click Create.
7. Open the Business events catalog workspace in Dynamics (BusinessEventsWorkspace), navigate to the Endpoints tab and click + New. Select Azure Service Bus Queue and click Next. Give the Endpoint a name, then enter the Queue name (step 3), Service Bus SKU (step 2), Azure Active Directory application ID and secret (step 1), Key Vault DNS name (copy the Vault URI from the Overview page of the Key vault) and secret name (step 6). Click OK - this will send a test message to the queue.
8. Navigate back to the Business event catalog tab, select the BusinessEventsAlertEvent and the ProductionOrderReleasedBusinessEvent and click + Activate. Select the Legal entity (e.g. USMF) and the Endpoint you just created, and click OK.
9. In ConsoleApplications/BusinessEventListenerApplication/Program.cs, paste the connection string from step 4 into the ServiceBusClient constructor parameter in the ListenServiceBus() method, and the queue name into the CreateProcessor call on the next line. You can of course use different access keys/policies for reading and writing to the queue.

### Send a business event if a message fails
1. Open the Manufacturing execution system integration form in Dynamics (JmgMES3PMessageProcessorMessage) and select Options -> Create a custom alert.
2. Set Table name to "Processing state", Field to "Message state" and Event to "is set to: Failed". Under "Alert me for", uncheck Organization-wide. Under "Alert me with", check Send externally. You can also set a Dynamics user that will get an in-app notification, as well as send an email, although the latter requires office/exchange integration to be set up. Then click OK.
3. Open the Change based alerts dialog (Action:EventCUD) and click Recurrence under "Run in the background".
4. Check "No end date", set the Recurrence pattern to Minutes and Count to e.g. 1. Click OK twice.
