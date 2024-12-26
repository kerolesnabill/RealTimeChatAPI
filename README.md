# Real-Time Chat API

This API enables real-time messaging using WebSockets, providing instant communication and updates. With seamless message delivery and real-time notifications, users can interact with the app without delays. Built with ASP.NET Core and SignalR, it also includes user authentication for secure communication, making it ideal for applications that need live, interactive messaging.

## Features

- **User Management**:

  - User registration and login.
  - Update user profile, including profile image and password.
  - Retrieve user information by ID or username.

- **Messaging System**:

  - Real-time messaging with delivery and read status updates.
  - Retrieve message history with specific users.

- **Chat Management**:

  - View active chat lists for the authenticated user.

- **SignalR Integration**:
  - Real-time communication via `ChatHub`, supporting:
    - Sending messages.
    - Message delivery and read status tracking.
    - Notification for incoming messages and errors.

## Endpoints

### **Users Endpoints**

- `POST /api/users/register` - Register a new user.
- `POST /api/users/login` - Log in and receive a JWT token.
- `GET /api/users/{id}` - Retrieve user information by ID.
- `GET /api/users/{username}` - Retrieve user information by username.
- `GET /api/users/me` - Retrieve the authenticated user’s information.
- `PATCH /api/users/me` - Update the authenticated user’s information.
- `PATCH /api/users/me/image` - Upload or update the user’s profile image.
- `DELETE /api/users/me/image` - Delete the user’s profile image.
- `PATCH /api/users/me/password` - Update the user’s password.

### **Messages Endpoints**

- `GET /api/messages/{userId}` - Retrieve messages exchanged with a specific user.

### **Chats Endpoints**

- `GET /api/chats` - Retrieve a list of active chats for the authenticated user.

### **Hubs Endpoints**

- `GET /api/hubs/ChatHub/info` - Retrieve metadata about the SignalR hub and its methods.

#### **SignalR Hub: `ChatHub`**

- **Methods**:

  - `SendMessage(string userId, string message)` - Send a message to a specific user.
  - `DeliveredMessage(string messageId)` - Mark a message as delivered.
  - `DeliveredAndReadMessage(string messageId)` - Mark a message as delivered and read.
  - `ReadMessages(string userChatId)` - Mark messages as read between the current user and another user.
  - `DeliveredAllMessages()` - Mark all messages as delivered for the current user.

- **Connection Events**:
  - `SendMessage` - Triggered when a message is sent and saved in the database.
  - `ReceiveMessage` - Triggered when a new message is received by the user.
  - `MessageStatus` - Triggered when the status of a message changes (e.g., delivered, read).
  - `Error` - Triggered when an error occurs, sending the error message to the client.

## Technologies Used

- **Framework**: ASP.NET Core
- **Database**: SQL Server with Entity Framework Core
- **Real-Time Communication**: SignalR
- **Authentication**: JWT Bearer Authentication
- **Architecture**: Clean Architecture
- **Libraries and Tools**:
  - `AutoMapper` - Object mapping.
  - `BCrypt.Net-Next` - Password hashing.
  - `FluentValidation.AspNetCore` - Input validation.
  - `MediatR` - CQRS and mediator pattern.
  - `Microsoft.AspNetCore.OpenApi` - Swagger/OpenAPI documentation.
  - `Scalar.AspNetCore` - Generate beautiful interactive API documentation from OpenAPI/Swagger documents.

## Live Demo

- **API**: [Live Demo](http://real-time-chat-api.runasp.net/scalar/v1)
- **Frontend Web App**: [Live Demo](https://real-time-chat-webapp.vercel.app/)  
  (This React web app uses the Real-Time Chat API.) [GitHub Repository](https://github.com/kerolesnabill/RealTimeChatWebApp)

## Setup Instructions

1. **Clone the Repository**:

   ```bash
   git clone https://github.com/kerolesnabill/RealTimeChatAPI.git
   cd RealTimeChatAPI
   ```

2. **Configure the Application**:

   - Update `appsettings.json` with your SQL Server connection string and JWT secret.

3. **Run Database Migrations**:

   ```bash
   dotnet ef database update
   ```

4. **Run the Application**:

   ```bash
   dotnet run
   ```

5. **Access Scalar documentation**:
   - Navigate to `http://localhost:{port}/scalar/v1` in your browser.

## License

This project is licensed under the MIT License.
