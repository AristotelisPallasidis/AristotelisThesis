# Entity Framework Layer Improvements

The `AristotelisThesis.EntityFramework` project handles data access and database infrastructure. To ensure optimal performance and code maintainability, consider the following improvements:

## 1. Extract Entity Configurations
**Current State:** Entity constraints and foreign key configurations are set directly inside the `OnModelCreating` method in `AristotelisThesisDbContext.cs` (e.g., mapping `FaceImage` relational behaviors).
**Improvement:** As the domain grows, `OnModelCreating` will become large and difficult to read. Extract these configurations into separate classes by implementing `IEntityTypeConfiguration<T>`. For example, create a `FaceImageConfiguration.cs` and simply apply it in `OnModelCreating` using `modelBuilder.ApplyConfigurationsFromAssembly(...)`.

## 2. Performance Tracking (AsNoTracking)
**Current State:** Queries fetching models likely retrieve them fully tracked by the EF Core Change Tracker.
**Improvement:** For queries that are purely for viewing (like calculating statistics or checking if a username exists before logging in), use `.AsNoTracking()`. This significantly reduces memory overhead and query execution time since Entity Framework doesn't need to monitor the object for changes.

## 3. Database Seeding
**Current State:** Testing may require entering dummy data manually.
**Improvement:** Utilize the `OnModelCreating` method (or a separate database initializer) to inject a few seed records (Administrator accounts, sample departments, default students) so that the application environment comes pre-populated and ready to test out of the box when the database is built or migrated.
