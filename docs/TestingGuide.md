# Postman Testing Guide - AAU Connect API

## Prerequisites

### 1. Start the Application
```bash
cd /home/bisrat/Projects/Backend
dotnet run --project src/API/AAU.Connect.API
```

**Base URL**: `http://localhost:5000` (or check console output)

### 2. Start Keycloak (for Authentication)
```bash
docker-compose up -d keycloak
```

**Keycloak Admin Console**: `http://localhost:8080`
- Username: `admin`
- Password: `admin`

---

## Part 1: Keycloak Setup

### Step 1: Create Realm
1. Open Keycloak Admin Console: `http://localhost:8080`
2. Login with `admin` / `admin`
3. Click dropdown next to "master" → **Create Realm**
4. Name: `aau-connect`
5. Click **Create**

### Step 2: Create Client
1. In `aau-connect` realm, go to **Clients** → **Create client**
2. Client ID: `account`
3. Client authentication: **OFF** (public client)
4. Valid redirect URIs: `*`
5. Web origins: `*`
6. Click **Save**

### Step 3: Create Test User
1. Go to **Users** → **Add user**
2. Username: `testuser`
3. Email: `test@aau.edu.et`
4. First name: `Test`
5. Last name: `User`
6. Click **Create**
7. Go to **Credentials** tab
8. Set password: `password123`
9. Temporary: **OFF**
10. Click **Set password**

### Step 4: Get Access Token (Postman)

**Request**:
```http
POST http://localhost:8080/realms/aau-connect/protocol/openid-connect/token
Content-Type: application/x-www-form-urlencoded

grant_type=password
&client_id=account
&username=testuser
&password=password123
```

**Response**:
```json
{
  "access_token": "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expires_in": 300,
  "refresh_token": "...",
  "token_type": "Bearer"
}
```

**Copy the `access_token`** - you'll use it for authenticated requests.

---

## Part 2: API Testing

### Environment Variables (Postman)
Create these variables in Postman:
- `baseUrl`: `http://localhost:5000`
- `token`: `<paste access_token here>`

---

## Auth Module

### 1. Register User

**Request**:
```http
POST {{baseUrl}}/auth/register
Content-Type: application/json

{
  "id": "{{$guid}}",
  "email": "john.doe@aau.edu.et",
  "firstName": "John",
  "lastName": "Doe",
  "role": "Student"
}
```

**Response**:
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "email": "john.doe@aau.edu.et",
  "firstName": "John",
  "lastName": "Doe",
  "role": "Student"
}
```

**Save the `id`** as `userId` variable.

**Verify Outbox**:
```sql
SELECT * FROM auth."OutboxMessages" 
WHERE "Type" = 'UserRegisteredDomainEvent' 
ORDER BY "OccurredOnUtc" DESC LIMIT 1;
```

---

### 2. Get Current User (Authenticated)

**Request**:
```http
GET {{baseUrl}}/auth/me
Authorization: Bearer {{token}}
```

**Response**:
```json
{
  "isAuthenticated": true,
  "name": "testuser",
  "claims": [
    { "type": "sub", "value": "..." },
    { "type": "email", "value": "test@aau.edu.et" }
  ]
}
```

---

## Timeline Module

### 3. Create Post

**Request**:
```http
POST {{baseUrl}}/posts
Content-Type: application/json

