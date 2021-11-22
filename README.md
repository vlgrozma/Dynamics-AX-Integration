# Dynamics-AX-Integration-MES3P
Dynamics AX MES 3P Integration samples and demos.

Instructions:
1. Build. Any warnings can usually be ignored.
2. Unload the the Utilities/ODataUtility project (right-click -> Unload Project) to enable code completion and highlighting for OData entities.
This project contains a 100 MB auto-generated source file, which is too big for Visual Studio. The build should also be much faster with ODataUtility unloaded.
3. Insert the endpoints and credentials into Utilities/AuthenticationUtility/ClientConfiguration.cs. Update useWebAppAuthentication in ODataConsoleApplication and JsonConsoleApplication for the authentication method you want to use.
4. Change the default company of the Dynamics user to USMF. To retrieve data outside of the default company through OData, you need to add the cross-company query parameter, e.g. using myLinqQuery.AddQueryOption("cross-company", true);
