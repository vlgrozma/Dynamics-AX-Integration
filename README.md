# Dynamics-AX-Integration-MES3P
Dynamics AX MES 3P Integration samples and demos.

Instructions:
1. Build. Any warnings can usually be ignored.
2. Unload the the Utilities/ODataUtility project (right-click -> Unload Project) to enable code completion and highlighting for OData entities.
This project contains a 100 MB auto-generated source file, which is too big for Visual Studio.
3. Insert the endpoints and credentials into Utilities/AuthenticationUtility/ClientConfiguration.cs (the build should be much faster with ODataUtility unloaded).
4. Run the samples, making sure that useWebAppAuthentication is set correctly for the authentication method you want to use in ODataConsoleApplication and JsonConsoleApplication.
