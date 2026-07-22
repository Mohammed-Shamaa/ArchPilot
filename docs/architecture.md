# ArchPilot - Architecture Plan

## System Overview

ArchPilot is an AI-powered Software Engineering Lifecycle Platform that transforms raw software ideas into complete professional blueprints.

## Technology Stack

### Backend
- **Framework**: ASP.NET Core 10 Web API
- **Architecture**: Clean Architecture (Domain → Application → Infrastructure → API)
- **ORM**: Entity Framework Core
- **Database**: PostgreSQL
- **Auth**: JWT + Refresh Tokens
- **AI Integration**: Grok API (xAI)
- **PDF Generation**: QuestPDF
- **DOCX Generation**: OpenXML SDK
- **Logging**: Serilog
- **Validation**: FluentValidation + MediatR

### Frontend
- **Framework**: Next.js 15 (App Router)
- **Language**: TypeScript
- **State Management**: Zustand
- **Styling**: Tailwind CSS + shadcn/ui
- **i18n**: next-intl (English/Arabic, RTL support)
- **HTTP Client**: Axios
- **Diagrams**: Mermaid.js

### Deployment
- **Frontend**: Vercel
- **Backend**: Render
- **Database**: Render PostgreSQL / Supabase
- **Storage**: AWS S3 / Azure Blob

## Solution Structure

```
ArchPilot/
├── backend/
│   └── src/
│       ├── ArchPilot.API/              # Presentation Layer
│       │   ├── Controllers/
│       │   ├── Middleware/
│       │   ├── Filters/
│       │   └── Program.cs
│       ├── ArchPilot.Application/       # Business Logic Layer
│       │   ├── Common/
│       │   │   ├── Behaviors/
│       │   │   ├── Interfaces/
│       │   │   ├── Models/
│       │   │   └── Mappings/
│       │   ├── Features/
│       │   │   ├── Auth/
│       │   │   ├── Projects/
│       │   │   ├── AI/
│       │   │   ├── Documents/
│       │   │   └── Teams/
│       │   └── Services/
│       ├── ArchPilot.Domain/            # Core Domain Layer
│       │   ├── Entities/
│       │   ├── ValueObjects/
│       │   ├── Enums/
│       │   └── Exceptions/
│       ├── ArchPilot.Infrastructure/    # External Integrations
│       │   ├── AI/
│       │   ├── Email/
│       │   ├── Storage/
│       │   └── Services/
│       └── ArchPilot.Persistence/       # Data Access Layer
│           ├── Context/
│           ├── Configurations/
│           ├── Migrations/
│           └── Repositories/
├── frontend/
│   └── src/
│       ├── app/                         # Next.js App Router
│       │   ├── (auth)/
│       │   ├── (dashboard)/
│       │   ├── (landing)/
│       │   └── api/
│       ├── components/
│       │   ├── ui/                      # shadcn/ui
│       │   ├── layout/
│       │   └── features/
│       ├── features/
│       │   ├── auth/
│       │   ├── projects/
│       │   ├── ai/
│       │   └── documents/
│       ├── hooks/
│       ├── lib/
│       ├── services/
│       ├── store/
│       ├── types/
│       └── i18n/
└── docs/
```

## API Design

### Authentication
- `POST /api/auth/register` - Create account
- `POST /api/auth/login` - Login
- `POST /api/auth/refresh` - Refresh token
- `POST /api/auth/logout` - Invalidate token

### Projects
- `POST /api/projects` - Create project
- `GET /api/projects` - List projects
- `GET /api/projects/{id}` - Get project
- `PUT /api/projects/{id}` - Update project
- `DELETE /api/projects/{id}` - Delete project

### AI Chat
- `POST /api/ai/chat` - Send message to AI
- `GET /api/ai/projects/{id}/conversations` - List conversations
- `GET /api/ai/conversations/{id}/messages` - Get messages

### Documents
- `GET /api/projects/{id}/documents` - List project documents
- `GET /api/documents/{id}` - Get document
- `GET /api/documents/{id}/export?format=pdf|docx` - Export document

## Database Schema (Core Entities)

See database-design.md for full schema.

## Security
- JWT Bearer Authentication
- Refresh Token Rotation
- BCrypt Password Hashing
- Rate Limiting
- CORS Configuration
- Input Validation
- SQL Injection Protection (EF Core parameterized queries)

## AI Multi-Agent System

| Agent | Responsibility |
|-------|---------------|
| Product Manager | Idea analysis, business goals, scope |
| Requirements Engineer | SRS, functional requirements |
| Software Architect | Architecture, technology decisions |
| Database Engineer | ERD, schema, relationships |
| UX Designer | Pages, components, user flows |
| QA Engineer | Testing strategy, test cases |
| Project Manager | Timeline, sprints, tasks |

## AI Memory System

Each project maintains context:
- Project information
- Conversation history
- Generated documents
- Technical decisions
- User preferences

## Document Version Control

Every document supports:
- Version numbering (v1.0, v1.1, v2.0)
- Change summaries
- Previous version access
- Restore capability
