# AristotelisThesis.Domain

## Overview
This is the **Domain Layer** of the application. It acts as the core of the project, holding all the primary business objects, data models, and service interfaces that the rest of the application layers depend upon.

## Responsibilities
- **Domain Models**: Contains core entities like `Student`, `Account`, `FaceImage`, `PalmprintImage` and `SessionHistory`. `FaceImage` and `PalmprintImage` each store both the raw image (`ImageData`) and its feature vector (`Embedding`, packed as bytes).
- **Read-models**: `AttendanceStatistics` and `WeeklyAttendanceDataPoint` are **not** stored entities — they are projected from `SessionHistory` on demand (see `IStatisticsService`).
- **Validation**: Enforces basic validation logic via DataAnnotations (e.g., in the `Student` model, enforcing string lengths, email regexes, and required fields).
- **Service Interfaces**: Declares interfaces like `IDataService`, `IAccountService`, `IAuthenticationService`, etc., without providing concrete implementations. This ensures the domain has zero dependencies on databases or UI frameworks.

## Biometric matching (domain side)
Both biometric flows are split the same way: a Python service only produces feature vectors, and the domain owns the matching.

- **`IFaceRecognitionService`** / **`IPalmprintRecognitionService`**: contracts for turning an image into a feature vector (implemented in the WPF layer over the Python services).
- **`IFaceImageService`**: stores/loads enrolled faces and their embeddings — `SaveFaceImages`, `GetAllEmbeddings`, `GetAllImageData`, `GetFirstImageData`.
- **`IPalmprintImageService`**: the palmprint mirror — `SavePalmprintImages`, `GetAllEmbeddings`, `GetAllImageData`.
  Both `Save…Images` methods take the whole captured set and persist it in **one** save, so an enrolment is never left half-written.
- **`EmbeddingSerializer`**: packs/unpacks `float[]` ↔ `byte[]` and computes the Euclidean (L2) distance used for matching.
- **`IAuthenticationService`**: biometric-only. `LoginWithFace(float[] probe)` and `LoginWithPalmprint(float[] probe)` each match a probe against the enrolled students (average L2 distance per student) and return the owning `Account`, or `null` if nothing is close enough. There is no username/password path.

| Modality | Threshold | Notes |
|---|---|---|
| Face | `0.45` | dlib's recommended L2 threshold |
| Palmprint | `0.6` | **Starting value — not yet calibrated against real captures** |

## Attendance
- **`ISessionTrackingService`**: records a check-in when a biometric login succeeds.
- **`IStatisticsService`**: aggregates a student's `SessionHistory` rows into an `AttendanceStatistics` read-model (days attended this week, streak, monthly percentage, check-in time, weekly graph points). Nothing is written back.

## Key Relationships
- **No external dependencies**: This project does not depend on `EntityFramework` or `WPF`.
- Provides models and logic consumed by both UI (`AristotelisThesis.WPF`) and infrastructure (`AristotelisThesis.EntityFramework`).

## NuGet Packages
Target framework: **net10.0-windows**.

| Package | Version | Used for |
|---|---|---|
| `OpenCvSharp4` | 4.13.0.20260627 | Computer-vision types shared with the other layers |
| `OpenCvSharp4.runtime.win` | 4.13.0.20260627 | Native OpenCV runtime (Windows) |
| `OpenCvSharp4.WpfExtensions` | 4.13.0.20260627 | OpenCV ↔ WPF imaging helpers |
| `System.Drawing.Common` | 10.0.11 | Image handling |

## Future Notes
When adding new business features, start here by creating the necessary interfaces and data models before creating database tables or user interfaces.
