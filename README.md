# Workhub API 🚀

[![Build Status](https://img.shields.io/badge/build-passing-brightgreen.svg)]()
[![Framework](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/)
[![Architecture](https://img.shields.io/badge/Architecture-Clean-orange.svg)]()
[![Database](https://img.shields.io/badge/Database-SQL%20Server-red.svg)]()

**Workhub** is a full-featured backend engine designed to power modern networking and employment platforms. It provides a robust, scalable foundation for job boards, professional social networks, and real-time community engagement tools.

---

## 🏗️ Architecture
The project is built following **Clean Architecture** principles, ensuring separation of concerns, testability, and independence from external frameworks.

- **Workhub.Api**: The entry point. Handles HTTP requests, middleware, and API configurations.
- **Workhub.Application**: Contains business logic, MediatR handlers (CQRS), and service interfaces.
- **Workhub.Domain**: The core of the application. Contains entities, domain logic, and custom errors.
- **Workhub.Infrastructure**: Realizes interfaces. Handles persistence (EF Core), external integrations (Firebase, Email), and logging.
- **Workhub.Contracts**: Defines the data transfer objects (DTOs) used for communication between the API and clients.

---

## ✨ Key Features

- **💼 Job Management**: Comprehensive system for posting, searching, and managing job opportunities.
- **👤 Professional Profiles**: Detailed user profiles with skill tracking and career history.
- **💬 Real-time Community**: Discussion posts and replies with SignalR-powered real-time updates.
- **🔐 Secure Authentication**: Multi-layer security including JWT Bearer tokens and API Key authentication.
- **🔔 Notification Engine**: Integrated with Firebase Cloud Messaging (FCM) for push notifications.
- **📧 Email Communications**: Built-in service for transactional emails and verifications.
- **📁 File Handling**: Optimized file upload settings for handling resumes and profile assets.

---

## 🛠️ Tech Stack

- **Framework**: .NET 8.0
- **Database**: Microsoft SQL Server / Entity Framework Core
- **Messaging**: Firebase Cloud Messaging (FCM)
- **Real-time**: SignalR
- **Communication**: MediatR (CQRS Pattern)
- **Logging**: Serilog
- **Documentation**: Swagger / OpenAPI

---

## 🚀 Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- A Firebase project (for `firebase.json`)

### Configuration
1. **Database Connection**: Update the `AppDataContext` connection string in `Workhub.Api/appsettings.json`.
2. **Firebase Setup**: Place your `firebase.json` file in the `Workhub.Api` directory.
3. **JWT Settings**: Configure your JWT Secret, Issuer, and Audience in `appsettings.json`.

### Run the Project
```bash
# Clone the repository
git clone https://github.com/dikachi-dev/workhub.git

# Navigate to the API folder
cd Workhub.Api

# Restore dependencies
dotnet restore

# Run migrations
dotnet ef database update --project ../Workhub.Infrastructure --startup-project .

# Start the application
dotnet run
```

---

## 📖 API Documentation
Once the project is running, you can access the interactive Swagger documentation at:
`http://localhost:5000/swagger` (or your configured port)

The API supports:
- **Authorization**: Bearer token in the `Authorization` header.
- **API Key**: Required in the `ApiKey` header for specific endpoints.

---

## 🐳 Docker Support
The project includes a `Dockerfile` for easy containerization.
```bash
docker build -t workhub-api .
docker run -p 8080:8080 workhub-api
```

---

## 🤝 Contributing
1. Fork the project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

*Built with ❤️ for the development community.*