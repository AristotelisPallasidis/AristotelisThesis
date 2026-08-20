# Palmprint Service (Guide-box ROI + Gabor features)

A small FastAPI service that turns a hand image into a fixed-length **palmprint feature vector**.
The user aligns their open palm inside the on-screen guide box; the service crops that central
region and builds a texture descriptor from a **Gabor filter bank** (the classical palmprint
approach) using OpenCV.

No hand-landmark model is used — the ROI is fixed by the on-screen guide. See the ROI note below
for the adaptive alternative.

The WPF app calls this service to encode palms; **all matching and storage happen in C#**.
This service is stateless and has no database access. It is the palmprint counterpart of
`face_service/` and runs on a **different port (8501)**.

## Endpoints

| Method | Path           | Body            | Response |
|--------|----------------|-----------------|----------|
| GET    | `/health`      | -               | `{"status":"ok"}` |
| POST   | `/encode-palm` | raw image bytes | `{"found":true,"embedding":[...floats]}` or `{"found":false}` |

The embedding is a fixed-length L2-normalized float vector (orientations × scales × grid blocks),
matched in C# by Euclidean (L2) distance.

## Python packages

See `requirements.txt`. All of these ship prebuilt Windows wheels, so no compiler is needed.

| Package | Used for |
|---|---|
| `fastapi` | HTTP API framework |
| `uvicorn[standard]` | ASGI server that runs the app |
| `opencv-python` | Gabor filter bank, ROI crop, image ops |
| `numpy` | Array handling for image/feature data |
| `pillow` | Image decoding |
| `python-multipart` | Lets FastAPI accept uploaded image bodies |

## Matching

Matching is **not** done here. Feature vectors are stored in SQL Server at enrolment, and
`AuthenticationService.LoginWithPalmprint` compares a probe against the per-student average by
Euclidean (L2) distance.

> **The threshold is `0.6` and is a starting value, not a calibrated one.** Unlike the face
> threshold (`0.45`, chosen as a deliberately stricter alternative to dlib's default `0.6`), it has
> not been tuned against real captures. Calibrate it before making any FAR/FRR claim about the
> palmprint modality.

Enrolment captures **7** palms of the right hand (`Register04`). Because the ROI is the fixed
guide box, results depend on the palm being presented the same way at enrolment and at login —
which is why the enrolment instructions ask for the capture rig and its internal illumination.

## Setup (Windows)

```powershell
cd palmprint_service
.\install.ps1
```

This creates a venv and installs the deps (`opencv-python`, FastAPI, …).

> **ROI note:** the palm ROI is the central guide box (the user aligns their palm inside the
> on-screen rectangle). A more adaptive option is automatic hand localization (e.g. MediaPipe
> Hands or its Tasks `HandLandmarker`); swap the crop in `_extract_palm_roi` if you add it.

## Run

```powershell
venv\Scripts\Activate.ps1
uvicorn app:app --host 127.0.0.1 --port 8501
```

The WPF app auto-starts this service on launch (see `PythonServiceLauncher`, which the app
instantiates once per service).

## Quick test

```powershell
curl http://127.0.0.1:8501/health
curl -X POST --data-binary "@some_palm.jpg" http://127.0.0.1:8501/encode-palm
```
