# Workplace Tasks - Fullstack RBAC Task Management System

This project was developed as part of a technical challenge.

It consists of a fullstack application built with:

- Backend: ASP.NET Core (.NET 8) + Entity Framework Core + PostgreSQL
- Frontend: React + TypeScript + Tailwind CSS
- Authentication: JWT
- Authorization: Role-Based Access Control (RBAC)

---

## Overview

The system allows task management with different access levels:

- **Admin**
- **Manager**
- **Member**

Each role has different permissions and restrictions, implemented both at API level and in the frontend UI.

---

## Backend Setup

### Requirements

- .NET 8 SDK
- PostgreSQL

### Steps

1. Navigate to backend folder: cd backend/WorkplaceTasks.API

2. Configure `appsettings.json`:

Update your PostgreSQL connection string --> "DefaultConnection": "Host=localhost;Port=5432;Database=workplacetasks;Username=postgres;Password=YOUR_PASSWORD"


3. Apply migrations: run on the termninal 'dotnet ef database update'


4. Run the API: run on the terminal 'dotnet run'

The API will run on: https://localhost:7045 or http://localhost:5275


---

## Frontend Setup

### Requirements

- Node.js (v18+ recommended)

### Steps

1. Navigate to frontend folder: cd frontend;


2. Install dependencies: run on the terminal 'npm install';


3. Start development server: run on the terminal 'npm run dev'

Frontend runs on: http://localhost:5173


---

## Authentication & RBAC Testing

### Test Accounts

For the role Admin:  
Email: admin@example.com
Password: 123456


For the role Manager:
Email: manager@example.com
Password: 123456


For the role Member:
Email: member@example.com
Password: 123456


---

### Role Permissions

#### Admin
- Full access to all endpoints
- Create, update, delete any task
- Assign tasks
- Manage users (create, delete, change roles)

#### Manager
- Create tasks
- Assign tasks
- Update any task
- Delete only tasks they created
- View users (for assignment purposes)

#### Member
- Create tasks
- View tasks assigned to them or created by them
- Update tasks they created
- Update status of tasks assigned to them
- Delete only tasks they created

---

## Technical Decisions

- Clean separation between DTOs and Entities
- RBAC enforced both in API layer and UI
- JWT-based authentication
- Pagination implemented server-side
- Enum-based task status (Pending, InProgress, Done)
- Secure role validation in controllers
- Conditional UI rendering based on permissions

---

## Features Implemented

- JWT authentication
- Role-based authorization
- Task creation, editing, deletion
- Task assignment
- Server-side pagination
- Status filtering
- Responsive UI with Tailwind
- Empty state UX
- Protected routes

---

## Possible Improvements (If More Time Was Available)

- Add refresh token mechanism
- Add centralized error handling middleware
- Add logging layer (Serilog)
- Add unit and integration tests
- Add Docker setup
- Improve UI transitions and animations
- Add dashboard with statistics
- Add audit logging (who changed what)

---

## Architecture Notes

The project follows a layered architecture:

- Controllers → Application logic entry
- DTOs → Transport layer
- EF Core → Data persistence
- React SPA → Client-side UI
- Axios → API communication

RBAC is enforced both server-side and client-side to ensure security and proper UX.

---

## Final Notes

This implementation prioritizes:

- Clean structure
- Security
- Readability
- Maintainability
- Clear RBAC rules

Thank you for reviewing this project.



