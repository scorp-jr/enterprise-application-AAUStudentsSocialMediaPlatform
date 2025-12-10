# Project Proposal: AAU Students Social Media Platform

## Group Members
| No. | Members | ID |
|-----|---------|------|
| 1 | Biruk Dereje | UGR/6190/15 |
| 2 | Bisrat Dereje | UGR/3229/15 |
| 3 | Hailemichael Molla | UGR/3629/15 |
| 4 | Kena Ararso | UGR/9085/15 |


## 1. Introduction and Business Problem
University students often rely on scattered communication channels—Telegram groups, WhatsApp chats, random social media pages, and unofficial websites—to share academic announcements, class notes, events, and campus updates. This fragmentation leads to:

- Missed announcements and lack of centralized communication  
- Fake information circulating across multiple platforms  
- Difficulty finding academic resources or connecting with classmates  
- No dedicated space for student communities and clubs  
- Limited ways for new students to integrate socially  

AAU lacks a unified digital platform built specifically for students, where they can interact socially, access academic content, receive verified updates, and communicate in a structured way.

This project proposes the **AAU Students Social Media Platform**, to model the social interactions, messaging flows, academic communities, and verification mechanisms required on a campus network.

---

## 2. Domain and Subdomain Analysis (Core, Supporting, Generic)

### Core Domains
These directly deliver the platform’s unique value.

- **Student Identity** — ensures only verified students can join, making exclusivity real. It owns registration and verification flows.
- **Campus Social Graph** — models how students connect based on classes, departments, and campus life.
- **Content** — posts and interactions shaped around the student community, such as course groups or campus events.
- **Messaging** — private and group chats designed for academic and social collaboration among verified students.

### Supporting Subdomains
- **Moderation** — keeps the student space respectful and safe.  
- **Academic Integration** — ties discussion spaces to courses, schedules, or faculty data.  
- **Notifications** — keeps students updated on posts, class groups, or messages.

### Generic Subdomains
- **Access Management** — authentication and session control.  
- **Analytics** — engagement metrics and usage patterns.  
- **File Storage** — generic media upload and retrieval.  

---

## 3. Core Domain Bounded Contexts

### 3.1 Post Interaction Context
**Responsibilities:**
- Create, edit, and delete posts  
- Manage likes, dislikes, and comments  
- Handle media uploads (images, files, videos)  
- Publish events like **PostCreated**, **CommentAdded**, **PostLiked**  

This context drives engagement across the platform.

### 3.2 Student Verification Context
**Responsibilities:**
- Handles eligibility rules  
- Validate student identity using AAU email or university ID  

Ensures trust, authenticity, and a safe environment.

### 3.3 Messaging Context
**Responsibilities:**
- Manage one-to-one and group conversations  
- Deliver messages in real-time  
- Support media and file sharing in chats  
- Publish and subscribe to events such as **MessageSent** and **MessageDelivered**  

Keeps communication consistent across devices.

---

## 4. User Stories with Domain Events & Eventual Consistency

### 4.1 Posting Announcements in a Course Group
**As a:** Course Representative  
**I want:** to post announcements in our course community  
**So that:** classmates can receive updates instantly  

**Contexts Involved**
- Academic Community  
- Post Interaction  
- Notification  

**Event Flow**
1. CR posts update → **CourseAnnouncementCreated**  
2. Post Interaction formats and saves post → **PostPublished**  
3. Notification sends events → **NotificationPushed**  

**Eventual Consistency**
- Notifications may appear a few seconds later  
- The platform remains consistent after asynchronous events settle  

---

### 4.2 Private Messaging Between Students
**As a:** Student  
**I want:** to send a message to another student  
**So that:** we can communicate privately  

**Contexts Involved**
- Messaging  
- Notification  

**Event Flow**
1. User sends message → **MessageSent**  
2. Receiver gets real-time update → **MessageDelivered**  
3. Notification triggers → **MessageNotificationCreated**  

**Eventual Consistency**
- Delivery and read receipts update asynchronously  

---

### 4.3 Identity Verification on First Sign-Up
**As a:** New AAU Student  
**I want:** to verify my identity using my AAU email  
**So that:** I can access student communities safely  

**Contexts Involved**
- Student Verification  
- User Management  

**Event Flow**
1. Student submits AAU email → **VerificationInitiated**  
2. Verification Context confirms → **StudentVerified**  
3. User is granted access → **AccessGranted**  

**Eventual Consistency**
- Verification emails and status updates may require slight asynchronous delays  

---

## 5. AI-Driven Feature
**Smart Content Moderation & Community Assistance Engine**
- Detects harmful or inappropriate content  
- Auto-suggests tags and trending hashtags for posts  
- Suggests replies in chat (AI-assisted messaging)  

---

