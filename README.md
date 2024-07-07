# BLA .NET Web API

## User story
As a user of the application
I want to be able to register for an account, securely log in, and then create, read, update, and delete my data records
So that I can manage my information effectively within the system.

## Getting Started

This project use Postgres database and ADO.NET to the operations on the database.

## Clean architecture structure
    .
    ├── BLA.UserFlow.API                # presentation layer
    ├── BLA.UserFlow.Application        # use cases (services), requests and responses
    ├── BLA.UserFlow.Core               # core domain (entities, value objects)
    ├── BLA.UserFlow.Infrastructure     # interaction with external dependencies (Database, IoC)
    └── BLA.UserFlow.Tests              # unit test

Other folder

    .
    ├── init-scripts                    # database scripts

### Prerequisites

1. Docker (https://docs.docker.com/engine/install/)

2. Start up all services
```
docker-compose up
```
3. Navigate to
```
http://localhost:8080/swagger/index.html
```
4. Stop services
```
docker-compose down
```

## API Endpoints

| Method | Endpoint   | Description   |
|--------|------------|---------------|
| GET    | /api/users | Get all users |


## Resources