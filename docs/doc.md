# 🚀 AAU Connect

> **A modern, modular social platform for the AAU community.**

Welcome to the backend repository for **AAU Connect**! This isn't just another API; it's a robust, scalable system built to connect students and instructors through posts, groups, and real-time messaging.

We've built this using a **Modular Monolith** architecture, ensuring that our code remains clean, maintainable, and ready for future growth (even into microservices!).

---

## ✨ Key Features

-   **🔐 Secure Authentication**: Integrated with **Keycloak** for industrial-strength Identity Management (OpenID Connect).
-   **📰 Interactive Timeline**: Create posts, share media, like, and comment in a rich social feed.
-   **👥 Community Groups**: Join Interest Groups, Study Clubs, or Classes to connect with like-minded peers.
-   **💬 Real-Time Messaging**: Private conversations powered by event-driven communication.
-   **🛡️ Robust Architecture**: Built with **Clean Architecture** and **Domain-Driven Design (DDD)** principles.

---

## 🛠️ The Tech Stack

We use cutting-edge tools to deliver performance and reliability:

-   **Framework**: ASP.NET Core 8 Web API
-   **Database**: PostgreSQL (with EF Core)
-   **Messaging**: RabbitMQ (via MassTransit)
-   **Background Jobs**: Quartz.NET
-   **Identity**: Keycloak
-   **Containerization**: Docker & Docker Compose

---

## 🧩 Architecture Highlights

Our system is designed to be resilient and decoupled.

### 1. Modular Monolith
The application is split into four distinct modules:
-   `Auth`: User identity and profile management.
-   `Timeline`: Posts, comments, and interactions.
-   `Groups`: Community management and membership.
-   `Messaging`: Direct user-to-user communication.

### 2. Event-Driven & Reliable
We use the **Transactional Outbox Pattern** to ensure we never lose a message.
1.  When you do something (e.g., register), we save the data *and* an event to the DB in one transaction.
2.  A background job picks up the event and sends it to **RabbitMQ**.
3.  Other modules listen to RabbitMQ and react instantly!

> 📚 **Want the deep dive?** Check out our [Architecture Documentation](docs/Architecture.md).

---

## 🚀 Getting Started

Ready to run the code? Follow these simple steps.

### 1. Start Infrastructure
Spin up the database, message broker, and authentication server with one command:
```bash
docker-compose up -d
```
*   **RabbitMQ Dashboard**: [http://localhost:15672](http://localhost:15672) (guest/guest)
*   **Keycloak Admin**: [http://localhost:8080](http://localhost:8080) (admin/admin)

### 2. Run the API
```bash
dotnet run --project src/API/AAU.Connect.API
```
The API will start at `https://localhost:5001`.

### 3. Verification & Testing
We have a comprehensive guide to help you test every endpoint using Postman.
> 🧪 **Read the [Postman Testing Guide](docs/TestingGuide.md)**

---

## 📂 Documentation

We believe in documenting *why*, not just *how*.
-   [**Architecture Overview**](docs/Architecture.md): Diagrams and detailed explanations of our patterns.
-   [**Class Diagrams**](docs/ClassDiagrams.md): UML diagrams for our domain models.
-   [**Testing Guide**](docs/TestingGuide.md): Step-by-step verification instructions.

---
