# AristotelisThesis.Domain

## Overview
This is the **Domain Layer** of the application. It acts as the core of the project, holding all the primary business objects, data models, and service interfaces that the rest of the application layers depend upon.

## Responsibilities
- **Domain Models**: Contains core entities like `Student`, `PalmprintImage`, `FaceImage`, `Account`, `AttendanceStatistics`, etc.
- **Validation**: Enforces basic validation logic via DataAnnotations (e.g., in the `Student` model, enforcing string lengths, email regexes, and required fields).
- **Service Interfaces**: Declares interfaces like `IDataService`, `IAuthenticationService`, etc., without providing concrete implementations. This ensures the domain has zero dependencies on databases or UI frameworks.

## Key Relationships
- **No external dependencies**: This project does not depend on `EntityFramework` or `WPF`.
- Provides models and logic consumed by both UI (`AristotelisThesis.WPF`) and infrastructure (`AristotelisThesis.EntityFramework`).

## Future Notes
When adding new business features, start here by creating the necessary interfaces and data models before creating database tables or user interfaces.
