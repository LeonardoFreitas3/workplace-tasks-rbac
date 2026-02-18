# Workplace Tasks - Fullstack RBAC Task Management System

This project was developed as part of a technical challenge.

It consists of a fullstack application built with:

-   Backend: ASP.NET Core (.NET 8) + Entity Framework Core + PostgreSQL
-   Frontend: React + TypeScript + Tailwind CSS (Vite)
-   Authentication: JWT
-   Authorization: Role-Based Access Control (RBAC)

------------------------------------------------------------------------

## Backend Setup

### Requirements

-   .NET 10 SDK
-   PostgreSQL

### Steps

1.  Navigate to backend folder: cd backend/WorkplaceTasks.API

2.  Configure `appsettings.json` with your PostgreSQL connection string.

3.  Apply migrations: dotnet ef database update

4.  Run the API: dotnet run

API runs on: http://localhost:5275

------------------------------------------------------------------------

## Frontend Setup

### Requirements

-   Node.js (v18+ recommended)

### Steps

1.  Navigate to frontend folder: cd frontend

2.  Install dependencies: npm install

3.  Start development server: npm run dev

Frontend runs on: http://localhost:5173

------------------------------------------------------------------------

## Instructions to Test RBAC

### Test Accounts

Admin: admin@example.com\
Password: 123456

Manager: manager@example.com\
Password: 123456

Member: member@example.com\
Password: 123456

### How to Generate JWT

Use:

POST /api/Auth/login

Body: { "email": "admin@example.com", "password": "123456" }

Copy the returned token and use it in Authorization header:

Authorization: Bearer YOUR_TOKEN

------------------------------------------------------------------------

## API Endpoints

### Authentication

POST /api/Auth/login\
Authenticates user and returns JWT token.

------------------------------------------------------------------------

### Tasks

GET /api/Tasks\
Returns paginated tasks.\
Optional query params: status, page, pageSize.

POST /api/Tasks\
Creates a task.\
Admin and Manager can assign tasks.

PUT /api/Tasks/{id}\
Updates a task.\
Members can only update their own tasks or status if assigned.

DELETE /api/Tasks/{id}\
Deletes a task.\
Only Admin or task creator allowed.

------------------------------------------------------------------------

### Users

GET /api/Users\
Admin and Manager can list users.

POST /api/Users\
Admin only -- create new user.

PUT /api/Users/{id}/role\
Admin only -- change user role.

DELETE /api/Users/{id}\
Admin only -- delete user.\
Cannot delete self or last Admin.

------------------------------------------------------------------------

## Technical Decisions

-   Clean separation between Controllers, DTOs and Entities
-   RBAC enforced server-side and client-side
-   JWT authentication with role claims
-   Server-side pagination
-   Enum-based status and roles
-   Axios interceptor for automatic token attachment
-   Reusable pagination component

------------------------------------------------------------------------

## Possible Improvements

-   Refresh token mechanism
-   Global exception middleware
-   Unit and integration tests
-   Docker support
-   CI/CD pipeline
