# What's This?
An example to demonstrate how to query database without ORM in the context of WPF app.

# How Do I Run This?
## 1. Database Setup (Choose One)
### 1.1. Docker
1. Have [Docker](https://www.docker.com/products/docker-desktop/) installed.
2. Create `.env` file
    ```
    POSTGRES_DB=orders
    POSTGRES_PASSWORD=super_secret_db_passport
    ```
3. Run `docker-compose up`, still will start local Postgres instance via Docker, create the database you specified in `.env` file and protect it with the password you specified in the same file.

### 1.2. Local Postgres
1. Download and run [Postgres](https://www.postgresql.org/download/) locally.
2. Create a database (e.g. `orders`). Or just use default `postgres` one, doesn't really matter.

## 2. Run the App
### 2.1. With IDE
1. In the project startup settings create an env var called `DB_CONNECTION_STRING` and put a connection string to your database here. It should probably look like this: 
    ```
    DB_CONNECTION_STRING=User ID=postgres; Password=super_secret_db_password; Host=localhost; Port=5432; Database=orders
    ``` 
2. Run the project with IDE.

### 2.2. dotnet run
1. Create a script for the shell of your choice that will set the `DB_CONNECTION_STRING` env var and then call `dotnet run`.
2. Alternatively you can put the env var inline (e.g. in Bash call from the project folder `DB_CONNECTION_STRING="User ID=postgres; Password=super_secret_db_password; Host=localhost; Port=5432; Database=orders" dotnet run`)
