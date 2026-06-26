# Domain Class Diagram

Core models and the service interfaces they flow through. Implementations live in the
EntityFramework and WPF layers.

```mermaid
classDiagram
    class DomainObject {
        +int Id
    }
    class Student {
        +string Name
        +string Surname
        +string Username
        +string PasswordHash
        +string AcademicEmail
        +int AEM
        +otherDemographics
    }
    class Account {
        +Student AccountHolder
    }
    class FaceImage {
        +int StudentId
        +bytes ImageData
        +bytes Embedding
        +DateTime DateCaptured
    }
    class SessionHistory {
        +int StudentId
        +DateTime Date
        +TimeSpan ActiveTime
        +DateTime CheckIn
        +DateTime CheckOut
    }

    DomainObject <|-- Student
    DomainObject <|-- Account
    DomainObject <|-- FaceImage
    DomainObject <|-- SessionHistory
    Account "1" --> "1" Student : AccountHolder
    Student "1" --> "*" FaceImage
    Student "1" --> "*" SessionHistory

    class IAuthenticationService {
        <<interface>>
        +Login(username, password) Account
        +LoginWithFace(probeEmbedding) Account
        +Register(...) RegistrationResult
    }
    class IFaceRecognitionService {
        <<interface>>
        +EncodeAsync(jpeg) embedding
        +IsHealthyAsync() bool
    }
    class IFaceImageService {
        <<interface>>
        +SaveFaceImage(studentId, jpeg, embedding)
        +GetAllEmbeddings() list
        +GetAllImageData(studentId) list
        +GetFirstImageData(studentId) bytes
    }
    class EmbeddingSerializer {
        <<static>>
        +ToBytes(floats) bytes
        +ToFloats(bytes) floats
        +Distance(a, b) double
    }

    IAuthenticationService ..> IFaceImageService : matches against
    IAuthenticationService ..> EmbeddingSerializer : L2 distance
```

**Key idea:** `IFaceRecognitionService` (implemented over the Python service) only *encodes* a
face into a 128-d embedding; `IAuthenticationService.LoginWithFace` does the *matching* against
the stored embeddings. `ImageData` / `Embedding` are `byte[]` in code (the 128 floats are
packed into 512 bytes via `EmbeddingSerializer`); `CheckIn` / `CheckOut` are nullable.
