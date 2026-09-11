# TaskManagementAPI 🚀

A complete, beginner-friendly **Task Management REST API** built with **.NET 8 Web API**, **C#**, **PostgreSQL**, **Entity Framework Core**, **Swagger / OpenAPI**, **JWT Authentication**, and **xUnit** unit testing.

---

## 📁 Complete Project Structure

```text
TaskManagementAPI/
│
├── TaskManagementAPI/                # Core Web API Project
│   ├── Controllers/                  # API Controllers
│   │   ├── AuthController.cs         # POST /api/auth/register, POST /api/auth/login
│   │   ├── TasksController.cs        # Task CRUD endpoints (User-authenticated)
│   │   └── HealthController.cs       # GET /health endpoint
│   │
│   ├── Models/                       # Domain Entities
│   │   ├── User.cs                   # User Entity (Id, Name, Email, PasswordHash, CreatedAt)
│   │   ├── TaskItem.cs               # Task Entity (Id, Title, Description, Status, Priority, Dates, UserId)
│   │   ├── TaskStatus.cs             # Enum: Pending, InProgress, Completed
│   │   └── TaskPriority.cs           # Enum: Low, Medium, High
│   │
│   ├── DTOs/                         # Data Transfer Objects (Validation & Serialization)
│   │   ├── RegisterDto.cs            # Name, Email, Password
│   │   ├── LoginDto.cs               # Email, Password
│   │   ├── AuthResponseDto.cs        # JWT Token, Expiry, User Info
│   │   ├── CreateTaskDto.cs          # Title, Description, Status, Priority
│   │   ├── UpdateTaskDto.cs          # Title, Description, Status, Priority
│   │   └── TaskResponseDto.cs        # Formatted Task Output
│   │
│   ├── Services/                     # Business Logic Layer
│   │   ├── IAuthService.cs & AuthService.cs           # Auth & Hashing Logic
│   │   ├── ITaskService.cs & TaskService.cs           # User-Scoped Task CRUD Logic
│   │   └── IJwtTokenGenerator.cs & JwtTokenGenerator.cs # JWT Generation
│   │
│   ├── Repositories/                 # Data Access Layer
│   │   ├── IUserRepository.cs & UserRepository.cs     # User Queries & Persistence
│   │   └── ITaskRepository.cs & TaskRepository.cs     # Task Queries & Persistence
│   │
│   ├── Data/                         # EF Core Database Context & Seeding
│   │   ├── AppDbContext.cs           # DbContext with Npgsql PostgreSQL Provider
│   │   └── DbInitializer.cs          # Database Seeder (Demo User & Tasks)
│   │
│   ├── Middleware/                   # Custom HTTP Pipeline Middleware
│   │   └── ExceptionHandlingMiddleware.cs # Global Exception Handling & JSON Error Formatting
│   │
│   ├── Helpers/                      # Utility Helpers
│   │   ├── PasswordHasher.cs         # PBKDF2/HMAC-SHA256 Password Hashing & Verification
│   │   └── JwtSettings.cs            # JWT Configuration Options Model
│   │
│   ├── Migrations/                   # EF Core PostgreSQL Database Migrations
│   │   ├── 20260911000000_InitialCreate.cs
│   │   ├── 20260911000000_InitialCreate.Designer.cs
│   │   └── AppDbContextModelSnapshot.cs
│   │
│   ├── Program.cs                    # Application Entrypoint, DI, Auth & Swagger Config
│   ├── appsettings.json              # Connection Strings & JWT Settings
│   └── appsettings.Development.json
│
├── TaskManagementAPI.Tests/          # Unit Test Project (xUnit & Moq)
│   ├── Services/
│   │   ├── AuthServiceTests.cs       # Unit tests for User Register & Login
│   │   └── TaskServiceTests.cs       # Unit tests for Task CRUD & Data Isolation
│   └── TaskManagementAPI.Tests.csproj
│
├── TaskManagementAPI.sln             # Visual Studio / .NET Solution File
└── README.md                         # Detailed Instructions & Documentation
```

---

## 🛠️ Requirements & Tech Stack

