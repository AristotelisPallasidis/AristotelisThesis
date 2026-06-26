# AristotelisThesis.Domain

## Overview
This is the **Domain Layer** of the application. It acts as the core of the project, holding all the primary business objects, data models, and service interfaces that the rest of the application layers depend upon.

## Responsibilities
- **Domain Models**: Contains core entities like `Student`, `FaceImage`, `Account`, `SessionHistory`, etc. `FaceImage` stores both the raw image (`ImageData`) and its 128-d face embedding (`Embedding`, packed as bytes).
- **Validation**: Enforces basic validation logic via DataAnnotations (e.g., in the `Student` model, enforcing string lengths, email regexes, and required fields).
- **Service Interfaces**: Declares interfaces like `IDataService`, `IAccountService`, `IAuthenticationService`, etc., without providing concrete implementations. This ensures the domain has zero dependencies on databases or UI frameworks.

## Face Recognition (domain side)
The face-recognition login is split so the domain owns matching while a Python service only produces embeddings:
- **`IFaceRecognitionService`**: contract for turning a face image into a 128-d embedding (implemented in the WPF layer over the Python service).
- **`IFaceImageService`**: stores/loads enrolled faces and their embeddings (`SaveFaceImage`, `GetAllEmbeddings`, `GetAllImageData`, `GetFirstImageData`).
- **`EmbeddingSerializer`**: packs/unpacks `float[128]` ↔ `byte[]` and computes the Euclidean (L2) distance used for matching.
- **`IAuthenticationService.LoginWithFace(float[] probe)`**: matches a probe embedding against enrolled students (average L2 distance per student, threshold `0.45`) and returns the owning `Account`.

## Key Relationships
- **No external dependencies**: This project does not depend on `EntityFramework` or `WPF`.
- Provides models and logic consumed by both UI (`AristotelisThesis.WPF`) and infrastructure (`AristotelisThesis.EntityFramework`).

## NuGet Packages
Target framework: **net10.0-windows**.

| Package | Version | Used for |
|---|---|---|
| `Microsoft.AspNet.Identity.Core` | 2.2.4 | Password hashing (`IPasswordHasher`) used by `AuthenticationService` |
| `OpenCvSharp4` | 4.13.0.20260602 | Computer-vision types shared with the other layers |
| `OpenCvSharp4.runtime.win` | 4.13.0.20260602 | Native OpenCV runtime (Windows) |
| `OpenCvSharp4.WpfExtensions` | 4.13.0.20260602 | OpenCV ↔ WPF imaging helpers |
| `System.Drawing.Common` | 10.0.9 | Image handling |

## Future Notes
When adding new business features, start here by creating the necessary interfaces and data models before creating database tables or user interfaces.
