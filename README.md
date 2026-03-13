# FluentFtpServer

A modern, high-level .NET 10.0 wrapper for **FluentFTP**, designed for simplicity and reliability.  
Built with **Builder Pattern** support and optimized for modern C# features.

[![.NET CI](https://github.com/your-username/FluentFtpServer/actions/workflows/ci.yml/badge.svg)](https://github.com/your-username/FluentFtpServer/actions/workflows/ci.yml)

## ✨ Features

- **.NET 10.0 Target:** Fully leverages the latest .NET runtime optimizations.
- **Fluent Builder API:** Easy configuration using `FtpClientBuilder`.
- **Async/Await Support:** All I/O operations are non-blocking.
- **SSL/TLS Support:** Secure connections via Explicit Encryption Mode.
- **Modern C# 13:** Utilizes file-scoped namespaces, primary constructors, and more.

## 📦 Installation

Add this project as a submodule or project reference in your GitHub repo.

```bash
git submodule add https://github.com/your-username/FluentFtpServer.git
```

## 🚀 Quick Start

```csharp
using FluentFtpServer;

// Build the FTP client
var client = new FtpClientBuilder()
    .WithServer("ftp.example.com")
    .WithCredentials("username", "password")
    .WithSsl(true)
    .WithMainPath("/upload")
    .Build();

// Upload a file
bool success = await client.UploadFileAsync("local.txt", "remote.txt");
```

## 🛠️ Configuration Options

| Method | Description |
|--------|-------------|
| `.WithServer(string)` | Sets the FTP server host. |
| `.WithPort(int)` | Sets the port (default: 21). |
| `.WithCredentials(user, pass)` | Sets login details. |
| `.WithSsl(bool)` | Enables SSL/TLS (Explicit). |
| `.WithMainPath(string)` | Sets the base directory for all operations. |

## ⚖️ License

Distributed under the **MIT License**. See `LICENSE` for more information.
