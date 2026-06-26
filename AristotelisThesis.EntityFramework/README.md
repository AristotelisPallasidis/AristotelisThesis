# AristotelisThesis.EntityFramework

## Overview
This project represents the **Infrastructure / Data Access Layer** for the application. It is responsible for bridging the core Domain models with the actual relational database utilizing **Entity Framework Core**.

## Responsibilities
- **Entity Framework Context**: Contains `AristotelisThesisDbContext`, which manages the data sets (`Students`, `Accounts`, `FaceImages`, `SessionHistories`) and configures the SQL table mappings. `FaceImages.Embedding` is a nullable `varbinary(max)` column holding the 128-d face embedding.
- **Design-Time Factory**: Contains `AristotelisThesisDbContextFactory` to help EF Core tools (like migrations) instantiate the DbContext during development.
- **Migrations**: Hosts all the auto-generated migration steps used to keep the database schema in sync with the domain models (e.g. `AddFaceEmbedding`, which adds the embedding column).
- **Service Implementations**: Contains concrete implementations for data access services bound to the Domain interfaces — e.g. `AccountDataService` (`GetByStudentId`, cascade-aware `Delete`) and `FaceImageService` (`SaveFaceImage`, `GetAllEmbeddings`, `GetAllImageData`, `GetFirstImageData`).

## Tooling Commands
Using the Package Manager Console (`PM>`):
1. Create a migration: `Add-Migration <MigrationName>`
2. Apply changes to DB: `Update-Database`
3. Rollback migration: `Remove-Migration`

Or with the .NET CLI (the WPF startup project lacks the EF Design package, so use this project as its own startup):
```
dotnet ef migrations add <Name> -p AristotelisThesis.EntityFramework -s AristotelisThesis.EntityFramework
dotnet ef database update -p AristotelisThesis.EntityFramework -s AristotelisThesis.EntityFramework
```
The database is SQL Server LocalDB (`(localdb)\MSSQLLocalDB`, database `AristotelisThesisDB`).

## NuGet Packages
Target framework: **net10.0-windows**.

| Package | Version | Used for |
|---|---|---|
| `Microsoft.EntityFrameworkCore` | 10.0.9 | EF Core ORM |
| `Microsoft.EntityFrameworkCore.SqlServer` | 10.0.9 | SQL Server / LocalDB provider |
| `Microsoft.EntityFrameworkCore.Design` | 10.0.9 | Design-time support for migrations |
| `Microsoft.EntityFrameworkCore.Tools` | 10.0.9 | `Add-Migration` / `Update-Database` tooling |
| `OpenCvSharp4` | 4.13.0.20260602 | Computer-vision types shared across layers |
| `OpenCvSharp4.runtime.win` | 4.13.0.20260602 | Native OpenCV runtime (Windows) |
| `OpenCvSharp4.WpfExtensions` | 4.13.0.20260602 | OpenCV ↔ WPF imaging helpers |
| `System.Drawing.Common` | 10.0.9 | Image handling |

## Key Relationships
- **Depends on**: `AristotelisThesis.Domain` (for tracking the actual models).
- **Referenced by**: `AristotelisThesis.WPF` (to execute queries and dependency injection).
