# AristotelisThesis.EntityFramework

## Overview
This project represents the **Infrastructure / Data Access Layer** for the application. It is responsible for bridging the core Domain models with the actual relational database utilizing **Entity Framework Core**.

## Responsibilities
- **Entity Framework Context**: Contains `AristotelisThesisDbContext`, which manages the data sets (`Students`, `Accounts`, `FaceImages`, `PalmprintImages`, `SessionHistories`) and configures the SQL table mappings.
  - `FaceImages.Embedding` and `PalmprintImages.Embedding` are nullable `varbinary(max)` columns holding the packed feature vectors.
  - Face, palmprint and session rows all cascade-delete with their `Student`.
  - `Student.AEM` has a **unique index** — the university's student number is unique by definition, so it is enforced in the database rather than only documented in the model.
- **Design-Time Factory**: `AristotelisThesisDbContextFactory` lets the EF tools instantiate the context. It takes an optional connection string (the WPF app passes the one from `appsettings.json`) and falls back to `DefaultConnectionString` when none is supplied — which is also the path the parameterless constructor used by `dotnet ef` takes.
- **Migrations**: Hosts all the auto-generated migration steps used to keep the database schema in sync with the domain models.
- **Service Implementations**: Concrete implementations bound to the Domain interfaces:

| Service | Provides |
|---|---|
| `AccountDataService` | `GetByStudentId`, cascade-aware `Delete`, plus the generic CRUD |
| `FaceImageService` | `SaveFaceImages`, `GetAllEmbeddings`, `GetAllImageData`, `GetFirstImageData` |
| `PalmprintImageService` | `SavePalmprintImages`, `GetAllEmbeddings`, `GetAllImageData` |
| `SessionTrackingService` | Records the daily attendance check-in on a successful biometric login |
| `StatisticsService` | Projects `AttendanceStatistics` from `SessionHistory` rows (read-only; nothing persisted) |
| `GenericDataService` / `NonQueryDataService` | Shared CRUD plumbing |

Both `Save…Images` methods write the whole captured set in a single `SaveChangesAsync`, so an enrolment is stored whole or not at all.

## Migrations

| Migration | Adds |
|---|---|
| `initial` | Students, Accounts |
| `addNewMaxSemester` | Semester constraint change |
| `addPasswordHash` | `Student.PasswordHash` (now vestigial — auth is biometric-only) |
| `addAccountRelatedToStudent` | Account ↔ Student relationship |
| `addRandomMigration` | — |
| `AddFaceImage` | `FaceImages` table |
| `AddSessionHistory` | `SessionHistories` table (the attendance ledger) |
| `AddFaceEmbedding` | `FaceImages.Embedding` column |
| `AddPalmprintImages` | `PalmprintImages` table |
| `AddUniqueStudentAem` | `IX_Students_AEM` unique index |

> `AddUniqueStudentAem` **fails on a database that already contains duplicate AEMs.** Resolve the duplicates before running `database update`.

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
The database is SQL Server LocalDB (`(localdb)\MSSQLLocalDB`, database `AristotelisThesisDB`). At runtime the WPF app overrides this with `ConnectionStrings:DefaultConnection` from its `appsettings.json`; the EF tools always use `DefaultConnectionString`.

## NuGet Packages
Target framework: **net10.0-windows**.

| Package | Version | Used for |
|---|---|---|
| `Microsoft.EntityFrameworkCore` | 10.0.11 | EF Core ORM |
| `Microsoft.EntityFrameworkCore.SqlServer` | 10.0.11 | SQL Server / LocalDB provider |
| `Microsoft.EntityFrameworkCore.Design` | 10.0.11 | Design-time support for migrations |
| `Microsoft.EntityFrameworkCore.Tools` | 10.0.11 | `Add-Migration` / `Update-Database` tooling |
| `OpenCvSharp4` | 4.13.0.20260627 | Computer-vision types shared across layers |
| `OpenCvSharp4.runtime.win` | 4.13.0.20260627 | Native OpenCV runtime (Windows) |
| `OpenCvSharp4.WpfExtensions` | 4.13.0.20260627 | OpenCV ↔ WPF imaging helpers |
| `System.Drawing.Common` | 10.0.11 | Image handling |

## Key Relationships
- **Depends on**: `AristotelisThesis.Domain` (for tracking the actual models).
- **Referenced by**: `AristotelisThesis.WPF` (to execute queries and dependency injection).
