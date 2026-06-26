# System Architecture

Layered architecture of the .NET solution plus the external Python face-embedding service.
Arrows point in the direction of dependency / calls.

```mermaid
flowchart TB
    subgraph WPF["AristotelisThesis.WPF  (Presentation / MVVM)"]
        Views["Views & Controls<br/>(LoginWithFace, Register06, FaceRecognition, FaceGuideOverlay)"]
        VMs["ViewModels"]
        Cam["CameraCaptureService<br/>(OpenCvSharp + AForge)"]
        Bridge["PythonFaceRecognitionService<br/>+ PythonServiceLauncher"]
        DI["App.xaml.cs (Dependency Injection)"]
    end

    subgraph Domain["AristotelisThesis.Domain  (Core)"]
        Models["Models<br/>(Student, Account, FaceImage, SessionHistory)"]
        Ifaces["Service Interfaces<br/>(IAuthenticationService, IAccountService,<br/>IFaceImageService, IFaceRecognitionService)"]
        Match["AuthenticationService.LoginWithFace<br/>+ EmbeddingSerializer (L2 distance)"]
    end

    subgraph EF["AristotelisThesis.EntityFramework  (Data Access)"]
        DbCtx["AristotelisThesisDbContext"]
        Services["Data Services<br/>(AccountDataService, FaceImageService,<br/>SessionTrackingService)"]
    end

    subgraph Py["face_service  (Python / FastAPI)"]
        API["POST /encode , GET /health"]
        Model["dlib ResNet-34<br/>(face_recognition) -> 128-d embedding"]
    end

    DB[("SQL Server LocalDB<br/>AristotelisThesisDB")]

    Views --> VMs
    VMs --> Cam
    VMs --> Ifaces
    Bridge -. implements .-> Ifaces
    Match -. uses .-> Ifaces
    Services -. implements .-> Ifaces
    EF --> Domain
    WPF --> Domain
    Services --> DbCtx --> DB
    Bridge -- "HTTP 127.0.0.1:8500" --> API --> Model
```

**Key idea:** the Python service is a *stateless encoder*; embeddings are stored in the
database and **matching happens in C#** (Domain layer). Camera capture stays in C#; only the
captured JPEG crosses to Python.
