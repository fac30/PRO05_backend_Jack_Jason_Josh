# Sakura

## Table of Contents

1. [Project Overview](#project-overview)
2. [Tech Stack](#tech-stack)
3. [Installation Guide](#installation-guide)
4. [Database Models](#models)
   - 4.1 [User](#user)
   - 4.2 [Colour](#colour)
   - 4.3 [Collection](#collection)
   - 4.4 [ColourCollection](#colourcollection)
   - 4.5 [Comment](#comment)
5. [Endpoints](#endpoints)
   - 5.1 [Colour Endpoints](#colour-endpoints)
   - 5.2 [User Endpoints](#user-endpoints)

---

## Project Overview

This project is a backend API for managing colour-related data, including users, collections, and comments. It is built using ASP.NET Core and EF Core, connected to a PostgreSQL database. The API provides endpoints for CRUD operations on users, colours, collections, and comments, with support for associating colours with collections and users.

---

## Tech Stack

- **Backend**: ASP.NET Core
- **Database**: PostgreSQL
- **ORM**: Entity Framework Core
- **API Documentation**: Swagger
- **CORS**: Configured to allow requests from the frontend

---

## Colour Collection Management System API Documentation

## Models

### Colour 
```json
{
    "id": 1,
    "hex": "#FF5733"
}
```
- `id` (int): Unique identifier
- `hex` (string): Hexadecimal color code

### User 
```json
{
    "id": 1,
    "name": "John Doe",
    "email": "john@example.com",
    "createdAt": "2024-01-15T10:30:00Z"
}
```
- `id` (int): Unique identifier
- `name` (string): User's full name
- `email` (string): User's email address
- `createdAt` (datetime): Account creation timestamp

### Collection 
```json
{
    "id": 1,
    "type": "palette",
    "isPublic": true,
    "userId": 1,
    "name": "Summer Vibes",
    "description": "Warm and bright colors",
    "createdAt": "2024-01-20T15:45:00Z"
}
```
- `id` (int): Unique identifier
- `type` (string): Collection type
- `isPublic` (bool): Visibility flag
- `userId` (int): Creator's user ID
- `name` (string): Collection name
- `description` (string): Optional description
- `createdAt` (datetime): Collection creation timestamp

### ColourCollection 
```json
{
    "id": 1,
    "colourId": 5,
    "collectionId": 3,
    "order": 2
}
```
- `id` (int): Unique identifier
- `colourId` (int): Referenced color ID
- `collectionId` (int): Referenced collection ID
- `order` (int): Color placement order

### Comment 
```json
{
    "id": 1,
    "userId": 2,
    "collectionId": 1,
    "content": "Love these colors!",
    "createdAt": "2024-01-22T09:15:00Z"
}
```
- `id` (int): Unique identifier
- `userId` (int): Commenter's user ID
- `collectionId` (int): Commented collection ID
- `content` (string): Comment text
- `createdAt` (datetime): Comment timestamp

## Endpoints

## Colour Endpoints

### GET /colours
- **Description**: Retrieve all colors
- **Parameters**: None
- **Response**:
```json
[
    {"id": 1, "hex": "#FF5733"},
    {"id": 2, "hex": "#33FF57"}
]
```
- **Status Codes**:
  - 200 OK: Successful retrieval
  - 500 Internal Server Error: Unexpected error

### GET /colours/{id}
- **Description**: Retrieve specific color by ID
- **Parameters**: 
  - `id` (path parameter, integer): Color's unique identifier
- **Response**:
```json
{"id": 1, "hex": "#FF5733"}
```
- **Status Codes**:
  - 200 OK: Color found
  - 404 Not Found: Color doesn't exist

### POST /colours
- **Description**: Create a new color
- **Request Body**:
```json
{"hex": "#4433FF"}
```
- **Response**:
```json
{"id": 3, "hex": "#4433FF"}
```
- **Status Codes**:
  - 201 Created: Color successfully added
  - 400 Bad Request: Invalid input
  - 500 Internal Server Error: Unexpected error

## User Endpoints

### GET /users
- **Description**: Retrieve all users
- **Parameters**: None
- **Response**:
```json
[
    {"id": 1, "name": "John Doe", "email": "john@example.com"},
    {"id": 2, "name": "Jane Smith", "email": "jane@example.com"}
]
```
- **Status Codes**:
  - 200 OK: Successful retrieval
  - 500 Internal Server Error: Unexpected error

### GET /users/{id}
- **Description**: Retrieve specific user by ID
- **Parameters**: 
  - `id` (path parameter, integer): User's unique identifier
- **Response**:
```json
{"id": 1, "name": "John Doe", "email": "john@example.com"}
```
- **Status Codes**:
  - 200 OK: User found
  - 404 Not Found: User doesn't exist

### POST /users
- **Description**: Create a new user
- **Request Body**:
```json
{
    "name": "Alice Johnson",
    "email": "alice@example.com",
    "hash": "hashedpassword123"
}
```
- **Response**:
```json
{
    "id": 3,
    "name": "Alice Johnson",
    "email": "alice@example.com"
}
```
- **Status Codes**:
  - 201 Created: User successfully added
  - 400 Bad Request: Invalid input
  - 500 Internal Server Error: Unexpected error
