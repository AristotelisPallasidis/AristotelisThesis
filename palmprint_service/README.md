# Palmprint Service (MediaPipe ROI + Gabor features)

A small FastAPI service that turns a hand image into a fixed-length **palmprint feature vector**.
The user aligns their open palm inside the on-screen guide box; the service crops that central
region and builds a texture descriptor from a **Gabor filter bank** (the classical palmprint
approach) using OpenCV.

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

The WPF app auto-starts this service on launch (see `PalmprintServiceLauncher`).

## Quick test

```powershell
curl http://127.0.0.1:8501/health
curl -X POST --data-binary "@some_palm.jpg" http://127.0.0.1:8501/encode-palm
```
