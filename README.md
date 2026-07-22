# ArchPilot

AI-powered Software Engineering Lifecycle Platform that transforms raw software ideas into complete professional blueprints.

## Architecture

- **Backend**: ASP.NET Core 10 Web API (Clean Architecture)
- **Frontend**: Next.js 15 + TypeScript
- **Database**: PostgreSQL + Entity Framework Core
- **AI Integration**: Grok API (xAI)
- **Auth**: JWT + Refresh Tokens

## Backend Structure

```
backend/src/
├── ArchPilot.Domain/          # Entities, Enums, Exceptions
├── ArchPilot.Application/     # CQRS Commands/Queries, Validators, Interfaces
├── ArchPilot.Infrastructure/  # AI, Auth, Export services
├── ArchPilot.Persistence/     # EF Core, Configurations, Repositories
└── ArchPilot.API/             # Controllers, Middleware, Configuration
```

## Setup

### Backend
```bash
cd backend
dotnet restore
dotnet build
dotnet run --project src/ArchPilot.API
```

### Environment Variables
```env
DATABASE_CONNECTION_STRING=Host=localhost;Port=5432;Database=archpilot;Username=postgres;Password=postgres
JWT_KEY=YourSecretKeyHere
GROK_API_KEY=your-grok-api-key
```

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/auth/register` | Register new user |
| POST | `/api/auth/login` | Login |
| POST | `/api/projects` | Create project |
| GET | `/api/projects` | List projects |
| GET | `/api/projects/{id}` | Get project |
| POST | `/api/ai/chat` | Chat with AI |
| GET | `/api/documents/project/{id}` | Get project documents |
| GET | `/api/documents/{id}/export` | Export document |
