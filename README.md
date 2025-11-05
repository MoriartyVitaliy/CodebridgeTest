# Codebridge Test Backend

This project is a test backend application built using **.NET 8**, applying modern architecture patterns such as **Clean Architecture**, **CQRS**, and **MediatR**.
It manages a list of dogs with support for sorting, pagination, validation, and constraints.

##  Technologies

| Technology                         | Purpose                                        |
| ---------------------------------- | ---------------------------------------------- |
| .NET 8                             | API runtime                                    |
| ASP.NET Core Web API               | Backend                                        |
| Swagger                            | Comfortable testfield documentation            |
| CQRS + MediatR                     | Separation of concerns, clean request pipeline |
| Entity Framework Core              | Data Access                                    |
| SQL Server / PostgreSQL / InMemory | Database providers                             |
| Redis                              | Rate limiting & caching                        |
| FluentValidation                   | Input validation                               |
| Docker                             | Deployment                                     |
| xUnit + Moq + FluentAssertions     | Automated tests                                |

---

##  Features

* ✔ Clean Architecture
* ✔ CQRS + MediatR structure
* ✔ Dogs CRUD (create / list / delete)
* ✔ Sorting & Pagination
* ✔ Global Redis rate limiting middleware
* ✔ Centralized error handling
* ✔ Repository abstraction
* ✔ Unit tests (handlers, services, repository)
* ✔ Database-agnostic design
* ✔ Docker for deployment

##  Endpoints

| Method   | Endpoint      | Description                              |
| -------- | ------------- | ---------------------------------------- |
| `GET`    | `/dogs`       | Get all dogs (with sorting + pagination) |
| `GET`    | `/dog/{name}` | Get one dog by name                      |
| `PUT`    | `/dog/{name}` | Update dog data                          |
| `POST`   | `/dog`        | Add new dog                              |
| `DELETE` | `/dog/{name}` | Delete dog by name                       |
| `GET`    | `/ping`       | Service health check                     |

##  Project Structure

| Project                    | Purpose                                       |
| -------------------------- | --------------------------------------------- |
| CodebridgeTest.API         | Entrypoint, Controllers, Middleware, Settings |
| CodebridgeTest.Application | Commands, Queries, Handlers, Services         |
| CodebridgeTest.Core        | Entities, Interfaces, Custom Exceptions       |
| CodebridgeTest.Persistence | EF Core DbContext, Migrations, Repository     |
| CodebridgeTest.Tests       | xUnit tests                                   |

###  Query Parameters (GET /dogs)

| Parameter    | Description     | Example                         |
| ------------ | --------------- | ------------------------------- |
| `attribute`  | Sort field      | `name`, `tail_length`, `weight` |
| `order`      | Sort order      | `asc`, `desc`                   |
| `pageNumber` | Pagination page | `1`                             |
| `pageSize`   | Items per page  | `10`                            |

---

##  Example Requests

###  Get Dogs (with sorting & pagination)

```
GET /dogs?attribute=weight&order=desc&pageNumber=1&pageSize=5
```

```bash
curl -X GET "https://localhost:7173/dogs?attribute=weight&order=desc&pageNumber=1&pageSize=5" \
  -H "accept: application/json"
```

###  Get Dog by Name

```
GET /dog/Neo
```

```bash
curl -X GET "https://localhost:7173/dog/Neo" \
  -H "accept: application/json"
```

###  Create Dog

```
POST /dog
```

```bash
curl -X POST "https://localhost:7173/dog" \
  -H "accept: application/json" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Jessy",
    "color": "black & white",
    "tailLength": 7,
    "weight": 14
  }'
```

###  Update Dog

```
PUT /dog/Jessy
```

```bash
curl -X PUT "https://localhost:7173/dog/Jessy" \
  -H "accept: application/json" \
  -H "Content-Type: application/json" \
  -d '{
    "color": "white & brown",
    "tailLength": 8,
    "weight": 15
  }'
```

###  Delete Dog

```
DELETE /dog/Jessy
```

```bash
curl -X DELETE "https://localhost:7173/dog/Jessy" \
  -H "accept: application/json"
```

###  Health Check

```
GET /ping
```

```
"Dogshouseservice.Version1.0.1"
```

---
