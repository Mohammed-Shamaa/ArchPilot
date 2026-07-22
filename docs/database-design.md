# ArchPilot - Database Design

## Database: PostgreSQL

## Entity Relationship Diagram

```mermaid
erDiagram
    USERS {
        uuid id PK
        varchar username
        varchar email UK
        varchar password_hash
        varchar profile_image
        varchar role
        varchar language_preference
        varchar theme_preference
        timestamp created_at
        timestamp updated_at
        boolean is_active
    }

    REFRESH_TOKENS {
        uuid id PK
        uuid user_id FK
        varchar token_hash
        timestamp expires_at
        timestamp created_at
        boolean is_revoked
    }

    PROJECTS {
        uuid id PK
        uuid user_id FK
        varchar project_name
        text description
        varchar status
        timestamp created_at
        timestamp updated_at
        timestamp last_accessed_at
    }

    CONVERSATIONS {
        uuid id PK
        uuid project_id FK
        varchar title
        timestamp created_at
        timestamp updated_at
    }

    MESSAGES {
        uuid id PK
        uuid conversation_id FK
        varchar sender_type
        text content
        integer token_usage
        timestamp created_at
    }

    AI_CONTEXTS {
        uuid id PK
        uuid project_id FK
        varchar context_type
        text content
        timestamp created_at
        timestamp updated_at
    }

    GENERATED_DOCUMENTS {
        uuid id PK
        uuid project_id FK
        uuid conversation_id FK
        varchar document_type
        varchar title
        text content
        varchar format
        integer version
        timestamp created_at
        timestamp updated_at
    }

    DOCUMENT_VERSIONS {
        uuid id PK
        uuid document_id FK
        integer version_number
        text content
        varchar change_summary
        timestamp created_at
    }

    PROMPTS {
        uuid id PK
        varchar agent_type
        text prompt_content
        integer version
        boolean is_active
        timestamp created_at
        timestamp updated_at
    }

    AI_USAGE {
        uuid id PK
        uuid user_id FK
        uuid project_id FK
        varchar model_name
        integer tokens_used
        decimal request_cost
        timestamp created_at
    }

    SUBSCRIPTIONS {
        uuid id PK
        uuid user_id FK
        varchar plan_name
        timestamp start_date
        timestamp end_date
        varchar status
        varchar payment_status
        timestamp created_at
    }

    TEAMS {
        uuid id PK
        varchar name
        uuid owner_id FK
        timestamp created_at
    }

    TEAM_MEMBERS {
        uuid id PK
        uuid team_id FK
        uuid user_id FK
        varchar role
        timestamp joined_at
    }

    INVITATIONS {
        uuid id PK
        uuid team_id FK
        varchar email
        varchar status
        timestamp created_at
    }

    USERS ||--o{ PROJECTS : creates
    USERS ||--o{ REFRESH_TOKENS : has
    USERS ||--o{ AI_USAGE : generates
    USERS ||--o{ SUBSCRIPTIONS : has
    USERS ||--o{ TEAMS : owns
    USERS ||--o{ TEAM_MEMBERS : belongs_to
    PROJECTS ||--o{ CONVERSATIONS : has
    PROJECTS ||--o{ AI_CONTEXTS : stores
    PROJECTS ||--o{ GENERATED_DOCUMENTS : contains
    CONVERSATIONS ||--o{ MESSAGES : contains
    GENERATED_DOCUMENTS ||--o{ DOCUMENT_VERSIONS : versioned
    TEAMS ||--o{ TEAM_MEMBERS : includes
    TEAMS ||--o{ INVITATIONS : sends
```

## Table Definitions

### Users
- PK: `id` (UUID)
- Unique: `email`
- Index: `email` (for login lookup)
- Index: `username`

### RefreshTokens
- PK: `id` (UUID)
- FK: `user_id` → Users
- Index: `token_hash` (for token lookup)
- Index: `user_id` + `is_revoked` (for active token check)

### Projects
- PK: `id` (UUID)
- FK: `user_id` → Users
- Index: `user_id` (for user's projects)
- Index: `last_accessed_at` (for recent projects)

### Conversations
- PK: `id` (UUID)
- FK: `project_id` → Projects
- Index: `project_id`

### Messages
- PK: `id` (UUID)
- FK: `conversation_id` → Conversations
- Index: `conversation_id` + `created_at`

### AIContexts
- PK: `id` (UUID)
- FK: `project_id` → Projects
- Index: `project_id` + `context_type`

### GeneratedDocuments
- PK: `id` (UUID)
- FK: `project_id` → Projects
- FK: `conversation_id` → Conversations (nullable)
- Index: `project_id` + `document_type`

### DocumentVersions
- PK: `id` (UUID)
- FK: `document_id` → GeneratedDocuments
- Index: `document_id` + `version_number`

### Prompts
- PK: `id` (UUID)
- Unique: `agent_type` (only one active prompt per agent)
- Index: `agent_type` + `is_active`

### AIUsage
- PK: `id` (UUID)
- FK: `user_id` → Users
- FK: `project_id` → Projects
- Index: `user_id` + `created_at` (for billing queries)

### Subscriptions
- PK: `id` (UUID)
- FK: `user_id` → Users
- Index: `user_id` + `status`

### Teams / TeamMembers / Invitations
- For future team collaboration features
