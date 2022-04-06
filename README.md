## *SSL Validator*
*Visuallise when domains are about to expire*

### Technologies Used
- Framework: Blazor Web App
- Language: C#, Razor, CSS, Docker
- Project Template: Blazor App, ASP.NET Core hosted
- Important variables
  - checkDomainsIntervalMinutes: How often to refresh the sessions in redis cache
  - redis: A redis connection string where you want current sessions to be stored

### Set up
- Development
  - Install ASP.NET 6 SDK
  - Have a local instace of redis db running
  - Copy appsettings.example.json -> appsettings.json
  - Set all of the variables in appsettings.json 
  - Navigate to SSLValidator/Server
  - **dotnet run**
- Production
  - Install docker and docker-compose
  - Copy appsettings.example.json -> appsettings.json
  - Set all of the variables in appsettings.json
  - Get a .pfx certificate and configure it in appsettings.json
  - **Run docker-compose up**
