# Commands to create the project

```bash
dotnet new xunit -o Play.Security.JwtAuthentication.Tests 
dotnet sln add ./src/Play.Security.JwtAuthentication.Tests/Play.JwtAuthentication.Tests.csproj

dotnet add ./src/Play.Security.JwtAuthentication.Tests/Play.JwtAuthentication.Tests.csproj reference ./src/Play.Security.JwtAuthentication/Play.JwtAuthentication.csproj

dotnet add package Moq

```

## Commands to add the Dependency Injection framework

```bash
cd Play.Common.Tests
dotnet add package Xunit.Microsoft.DependencyInjection 
dotnet add package Microsoft.Extensions.DependencyInjection.Abstractions
dotnet add package Microsoft.Extensions.Configuration.Abstractions
```

## Commands to run the tests

```bash
dotnet test --logger "console;verbosity=detailed" 
```

## Steps

- Add the appsettings.json
- Add the Fixtures folder and associated class
- Add a reference to the Play.Commoun project
- Add a Services folder and associated classes
