# Project ArticlesAPI - .NET 8 - API RestFul

This project is a RestFul API developed with .NET 8. Below are the instructions to clone the repository, configure the environment, and run the project.

## Prerequisites

Make sure you have the following components installed on your machine:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Git](https://git-scm.com/downloads)

## Clone the Repository

To clone this repository, open a terminal and run the following command:

```sh
git clone https://github.com/your-username/your-repository.git
cd your-repository
```
## Environment Variables

### JWT Key 

This project uses Jwt. Make sure you have the key in the environment variables (appsettings.json or appsettings.Development.json)

```sh
{
  "jwt": {
    "key": "YourKey"
  }
}
```

### Database Configuration

This project uses SQL Server as the database. Make sure you have a running instance of SQL Server and create a database for the project.

#### Connection String Configuration

In the appsettings.json or appsettings.Development.json file (depending on your environment), configure the connection string to your SQL Server database:

```sh
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=YourDataBase;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;"
  }
}
```
## Restore package

Restore the necessary NuGet packages for the project by running the following command in the terminal:

```sh
dotnet restore
```
## Create and Apply Database Migrations

First, create a migration to set up the initial database schema:

```sh
dotnet ef migrations add Initial or add-migration Initial
```

Then, apply the migration to set up the database:

```sh
dotnet ef database update or update-database
```

## Run the Project

Finally, to run the project, execute the following command:

```sh
dotnet run
```

