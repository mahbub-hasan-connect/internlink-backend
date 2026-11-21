# InternLink — Backend API

InternLink is a full-stack internship-matching platform that connects students with recruiters through automated resume/profile data, a real-time application tracker, and an AI-driven skill-match score. This repository contains the backend REST API.

**Frontend repo:** [internlink-frontend](https://github.com/mahbub-hasan-connect/internlink-frontend)

## Features

- JWT-based authentication with role-based authorization (Student / Recruiter / Admin)
- Student profile management (skills, resume data, tech stack)
- Job posting creation and management for recruiters, with ownership checks
- Application workflow with status tracking (Applied → Shortlisted → Interview → Offered / Hired / Rejected)
- AI-based skill-match scoring between a student's profile and a job posting's required skills
- Real-time in-app notifications on application status changes and recruiter messages
- Admin panel endpoints for platform moderation (verifying recruiters, managing users)
- CSV export of applicants for a job posting

## Tech Stack

- **Framework:** ASP.NET Core Web API (.NET)
- **ORM / Database:** Entity Framework Core (Code-First) with SQL Server
- **Auth:** JWT Bearer tokens, BCrypt password hashing
- **Architecture:** Layered — Controller → Service → Repository → DbContext

## Project Structure

```
Controllers/   → API endpoints
Services/      → business logic (interfaces + implementations)
Repositories/  → data access layer (interfaces + implementations)
Models/        → EF Core entities
DTOs/          → request/response data transfer objects
Data/          → AppDbContext
Migrations/    → EF Core migrations
```

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- SQL Server (LocalDB works fine for development)

### Setup

1. Clone the repo:
   ```
   git clone https://github.com/mahbub-hasan-connect/internlink-backend.git
   cd internlink-backend
   ```
2. Copy the example config and fill in your own values:
   ```
   copy appsettings.json.example appsettings.json
   ```
   Then open `appsettings.json` and set your own `Jwt:Key` (any long random string) and adjust the `DefaultConnection` string if needed.
3. Apply database migrations:
   ```
   dotnet ef database update
   ```
4. Run the API:
   ```
   dotnet run --launch-profile https
   ```
   The API will be available at `https://localhost:7182`.

## Authentication & Authorization

Every protected endpoint requires a `Bearer` JWT token (obtained from `/api/auth/login`). Role checks are enforced server-side via `[Authorize(Roles = "...")]`, so access control cannot be bypassed from the client.

## AI Skill-Match Scoring

Match scores are calculated by comparing a student's listed tech stack against a job posting's required skills (case-insensitive, overlap-based percentage). See `Services/SkillMatchServiceSMS1.cs`.

## Author

Built by **Mahbub Hasan** as part of a university Software Engineering project.
