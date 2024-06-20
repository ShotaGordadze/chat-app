# ChatApp

**EFCore *connection-string***: `User ID={dbUser};Password={pass};Host={host};Port={port};Database={dbName};Pooling=true;`

* Add EFCore Migration cmd: `dotnet ef migrations add InitialCreate -p ./Infrastructure/Infrastructure.csproj -s ./ChatAppWebApi/ChatAppWebApi.csproj -c MessagesDbContext`
* Update DB EFCore cmd: `dotnet ef database update -p ./Infrastructure/Infrastructure.csproj -s ./ChatAppWebApi/ChatAppWebApi.csproj -c MessagesDbContext`