# Todo List Backend

Welcome to the Todo List Backend! This ASP.NET Core Web API serves as the backend for the Todo List Website, providing authentication and task management functionality.

## Features

- **Authentication**: Secure user authentication using JSON Web Tokens (JWT).
  
- **Task Actions**: CRUD operations for managing tasks, including creating, retrieving, updating, and deleting tasks.

## Getting Started

To run this project locally, follow these steps:

1. Clone the repository: `git clone https://github.com/Bacter16/WEB-API-To-Do-Website.git`
2. Navigate to the project directory: `cd WEB-API-To-Do-Website`
3. Install dependencies: `dotnet restore`
4. Configure the database connection string in `appsettings.json`.
5. Apply database migrations: `dotnet ef database update`
6. Start the server: `dotnet run`

## Authentication

This backend API uses JWT for authentication. When a user successfully logs in, they receive a JWT token, which they include in subsequent requests as a bearer token in the Authorization header.

## Endpoints

### Authentication

- `POST /api/auth/register`: Register a new user.
- `POST /api/auth/login`: Authenticate and log in a user.

### Task Actions

- `GET /api/tasks`: Retrieve all tasks.
- `GET /api/tasks/{id}`: Retrieve a specific task by ID.
- `POST /api/tasks`: Create a new task.
- `PUT /api/tasks/{id}`: Update an existing task.
- `DELETE /api/tasks/{id}`: Delete a task by ID.
---

This project was created by Bocti. Feel free to [reach out](bacter.cris1@gmail.com) with any questions or feedback.
