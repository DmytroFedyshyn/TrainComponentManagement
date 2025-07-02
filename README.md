# TrainComponentManagement

A sample three-layered .NET **Train Component Management** system built with **ASP.NET Core Web API**, EF Core, Pooling & Transient Fault Retry, idempotency, bulk operations, and comprehensive testing.


### DbContext Pooling & Transient Fault Retry

> **Requirement:**  
> “The system should be designed to efficiently handle a large number of items, optimized for both read and write operations. The system should also include robust mechanisms for handling errors and exceptional conditions, to ensure that the system remains stable and reliable even under heavy load or in the presence of network errors or other unexpected situations.”

- **DbContext Pooling** improves performance by reusing `DbContext` instances instead of recreating them for every request.  
- **EnableRetryOnFailure** automatically retries transient SQL Server errors (like deadlocks or connection timeouts), ensuring the system remains stable and reliable under high load or unreliable network conditions.

---

### Idempotency

> **Requirement:**  
> “Robust mechanisms for handling errors and exceptional conditions.”

- When users perform create or bulk insert operations, network issues or timeouts could cause retries.  
- Using an **`Idempotency-Key` header** ensures the same request won’t accidentally duplicate data if re-sent — it guarantees exactly-once semantics for critical operations.

---

### Bulk Operations

> **Requirement:**  
> “Allow users to quickly find a specific component, and also to rapidly perform operations such as adding or removing components.”

- For large datasets (e.g., adding or removing hundreds or thousands of components at once), standard `SaveChangesAsync` becomes inefficient.
- With **`EFCore.BulkExtensions`**, `BulkInsertAsync` and `BulkDeleteAsync` perform these operations with a single optimized SQL statement, dramatically improving performance.

---

  In service layer, `CreateExecutionStrategy` is used together with `BeginTransactionAsync` to wrap all critical operations in a single atomic transaction with automatic retries for transient failures such as network issues or deadlocks.

This guarantees that operations remain safe and consistent — either they succeed entirely, or they fail and roll back — providing production-level reliability exactly as required by the project spec.

---

## Features

### Data Access Layer (DAL)
- **Entity Framework Core** (Code-First, Fluent-API configurations)  
  - Proper modeling, indexes (including filtered and composite), check constraints  
  - Seed data via `HasData`
- **Repositories**  
  - CRUD methods (`GetAllAsync`, `GetByIdAsync`, `AddAsync`, `UpdateAsync`, `Remove`, batch operations)  
  - **BulkInsertAsync** and **BulkDeleteAsync** powered by [EFCore.BulkExtensions](https://github.com/borisdj/EFCore.BulkExtensions)

### Business Logic Layer (BLL)
- **DTOs** (`CreateComponentDto`, `ComponentDto`)  
- **Validation** with **FluentValidation**  
- **AutoMapper** profiles for DTO ↔ Entity mapping  
- **Services**  
  - Async CRUD (`CreateAsync`, `UpdateAsync`, `DeleteAsync`, `GetAsync`, `GetAllAsync`)  
  - **Idempotency** via `Idempotency-Key` header  
  - **Bulk operations** (`BulkInsertAsync`, `BulkDeleteAsync`) wrapped in an EF Core execution strategy & transaction

### Presentation Layer (PL)
- **ASP.NET Core Web API** controllers  
  - Standard CRUD endpoints:  
    - `GET    /api/components`  
    - `GET    /api/components/{id}`  
    - `POST   /api/components`  
    - `PUT    /api/components/{id}`  
    - `DELETE /api/components/{id}`  
  - **Bulk endpoints**:  
    - `POST /api/components/bulk`  
    - `POST /api/components/bulk-delete`
- **Global exception middleware** for uniform error handling  
- **Swagger / OpenAPI** for interactive API docs

---

## Tech Stack

- **.NET 9.0**  
- **ASP.NET Core Web API**  
- **Entity Framework Core**  
- **EFCore.BulkExtensions**  
- **AutoMapper**  
- **FluentValidation**  
- **SQL Server** (or LocalDB for development)  
- **xUnit**, **Moq**, **FluentAssertions**, **Faker.Net**, **FluentValidation.TestHelper** (testing)

---

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)  
- [SQL Server / LocalDB](https://docs.microsoft.com/sql/database-engine/configure-windows/sql-server-express-localdb)

### Clone & Setup

```bash
git clone https://github.com/your-org/TrainComponentManagement.git
cd TrainComponentManagement/PL
```

1. **Configure connection string** in `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=TrainComponentDb;Trusted_Connection=True;"
     }
   }
   ```

2. **Apply migrations & seed data**:
   ```bash
   dotnet tool install --global dotnet-ef
   
   dotnet ef migrations add InitialCreate --project ../DAL/TrainComponentManagement.DAL.csproj --startup-project TrainComponentManagement.PL.csproj

   dotnet ef database update --project ../DAL/TrainComponentManagement.DAL.csproj --startup-project TrainComponentManagement.PL.csproj
   ```

3. **Run the API**:
   ```bash
   dotnet run
   ```
   Swagger UI: `https://localhost:7121/swagger`

---

## DbContext Pooling & Transient Fault Retry

- **DbContext Pooling** reuses contexts to reduce allocation overhead.  
- **EnableRetryOnFailure** automatically retries transient SQL errors (network blips, deadlocks).

---

## Bulk Operations

- **BulkInsertAsync**: high-performance batch inserts with identity output.  
- **BulkDeleteAsync**: single-statement deletes via `WHERE Id IN (…)`.  
- Both wrapped in EF Core execution strategy & transaction, guarded by `Idempotency-Key`.

---

## Testing

Run all tests:
```bash
dotnet test
```
- **DAL.Tests**: repository CRUD  
- **BLL.Tests**: validation, service idempotency 
- **PL.Tests**: controller tests (Moq, FluentAssertions, Faker.Net)

---

## Future Enhancements

- API Rate Limiting & Request Logging
- Notification about crtitical errors by Messenger (Telegram, etc..)
- OpenTelemetry/Tracing (Prometheus, Jaeger or Zipkin)

---
