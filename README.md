# TheClimbFace - Climbing Competition Management System

## 🧗‍♂️ About

TheClimbFace is a comprehensive web application designed for managing climbing competitions. It provides a complete solution for organizing, scoring, and tracking climbing events.

## ✨ Features

### 🏆 Competition Management
- Create and manage climbing competitions
- Set competition dates and Judges
- Manage competition details and settings

### 👥 Climber Management
- Register climbers with personal information
- Organize climbers by groups and gender
- Track climber performance and results

### 🎯 Scoring System
- Real-time scoring for boulder problems
- Zone and Top attempt tracking
- Automatic result calculations
- Performance analytics and rankings

### 👨‍⚖️ Arbitrator System
- Assign arbitrators to specific boulders
- Scoring interface
- Real-time result updates


## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK
- SQL Server or SQLite
- Visual Studio 2022 or VS Code

### Installation Steps

#### Step 1: Configure Connection String
Open `appsettings.json` and update the connection string:


#### Step 2: Update Database
Run the following commands in the project directory:

```bash
dotnet ef database update
```

#### Step 3: Run the Application
Start the application:

```bash
dotnet run
```

The application will be available at `https://localhost:5001` or `http://localhost:5000`

## 🏗️ Architecture

### Project Structure
```
TheClimbFace/
├── TheClimbFace.Common/          # Shared utilities and validations
├── TheClimbFace.Data/            # Data access layer and repositories
├── TheClimbFace.Data.Models/     # Entity models
├── TheClimbFace.Extensions/      # Extension methods
├── TheClimbFace.Services.Data/   # Business logic services
├── TheClimbFace.Tests/           # Unit tests
├── TheClimbFace.Web/             # Web application (MVC)
└── TheClimbFace.Web.ViewModels/ # View models for UI
```

### Technology Stack
- **Backend**: ASP.NET Core 8.0 MVC
- **Database**: Entity Framework Core with SQL Server/SQLite
- **Frontend**: Bootstrap 5, Font Awesome, JavaScript
- **Authentication**: ASP.NET Core Identity
- **Testing**: NUnit

## 🎨 User Interface

### Key Pages
- **Home**: Overview of active competitions
- **Competitions**: Manage your competitions
- **Climbers**: Register and manage participants
- **Scoring**: Real-time scoring interface
- **Results**: View competition results and rankings

### Code Style
The project follows C# coding conventions and uses:
- Async/await patterns
- Repository pattern
- Service layer architecture
- ViewModel pattern for UI


---

**TheClimbFace** - Making climbing competitions easier to manage! 🧗‍♂️🏆 