{
  "userId": "{{userId}}",
  "caption": "My first post! #AAU #Connect",
  "mediaUrl": "https://example.com/image.jpg",
  "filters": "Vintage",
  "location": "Addis Ababa University",
  "tags": ["AAU", "Connect", "FirstPost"]
}
```

**Response**:
```json
{
  "id": "7c9e6679-7425-40de-944b-e07fc1f90ae7",
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "caption": "My first post! #AAU #Connect",
  "mediaUrl": "https://example.com/image.jpg",
  "filters": "Vintage",
  "location": "Addis Ababa University",
  "tags": ["AAU", "Connect", "FirstPost"],
  "createdAt": "2026-01-09T08:30:00Z"
}
```

**Save the `id`** as `postId` variable.

**Verify Outbox**:
```sql
SELECT * FROM timeline."OutboxMessages" 
WHERE "Type" = 'PostCreatedDomainEvent' 
ORDER BY "OccurredOnUtc" DESC LIMIT 1;
```

---

### 4. Get Feed (All Posts)

**Request**:
```http
GET {{baseUrl}}/posts
```

**Response**:
```json
[
  {
    "id": "7c9e6679-7425-40de-944b-e07fc1f90ae7",
    "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "caption": "My first post! #AAU #Connect",
    "mediaUrl": "https://example.com/image.jpg",
    ...
  }
]
```

---

### 5. Add Comment to Post

**Request**:
```http
POST {{baseUrl}}/posts/{{postId}}/comments
Content-Type: application/json

{
  "userId": "{{userId}}",
  "content": "Great post! 👍"
}
```

**Response**:
```json
{
  "id": "a1b2c3d4-...",
  "postId": "7c9e6679-...",
  "userId": "3fa85f64-...",
  "content": "Great post! 👍",
  "createdAt": "2026-01-09T08:35:00Z"
}
```

---

### 6. Get Comments for Post

**Request**:
```http
GET {{baseUrl}}/posts/{{postId}}/comments
```

**Response**:
```json
[
  {
    "id": "a1b2c3d4-...",
    "userId": "3fa85f64-...",
    "content": "Great post! 👍",
    "createdAt": "2026-01-09T08:35:00Z"
  }
]
```

---

### 7. Toggle Like on Post

**Request**:
```http
POST {{baseUrl}}/posts/{{postId}}/like
Content-Type: application/json

{
  "userId": "{{userId}}"
}
```

**Response**:
```json
{
  "isLiked": true
}
```

**Call again to unlike**:
```json
{
  "isLiked": false
}
```

---

## Groups Module

### 8. Create Group

**Request**:
```http
POST {{baseUrl}}/groups
Content-Type: application/json

{
  "name": "Software Engineering Students",
  "description": "Group for SE students to collaborate",
  "type": "Course",
  "creatorId": "{{userId}}",
  "bannerUrl": "https://example.com/banner.jpg"
}
```

**Response**:
```json
{
  "id": "9f8e7d6c-...",
  "name": "Software Engineering Students",
  "description": "Group for SE students to collaborate",
  "type": "Course",
  "creatorId": "3fa85f64-...",
  "bannerUrl": "https://example.com/banner.jpg",
  "memberIds": ["3fa85f64-..."],
  "createdAt": "2026-01-09T08:40:00Z"
}
```

**Save the `id`** as `groupId` variable.

**Verify Outbox**:
```sql
SELECT * FROM groups."OutboxMessages" 
WHERE "Type" = 'GroupCreatedDomainEvent' 
ORDER BY "OccurredOnUtc" DESC LIMIT 1;
```

---

### 9. Get All Groups

**Request**:
```http
GET {{baseUrl}}/groups
```

**Optional Filter by Type**:
```http
GET {{baseUrl}}/groups?type=Course
```

**Response**:
```json
[
  {
    "id": "9f8e7d6c-...",
    "name": "Software Engineering Students",
    "type": "Course",
    ...
  }
]
```

---

### 10. Get Group by ID

**Request**:
```http
GET {{baseUrl}}/groups/{{groupId}}
```

**Response**:
```json
{
  "id": "9f8e7d6c-...",
  "name": "Software Engineering Students",
  "description": "Group for SE students to collaborate",
  "type": "Course",
  "creatorId": "3fa85f64-...",
  "memberIds": ["3fa85f64-..."],
  "assignments": [],
  "resources": []
}
```

---

## Messaging Module

### 11. Start Conversation

**Request**:
```http
POST {{baseUrl}}/messaging/conversations
Content-Type: application/json

