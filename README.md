
<h1 align="center">.NET Core CRUD API - Order, Product, User</h1>

<p align="center">
  <img src="https://img.shields.io/badge/.NET_Core-5.0-blueviolet" alt=".NET Core Version" />
  <img src="https://img.shields.io/badge/Swagger-Enabled-brightgreen" alt="Swagger Enabled" />
  
</p>

<p align="center">
  <img src="https://skillicons.dev/icons?i=dotnet" alt="Tech Stack Icons" />
</p>

## Overview

This is a **CRUD API** built with **.NET Core** to manage **Orders**, **Products**, and **Users**. The API supports creating, reading, updating, and deleting data for each of these entities. The project is designed to follow clean architecture principles, is fully documented with **Swagger**, and uses **Entity Framework Core** as the ORM for database operations.

## Features

- CRUD Operations for **Orders**, **Products**, and **Users**
- RESTful API design
- **Swagger UI** for interactive API documentation
- **Entity Framework Core** with migrations
- Input validation and error handling
- **Dependency Injection** for better code maintainability
- **JWT Authentication** for secure API access 

## Tech Stack

- **.NET Core 5.0**
- **Entity Framework Core** (Code-First Approach)
- **PostgreSQL r** as the database
- **Swagger** for API documentation
- **JWT** for authentication

## Project Structure

```bash
dotnet-crud-api/
│
├── Controllers/        # API Controllers for Order, Product, and User
├── Data/               # EF Core DbContext and Migrations
├── Models/             # Order, Product, User Models
├── Services/           # Business logic and service layer
├── DTOs/               # Data Transfer Objects for request/response
├── Repositories/       # Repository pattern for data access
└── Program.cs          # Application startup

```
## Setup & Installation
--------------------

-   **Clone the repository**

    ```bash
    git clone https://github.com/mochrks/crud-api-netcore.git
    cd crud-api-netcore/WebApi

    ```

-   **Install dependencies**

    ```bash
    dotnet restore
    ```

-   **Set up the database**

    -   Update your connection string in `appsettings.json`.
    -   Run database migrations:
    
    ```bash
    dotnet ef database update
    ```

-   **Run the application**

    ```bash
    dotnet run
    ```

-   **Access Swagger UI**  
    Navigate to [http://localhost:5000/swagger](http://localhost:5000/swagger) to interact with the API documentation.


## Connect with me:
[![GitHub](https://img.shields.io/badge/GitHub-333?style=for-the-badge&logo=github&logoColor=white)](https://github.com/mochrks)
[![YouTube](https://img.shields.io/badge/YouTube-FF0000?style=for-the-badge&logo=youtube&logoColor=white)](https://youtube.com/@Gdvisuel)
[![Instagram](https://img.shields.io/badge/Instagram-E4405F?style=for-the-badge&logo=instagram&logoColor=white)](https://instagram.com/mochrks)
[![LinkedIn](https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white)](https://linkedin.com/in/mochrks)
[![Behance](https://img.shields.io/badge/Behance-1769FF?style=for-the-badge&logo=behance&logoColor=white)](https://behance.net/mochrks)
[![Dribbble](https://img.shields.io/badge/Dribbble-EA4C89?style=for-the-badge&logo=dribbble&logoColor=white)](https://dribbble.com/mochrks)
