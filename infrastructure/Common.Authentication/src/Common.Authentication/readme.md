# 🚀 Play.Security.JwtAuthentication 🚀

This project provides a secure way to handle JSON Web Token (JWT) authentication in .NET applications. It includes a `JwtTokenHandler` class that generates JWTs for authenticated users.

The `JwtTokenHandler` class uses a repository to retrieve user account details, validates the provided username and password, and generates a JWT if the credentials are valid. The JWT includes claims for the username and user role, and it expires after a specified amount of time.

The project uses the `System.IdentityModel.Tokens.Jwt` and `Microsoft.IdentityModel.Tokens` libraries to handle the creation and signing of the JWT.

## 🔧 Installation

### 🔑 JWT security key

To store the JWT security key in an environment variable, you can follow these steps:

- Set the environment variable on your system. The process for this varies depending on your operating system:

- On Windows, you can use the setx command in the command prompt:
  
```PowerShell
    ## To create the security key
    $bytes = New-Object Byte[] 32
    [Security.Cryptography.RNGCryptoServiceProvider]::Create().GetBytes($bytes)
    [System.Convert]::ToBase64String($bytes)
    ## To set the environment variable
    setx JWT_SECURITY_KEY "long_security_string_44_characters"
```

   Note: Please note that the setx command permanently sets the value of the environment variable. However, the new value will not be available in the current command prompt session. You need to open a new command prompt window to see the new value.

- On Linux or macOS, you can use the export command in the terminal:

```bash
    ## To create the security key
    openssl rand -base64 32
    ## To set the environment variable (only available for the current command prompt session)
    export JWT_SECURITY_KEY="long_security_string_44_characters"
    ## TTo set the environment variable permanently type the following command
    echo 'export JWT_SECURITY_KEY="aedcmVTADemkv50+KXIgv8cWEEYy7+lobCKcWC+kS28="' >> ~/.zshrc
````

### To create the project

These are the command necessary to create this project

```bash
dotnet new classlib -n JwtAuthentication   

dotnet add package Microsoft.IdentityModel.Tokens   
dotnet add package System.IdentityModel.Tokens.Jwt   
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Microsoft.Extensions.DependencyInjection.Abstractions
```

## 🛠️ Usage

To use the `JwtTokenHandler` class, create an instance and call the `GenerateJwtToken` method, passing in an `AuthenticationRequest` object that includes the username and password. The method will return an `AuthenticationResponse` object that includes the JWT and its expiry time if the credentials are valid.

### 🔍 In detail

First, you need to create an instance of the `JwtTokenHandler` class:

```csharp
var jwtTokenHandler = new JwtTokenHandler();
```

Next, create an AuthenticationRequest object with the username and password:

```csharp
var authenticationRequest = new AuthenticationRequest
{
    UserName = "exampleUser",
    Password = "examplePassword"
};
```

Then, call the GenerateJwtToken method to get an AuthenticationResponse object:

```csharp
var authenticationResponse = await jwtTokenHandler.GenerateJwtToken(authenticationRequest);
```

The AuthenticationResponse object will contain the JWT and its expiry time if the credentials are valid. You can access these properties like so:

```csharp
var jwtToken = authenticationResponse.JwtToken;
var jwtTokenExpiryTime = authenticationResponse.JwtTokenExpiryTime;
```

Please note that this is a basic example and your actual usage may vary depending on the specifics of your project.

Remember to replace "exampleUser" and "examplePassword" with actual username and password.

## 🤝 Contributing

Contributions are what make the open-source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

This project needs to be upgrated in order to read user information from a database. For the time being it's just a mock up based on hard coded values.

1. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
2. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
3. Push to the Branch (`git push origin feature/AmazingFeature`)
4. Open a Pull Request

## 🔬 Testing

This project is tested (unit test) using Xunit in a separate project: Play.Security.JwtAuthentication.Tests

## 📃 License

This project is licensed under the terms of the MIT License. This means you are free to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the software, under the following conditions:

1. You must include a copy of the MIT License terms and the copyright notice in all copies or substantial portions of the software.

2. The software is provided "as is", without warranty of any kind, express or implied, including but not limited to the warranties of merchantability, fitness for a particular purpose and noninfringement.

For more details, see the [LICENSE](LICENSE) file in the project repository.

## 📫 Contact

If you have any questions, issues, or if you want to contribute, feel free to reach out:

- **GitHub**: [Your GitHub Profile Link]
- **Email**: [Your Email Address]
- **Twitter**: [Your Twitter Handle]

Please ensure that you adhere to our [Code of Conduct](CODE_OF_CONDUCT.md) when interacting with the project.
