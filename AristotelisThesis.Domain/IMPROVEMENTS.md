# Domain Layer Improvements

The `AristotelisThesis.Domain` project holds the core business logic. Based on a code review, here are the architectural and clean-code improvements suggested for this specific project:

## 1. Separate Identity from Core Demographics
**Current State:** The `Student.cs` entity contains both personal demographic data (`Name`, `Address`, `Sex`) and authentication credentials (`Username`, `PasswordHash`).
**Improvement:** Separate these concerns. Create an `Account` or `User` entity strictly for identity and login management, which maps to a `Student` via a foreign key (e.g., `StudentId`). This prevents authentication queries from unnecessarily loading bulky demographic data, and prevents business updates from accidentally altering security fields.

## 2. Validation Pattern (FluentValidation)
**Current State:** Models rely entirely on DataAnnotations (`[Required]`, `[RegularExpression]`) which bloats the POCO (Plain Old C# Object) classes with magic strings and Regex.
**Improvement:** While DataAnnotations work perfectly for simple properties, consider using a library like **FluentValidation** for complex rules (such as validating the AEM number length or the specific Email suffix structure). This moves the validation logic into separate `<Model>Validator` classes, keeping the domain models clean and strictly focused on data shapes.

## 3. Strong Typing for Enumerations
**Current State:** Fields like `Sex` and `Department` are stored as `string` and validated against string expressions (e.g., `"^(Male|Female)$"`).
**Improvement:** Convert these constraints into `enum` data types (e.g., `public enum Gender { Male, Female }`). This naturally guarantees type-safety across the application and removes the need for regex constraint checks altogether.