- **.NET 8 SDK**
- **C# 12**
- **PostgreSQL Database**
- **Entity Framework Core 8** (`Npgsql.EntityFrameworkCore.PostgreSQL`)
- **Swagger / OpenAPI** with Bearer JWT UI integration
- **xUnit** & **Moq** for unit testing

---

## 🗄️ PostgreSQL Setup Guide

### Option A: Local PostgreSQL Installation (macOS / Linux / Windows)
1. Install PostgreSQL using standard package managers:
   - **macOS (Homebrew)**: `brew install postgresql@16 && brew services start postgresql@16`
   - **Ubuntu/Debian**: `sudo apt update && sudo apt install postgresql postgresql-contrib`
   - **Windows**: Download installer from [PostgreSQL Official Website](https://www.postgresql.org/download/).

2. Connect to PostgreSQL shell and create database:
   ```sql
   CREATE DATABASE "TaskManagementDb";
   ```

### Option B: Run PostgreSQL via Docker
If you prefer Docker:
```bash
docker run --name postgres-taskmanagement \
  -e POSTGRES_DB=TaskManagementDb \
  -e POSTGRES_USER=postgres \
  -e POSTGRES_PASSWORD=postgres \
  -p 5432:5432 -d postgres:latest
```

---

## ⚙️ Configuration (`appsettings.json`)

Edit `TaskManagementAPI/appsettings.json` to match your database credentials and secret key:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=TaskManagementDb;Username=postgres;Password=postgres"
  },
  "JwtSettings": {
    "Secret": "SuperSecretJwtSigningKeyMustBeAtLeast32BytesLong123!",
    "Issuer": "TaskManagementAPI",
    "Audience": "TaskManagementUsers",
    "ExpiryInMinutes": 60
  }
}
```

---

## 🚀 Commands to Restore, Build, Migrate, Run & Test

Open your terminal in the solution root directory (`TaskManagementAPI/` directory):

### 1. Restore NuGet Packages
```bash
dotnet restore
```

### 2. Build Solution
```bash
dotnet build
```

### 3. Apply Database Migrations (Create Tables)
```bash
cd TaskManagementAPI
dotnet ef database update
cd ..
```
*(Or simply launch the application — the application auto-applies pending migrations on startup if PostgreSQL is running).*

### 4. Run the API Server
```bash
dotnet run --project TaskManagementAPI
```

The application will start listening on:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`
- Swagger UI: `http://localhost:5000/swagger`

### 5. Run Unit Tests
```bash
dotnet test
```

---

## 🔐 How to Authorize in Swagger UI

1. Open `http://localhost:5000/swagger` in your browser.
2. Execute **`POST /api/auth/register`** or **`POST /api/auth/login`**.
3. Copy the returned `token` string from the JSON response.
4. Click the **`Authorize 🔓`** button at the top right of the Swagger UI page.
5. In the input box, type:
   ```text
   Bearer YOUR_JWT_TOKEN_HERE
   ```
6. Click **Authorize**, then **Close**. Now all protected Task endpoints are authenticated!

---

## 📡 API Endpoint Examples (cURL & JSON)

### 1. Health Check Endpoint
**`GET /health`**
```bash
curl -X GET "http://localhost:5000/health"
```
**Response (200 OK):**
```json
{
  "status": "Healthy",
  "timestamp": "2026-09-11T05:30:00Z",
  "service": "TaskManagementAPI",
  "version": "1.0.0"
}
```

---

### 2. User Registration
**`POST /api/auth/register`**
```bash
curl -X POST "http://localhost:5000/api/auth/register" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Jane Developer",
    "email": "jane@example.com",
    "password": "Password123!"
  }'
```
**Response (201 Created):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresAt": "2026-09-11T06:30:00Z",
  "user": {
    "id": 1,
    "name": "Jane Developer",
    "email": "jane@example.com",
    "createdAt": "2026-09-11T05:30:00Z"
  }
}
```

---

### 3. User Login
**`POST /api/auth/login`**
```bash
curl -X POST "http://localhost:5000/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "jane@example.com",
    "password": "Password123!"
  }'
