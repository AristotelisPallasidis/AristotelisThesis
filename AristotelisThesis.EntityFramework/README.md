# AristotelisThesis.EntityFramework

## Overview
This project represents the **Infrastructure / Data Access Layer** for the application. It is responsible for bridging the core Domain models with the actual relational database utilizing **Entity Framework Core**.

## Responsibilities
- **Entity Framework Context**: Contains `AristotelisThesisDbContext`, which manages the data sets (`Students`, `Accounts`, `FaceImages`, etc.) and configures the SQL table mappings.
- **Design-Time Factory**: Contains `AristotelisThesisDbContextFactory` to help EF Core tools (like migrations) instantiate the DbContext during development.
- **Migrations**: Hosts all the auto-generated migration steps used to keep the database schema in sync with the domain models.
- **Service Implementations**: Contains concrete implementations for data access services (e.g. `AccountDataService`) bound to the generic interfaces defined in the Domain project.

## Tooling Commands
To interact with the database during development using the Package Manager Console (`PM>`):
1. Create a migration: `Add-Migration <MigrationName>`
2. Apply changes to DB: `Update-Database`
3. Rollback migration: `Remove-Migration`

## Key Relationships
- **Depends on**: `AristotelisThesis.Domain` (for tracking the actual models).
- **Referenced by**: `AristotelisThesis.WPF` (to execute queries and dependency injection).
