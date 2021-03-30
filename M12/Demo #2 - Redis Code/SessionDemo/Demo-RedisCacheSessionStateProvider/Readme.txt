It has been kept simple to easily explain the concept to the attendees.

Steps:
1. This is a simple web application which uses Custom Session State Provider to store session values using Azure Redis Cache
2. Explain the Microsoft.Web.RedisSessionStateProvider Reference from Nuget.
3. Explain the custom session state provider section in web.config file.
4. Explain the code in Default.aspx (which is pretyt simple and straighforward.
5. Modify the web.config file to replace with your actual hostname and connection key.
6. Build and Execute.
7. In the browser, put in the value in 1st textbox (which would be stored in session backed by Redis cache) and click on Button "Add to Session"
8. In the browser, click on Button "Retrieve from Session" to retrieve the value from session and display in 2nd Textbox.