```
**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresAt": "2026-09-11T06:30:00Z",
  "user": {
    "id": 1,
    "name": "Jane Developer",
    "email": "jane@example.com",
    "createdAt": "2026-09-11T05:30:00Z"
  }
}
```

---

### 4. Create Task (Protected)
**`POST /api/tasks`**
```bash
curl -X POST "http://localhost:5000/api/tasks" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN_HERE" \
  -d '{
    "title": "Design REST API Architecture",
    "description": "Create clean layered architecture with repository and service pattern.",
    "status": "InProgress",
    "priority": "High"
  }'
```
**Response (201 Created):**
```json
{
  "id": 1,
  "title": "Design REST API Architecture",
  "description": "Create clean layered architecture with repository and service pattern.",
  "status": "InProgress",
  "priority": "High",
  "createdAt": "2026-09-11T05:35:00Z",
  "updatedAt": null,
  "userId": 1
}
```

---

### 5. Get All Tasks for Authenticated User (Protected)
**`GET /api/tasks`**
```bash
curl -X GET "http://localhost:5000/api/tasks" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN_HERE"
```
**Response (200 OK):**
```json
[
  {
    "id": 1,
    "title": "Design REST API Architecture",
    "description": "Create clean layered architecture with repository and service pattern.",
    "status": "InProgress",
    "priority": "High",
    "createdAt": "2026-09-11T05:35:00Z",
    "updatedAt": null,
    "userId": 1
  }
]
```

---

### 6. Get Task by ID (Protected)
**`GET /api/tasks/1`**
```bash
curl -X GET "http://localhost:5000/api/tasks/1" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN_HERE"
```
**Response (200 OK):**
```json
{
  "id": 1,
  "title": "Design REST API Architecture",
  "description": "Create clean layered architecture with repository and service pattern.",
  "status": "InProgress",
  "priority": "High",
  "createdAt": "2026-09-11T05:35:00Z",
  "updatedAt": null,
  "userId": 1
}
```

---

### 7. Update Task (Protected)
**`PUT /api/tasks/1`**
```bash
curl -X PUT "http://localhost:5000/api/tasks/1" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN_HERE" \
  -d '{
    "title": "Design REST API Architecture",
    "description": "Clean layered architecture completed successfully.",
    "status": "Completed",
    "priority": "High"
  }'
```
**Response (200 OK):**
```json
{
  "id": 1,
  "title": "Design REST API Architecture",
  "description": "Clean layered architecture completed successfully.",
  "status": "Completed",
  "priority": "High",
  "createdAt": "2026-09-11T05:35:00Z",
  "updatedAt": "2026-09-11T05:40:00Z",
  "userId": 1
}
```

---

### 8. Delete Task (Protected)
**`DELETE /api/tasks/1`**
```bash
curl -X DELETE "http://localhost:5000/api/tasks/1" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN_HERE"
```
**Response (204 No Content)**

---

## 🧪 Unit Tests

The test suite in `TaskManagementAPI.Tests` uses **xUnit** and **Moq** to test core business logic without touching an actual database:

- **`AuthServiceTests`**:
  - `RegisterAsync_ShouldCreateUserAndReturnToken_WhenValidDtoProvided`
  - `RegisterAsync_ShouldThrowInvalidOperationException_WhenEmailAlreadyExists`
  - `LoginAsync_ShouldReturnToken_WhenCredentialsAreValid`
  - `LoginAsync_ShouldThrowUnauthorizedAccessException_WhenPasswordIsIncorrect`

- **`TaskServiceTests`**:
  - `GetTasksForUserAsync_ShouldReturnOnlyUserTasks`
  - `GetTaskByIdForUserAsync_ShouldReturnTask_WhenTaskBelongsToUser`
  - `GetTaskByIdForUserAsync_ShouldReturnNull_WhenTaskBelongsToAnotherUser`
  - `CreateTaskForUserAsync_ShouldCreateAndReturnTask`
  - `UpdateTaskForUserAsync_ShouldUpdate_WhenTaskBelongsToUser`
  - `DeleteTaskForUserAsync_ShouldReturnTrue_WhenTaskDeleted`

Run all unit tests:
```bash
dotnet test
```
