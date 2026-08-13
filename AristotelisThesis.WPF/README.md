# AristotelisThesis.WPF

## Overview
This is the **Presentation Layer** built with Windows Presentation Foundation (WPF). It serves as the front-end for the biometric identity recognition system.

## Responsibilities
- **MVVM Architecture**: Heavily uses the Model-View-ViewModel pattern to separate UI design (`Views`, `Controls`) from application logic (`ViewModels`).
- **Dependency Injection**: Uses `Microsoft.Extensions.DependencyInjection`, configured in `App.xaml.cs`, to resolve services, view models and navigation.
  > **View models that show the signed-in student's data are registered `AddTransient`** (`DashboardViewModel`, `StatisticsViewModel`, `ProfileViewModel`, `FaceRecognitionViewModel`, `PalmprintRecognitionViewModel`). Registering them as singletons would let one student see the previous student's data after a logout.
- **Configuration**: the SQL connection string is read at startup from `appsettings.json` (`ConnectionStrings:DefaultConnection`, copied to the output folder). If the file is missing or malformed the app falls back to the LocalDB default in `AristotelisThesisDbContextFactory`.
- **Camera Capture**: `Services/CameraCaptureService` owns webcam access via OpenCvSharp — it enumerates cameras by name (DirectShow, via AForge), streams frames to the UI, and snapshots the current frame as JPEG. Shared by all four capture screens (face/palm × login/enrolment).
- **Biometric Bridge**: talks to two local Python services that produce feature vectors (see below); matching is done in the Domain layer.
- **State & Navigation**: Handles user state tracking (Login/Logout) and dynamic view switching through Renavigator commands and View factories.

## Biometric bridge (presentation side)

| Type | Role |
|---|---|
| `Services/PythonFaceRecognitionService` (`IFaceRecognitionService`) | POSTs a captured JPEG to `/encode` and returns the 128-d embedding |
| `Services/PythonPalmprintRecognitionService` (`IPalmprintRecognitionService`) | POSTs a captured JPEG to `/encode-palm` and returns the palm feature vector |
| `Services/PythonServiceLauncher` | Starts a uvicorn service on app launch and kills it on exit. **Two instances**: `face_service` and `palmprint_service`. The app degrades gracefully if one can't start |
| `Services/FaceServiceConfig` / `PalmprintServiceConfig` | Host and port for each service (`127.0.0.1:8500` and `127.0.0.1:8501`) |

- **`LoginWithFaceViewModel` / `LoginWithPalmprintViewModel`**: capture a frame → encode it → `IAuthenticator.LoginWithFace` / `LoginWithPalmprint` → on a match perform a real login (set `CurrentAccount`, record attendance) and navigate to the Dashboard.
- **`FaceRecognitionViewModel` / `PalmprintRecognitionViewModel`** and their views: gallery pages showing all of the signed-in student's enrolled images in a 4-across grid.
- **`Controls/FaceGuideOverlay`**: a transparent oval guide drawn over the camera feed so the user frames their face at a consistent distance.
- **`Controls/PalmGuideOverlay`**: the palm equivalent — a guide box the user aligns the open right palm inside. This box *is* the ROI the palmprint service crops.
  Both overlays are shared by the instruction, enrolment and login screens, so the guidance a user is shown is literally the guide they will see.

> Requires the `face_service` and `palmprint_service` Python services (repo root). Both are auto-started by the app; see their READMEs for one-time setup.

## Registration and login flows

**Registration** — six steps, `Register01` … `Register06`:

| Step | View | What it does |
|---|---|---|
| 1 | `Register01View` | Terms |
| 2 | `Register02WithInformationView` | Personal info. Binds to the shared singleton `RegistrationStore`; every required field shows a red `*`, and **Next** (a `RelayCommand`) stays disabled until `RegistrationStore.IsPersonalInfoValid()` passes, re-checked live as the user types |
| 3 | `Register03InstructionsForPalmprintView` | Palm positioning guidance, with `PalmGuideOverlay` previewed alongside the numbered steps |
| 4 | `Register04WithPalmprintView` | Captures **7** palm photos, encodes each, and buffers them in `RegistrationStore` |
| 5 | `Register05InstructionsForFaceView` | Face positioning guidance, same layout with `FaceGuideOverlay` |
| 6 | `Register06WithFaceView` | Captures **7** face photos, then on **Ολοκλήρωση** finishes the enrolment |

`Register06WithFaceViewModel.FinishAsync` runs in three stages: create the account, save both
biometric sets, then log in and navigate. Palms are buffered from step 4 rather than saved there,
because the rows need a `StudentId` that only exists once the account does. **If saving the
biometrics fails the account is deleted again**, so no student is left half-enrolled and unable to
re-register with the same email.

**Login** — `LoginView` offers face or palmprint (and registration). There is no
username/password path; accounts are biometric-only.

**Dashboard / Statistics** — `DashboardViewModel` and `StatisticsViewModel` read an
`AttendanceStatistics` read-model from `IStatisticsService`; the dashboard card shows the weekly
attendance percentage derived from it. The `Controls/*Card` and `WeeklyAttendanceGraph` controls
render the individual measures.

## Assets
All images and icons are bundled as WPF `Resource`s under `Assets/` and referenced by relative
pack paths — nothing is fetched from the network at runtime. Icon provenance and the attribution
obligations that come with them are recorded in the repo-root `THIRD-PARTY-NOTICES.md`.

## NuGet Packages
Target framework: **net10.0-windows** (`UseWPF`).

| Package | Version | Used for |
|---|---|---|
| `OpenCvSharp4` | 4.13.0.20260627 | Webcam capture & frame processing (`CameraCaptureService`) |
| `OpenCvSharp4.Windows` | 4.13.0.20260627 | Windows bundle incl. WPF extensions (frames → `BitmapSource`) |
| `OpenCvSharp4.runtime.win` | 4.13.0.20260627 | Native OpenCV runtime (Windows) |
| `AForge.Video.DirectShow` | 2.2.5 | Fast camera enumeration by name (DirectShow) — **LGPL v3**, see `THIRD-PARTY-NOTICES.md` |
| `System.Drawing.Common` | 10.0.11 | Image handling |

> Talks to the external Python services over HTTP via `HttpClient` — no NuGet package needed.

## Key Relationships
- **Depends on**: `AristotelisThesis.Domain` (for models/interfaces) and `AristotelisThesis.EntityFramework` (for the DB contexts).
- Integrates `OpenCvSharp` / `OpenCvSharp.WpfExtensions` (camera frames → WPF images) and `AForge.Video.DirectShow` (fast camera enumeration).
- Calls `face_service` on `127.0.0.1:8500` and `palmprint_service` on `127.0.0.1:8501`.