{
  "participantIds": [
    "{{userId}}",
    "00000000-0000-0000-0000-000000000001"
  ]
}
```

**Response**:
```json
{
  "id": "c1d2e3f4-...",
  "participantIds": [
    "3fa85f64-...",
    "00000000-0000-0000-0000-000000000001"
  ],
  "createdAt": "2026-01-09T08:45:00Z"
}
```

**Save the `id`** as `conversationId` variable.

---

### 12. Send Message

**Request**:
```http
POST {{baseUrl}}/messaging/conversations/{{conversationId}}/messages
Content-Type: application/json

{
  "senderId": "{{userId}}",
  "content": "Hello! How are you?"
}
```

**Response**:
```json
{
  "id": "d4e5f6a7-...",
  "conversationId": "c1d2e3f4-...",
  "senderId": "3fa85f64-...",
  "content": "Hello! How are you?",
  "createdAt": "2026-01-09T08:46:00Z"
}
```

**Verify Outbox**:
```sql
SELECT * FROM messaging."OutboxMessages" 
WHERE "Type" = 'MessageSentDomainEvent' 
ORDER BY "OccurredOnUtc" DESC LIMIT 1;
```

---

### 13. Get User's Conversations

**Request**:
```http
GET {{baseUrl}}/messaging/conversations?userId={{userId}}
```

**Response**:
```json
[
  {
    "id": "c1d2e3f4-...",
    "participantIds": ["3fa85f64-...", "00000000-..."],
    "lastMessage": "Hello! How are you?",
    "lastMessageAt": "2026-01-09T08:46:00Z"
  }
]
```

---

### 14. Get Messages in Conversation

**Request**:
```http
GET {{baseUrl}}/messaging/conversations/{{conversationId}}/messages
```

**Response**:
```json
[
  {
    "id": "d4e5f6a7-...",
    "senderId": "3fa85f64-...",
    "content": "Hello! How are you?",
    "createdAt": "2026-01-09T08:46:00Z"
  }
]
```

---

## Part 3: Verify Transactional Outbox

### Check Outbox Messages (SQL)

Connect to your PostgreSQL database and run:

```sql
-- View all unprocessed messages
SELECT * FROM auth."OutboxMessages" WHERE "ProcessedOnUtc" IS NULL;
SELECT * FROM timeline."OutboxMessages" WHERE "ProcessedOnUtc" IS NULL;
SELECT * FROM groups."OutboxMessages" WHERE "ProcessedOnUtc" IS NULL;
SELECT * FROM messaging."OutboxMessages" WHERE "ProcessedOnUtc" IS NULL;

-- View all processed messages
SELECT * FROM auth."OutboxMessages" WHERE "ProcessedOnUtc" IS NOT NULL;

-- View message details
SELECT 
    "Id",
    "Type",
    "OccurredOnUtc",
    "ProcessedOnUtc",
    "Error",
    LEFT("Content", 100) as "ContentPreview"
FROM timeline."OutboxMessages"
ORDER BY "OccurredOnUtc" DESC
LIMIT 10;
```

### Expected Behavior

1. **Immediately after API call**: Message appears in outbox with `ProcessedOnUtc = NULL`
2. **Within 10 seconds**: Quartz job processes message, sets `ProcessedOnUtc` to current time
3. **If error occurs**: `Error` column contains exception message

---

## Part 4: Testing Checklist

### Basic Functionality
- [ ] User registration creates user and outbox message
- [ ] Post creation creates post and outbox message
- [ ] Group creation creates group and outbox message
- [ ] Message sending creates message and outbox message

### Outbox Processing
- [ ] Messages appear in outbox immediately after transaction
- [ ] Messages are processed within 10 seconds
- [ ] `ProcessedOnUtc` is populated after processing
- [ ] No errors in `Error` column

### Authentication
- [ ] Can get access token from Keycloak
- [ ] `/auth/me` requires Bearer token
- [ ] Invalid token returns 401 Unauthorized

### Data Integrity
- [ ] All foreign keys are valid GUIDs
- [ ] Timestamps are in UTC
- [ ] Required fields are not null

---
