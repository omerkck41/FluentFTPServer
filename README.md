# FluentFtpServer (v1.0.0)

A modern, high-level **.NET 10.0** wrapper for **FluentFTP**, designed for high-performance enterprise applications.  
Built with **Dependency Injection**, **Options Pattern**, and **Structured Logging** support.

[![.NET CI](https://github.com/omerkck41/FluentFTPServer/actions/workflows/ci.yml/badge.svg)](https://github.com/omerkck41/FluentFTPServer/actions/workflows/ci.yml)

## ✨ Why FluentFtpServer?

- **🚀 .NET 10 Native:** Fully optimized for the latest .NET runtime.
- **🏗️ Clean Architecture Ready:** Designed to be easily integrated into Infrastructure layers.
- **💉 Built-in DI:** Simple `AddFluentFtpServer()` extension for Microsoft Dependency Injection.
- **📜 Structured Logging:** Integrated with `ILogger` for full traceability.
- **🛠️ Builder Pattern:** Flexible client creation for legacy or non-DI projects.
- **🔒 Secure by Default:** Explicit SSL/TLS support for encrypted transfers.
- **✅ Zero Warnings:** 100% XML Documentation for perfect IntelliSense support.

---

## 📦 Installation

Add this project as a submodule or project reference in your GitHub repo.

```bash
git submodule add https://github.com/omerkck41/FluentFTPServer.git
```

Or reference the project in your `.csproj`:

```xml
<ItemGroup>
  <ProjectReference Include="..\FluentFtpServer\FluentFtpServer.csproj" />
</ItemGroup>
```

---

## 🛠️ Configuration

Add the following section to your `appsettings.json`:

```json
{
  "Ftp": {
    "Server": "ftp.yourserver.com",
    "Port": 21,
    "Username": "your_username",
    "Password": "your_password",
    "UseSsl": true,
    "TimeoutSeconds": 15,
    "RetryAttempts": 3,
    "MainPath": "/uploads"
  }
}
```

---

## 🏗️ Clean Architecture Integration

In a Clean Architecture project, you should register this library in your **Infrastructure** or **Persistence** layer.

### 1. Register Services
In your `Program.cs` (or `DependencyInjection.cs` in the Infrastructure layer):

```csharp
using FluentFtpServer;

// Option A: Bind directly from IConfiguration (Recommended)
builder.Services.AddFluentFtpServer(builder.Configuration);

// Option B: Manual configuration via Action
builder.Services.AddFluentFtpServer(options => {
    options.Server = "ftp.example.com";
    options.Username = "admin";
    options.Password = "123456";
    options.MainPath = "/root";
});
```

### 2. Usage in Application Logic
Inject `IFtpClient` into your services or handlers:

```csharp
using FluentFtpServer;

public class FileUploadService(IFtpClient ftpClient, ILogger<FileUploadService> logger)
{
    public async Task ProcessFileAsync(string localPath)
    {
        // 1. Check if directory exists
        if (!await ftpClient.DirectoryExistsAsync("daily_reports"))
        {
            await ftpClient.CreateDirectoryAsync("daily_reports");
        }

        // 2. Upload the file
        bool success = await ftpClient.UploadFileAsync(localPath, "daily_reports/report.pdf");

        if (success)
            logger.LogInformation("File uploaded successfully.");
    }
}
```

---

## 🛠️ Non-DI / Legacy Usage

If you are not using Dependency Injection, you can use the **Builder Pattern**:

```csharp
using FluentFtpServer;

var ftpClient = new FtpClientBuilder()
    .WithServer("ftp.example.com")
    .WithCredentials("user", "pass")
    .WithSsl(true)
    .WithMainPath("/data")
    .WithTimeout(30)
    .Build();

var files = await ftpClient.ListDirectoryAsync("/");
```

---

## 📚 API Reference

| Method | Description |
|--------|-------------|
| `ConnectAsync()` | Asynchronously connects to the server. |
| `UploadFileAsync(local, remote)` | Uploads a single file. |
| `DownloadFileAsync(remote, local)` | Downloads a single file. |
| `FileExistsAsync(path)` | Checks if a file exists. |
| `DirectoryExistsAsync(path)` | Checks if a directory exists. |
| `CreateDirectoryAsync(path)` | Creates a new directory. |
| `ListDirectoryAsync(path)` | Lists all items in a directory. |
| `DeleteDirectoryAsync(path)` | Deletes a directory. |

---

## 🧪 Testing

The library includes a `FluentFtpServer.Tests` project using **xUnit** and **Moq**.

```bash
dotnet test
```

---

## ⚖️ License

Distributed under the **MIT License**. See `LICENSE` for more information.

---
Created with ❤️ by [omerkck41](https://github.com/omerkck41)
