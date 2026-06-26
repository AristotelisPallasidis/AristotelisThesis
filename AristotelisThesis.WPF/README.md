# AristotelisThesis.WPF

## Overview
This is the **Presentation Layer** built with Windows Presentation Foundation (WPF). It serves as the front-end for the biometric identity recognition system.

## Responsibilities
- **MVVM Architecture**: Heavily uses the Model-View-ViewModel pattern to separate UI design (`Views`, `Controls`) from application logic (`ViewModels`).
- **Dependency Injection**: Utilizes `Microsoft.Extensions.DependencyInjection` configured in `App.xaml.cs` to resolve services, VMs, and Navigation mechanisms.
- **Camera Capture**: `Services/CameraCaptureService` owns webcam access via OpenCvSharp — it enumerates cameras by name (DirectShow, via AForge), streams frames to the UI, and snapshots the current frame as JPEG. Shared by the face-login and face-enrollment screens.
- **Face Recognition Bridge**: Talks to a local Python service that produces face embeddings (see below); matching is done in the Domain layer.
- **State & Navigation**: Handles user state tracking (Login/Logout) and dynamic view switching through Renavigator commands and View factories.

## Face Recognition (presentation side)
- **`Services/PythonFaceRecognitionService`** (`IFaceRecognitionService`): POSTs a captured JPEG to the Python `/encode` endpoint and returns the 128-d embedding.
- **`Services/PythonServiceLauncher`**: auto-starts the Python `face_service` (uvicorn) on app launch and kills it on exit; the app degrades gracefully if it can't start.
- **`LoginWithFaceViewModel`**: captures a frame → encodes it → `IAuthenticator.LoginWithFace` → on a match performs a real login (sets `CurrentAccount`, records attendance) and navigates to the Dashboard.
- **`FaceRecognitionViewModel` / View**: gallery page showing all of the logged-in student's enrolled images.
- **`Controls/FaceGuideOverlay`**: a transparent oval guide drawn over the camera feed so the user frames their face at a consistent distance (used on both the login and enrollment screens).

> Requires the `face_service` Python service (repo root). It's auto-started by the app; see `face_service/README.md` for one-time setup.

## Multi-Stage Registration and Recognition Flows
- **Registration** (palmprint steps currently skipped): Information → Face Instructions → Face Capture → Dashboard.
  - The info form (`Register02`) binds to a shared singleton `RegistrationStore`; every required field shows a red `*`, and the **Next** button (a `RelayCommand`) stays disabled until `RegistrationStore.IsPersonalInfoValid()` passes, re-checked live as the user types.
  - `Register06` captures one or more face photos, encodes each, then on **Ολοκλήρωση** creates the (biometric-only) account, stores the faces + embeddings, auto-logs-in via the just-enrolled face, and navigates to the Dashboard.
- **Login** variations: credentials, face, and palmprint (palmprint is still a placeholder).

## NuGet Packages
Target framework: **net10.0-windows** (`UseWPF`).

| Package | Version | Used for |
|---|---|---|
| `OpenCvSharp4` | 4.13.0.20260602 | Webcam capture & frame processing (`CameraCaptureService`) |
| `OpenCvSharp4.Windows` | 4.13.0.20260602 | Windows bundle incl. WPF extensions (frames → `BitmapSource`) |
| `OpenCvSharp4.runtime.win` | 4.13.0.20260602 | Native OpenCV runtime (Windows) |
| `AForge.Video.DirectShow` | 2.2.5 | Fast camera enumeration by name (DirectShow) |
| `LoadingSpinner.WPF` | 1.0.0 | Loading-spinner UI control |
| `System.Drawing.Common` | 10.0.9 | Image handling |

> Talks to the external `face_service` (Python/FastAPI) over HTTP via `HttpClient` — no NuGet package needed.

## Key Relationships
- **Depends on**: `AristotelisThesis.Domain` (for Models/interfaces) and `AristotelisThesis.EntityFramework` (for the DB contexts).
- Integrates `OpenCvSharp` / `OpenCvSharp.WpfExtensions` (camera frames → WPF images) and `AForge.Video.DirectShow` (fast camera enumeration).
- Calls the external `face_service` (Python/FastAPI) over HTTP on `127.0.0.1:8500`.
