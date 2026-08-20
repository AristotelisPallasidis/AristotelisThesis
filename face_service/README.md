# Face Service (Python + ResNet-34)

A small FastAPI service that turns a face image into a **128-dimensional embedding**
using dlib's ResNet-34 face-recognition model (via the [`face_recognition`](https://github.com/ageitgey/face_recognition) library).

The WPF app calls this service to encode faces; **all matching and storage happen in C#**.
This service is stateless and has no database access. It is one of two encoder services —
its palmprint counterpart is `palmprint_service/`, which runs on port **8501**. This one runs
on port **8500**.

## Endpoints

| Method | Path      | Body                | Response |
|--------|-----------|---------------------|----------|
| GET    | `/health` | -                   | `{"status":"ok"}` |
| POST   | `/encode` | raw image bytes     | `{"found":true,"embedding":[...128 floats]}` or `{"found":false}` |

## Python packages
This is a Python project, so dependencies are pip packages (not NuGet). See `requirements.txt`.

| Package | Used for |
|---|---|
| `fastapi` | HTTP API framework |
| `uvicorn[standard]` | ASGI server that runs the app |
| `dlib-bin` | Prebuilt dlib (the ResNet-34 face engine) — avoids compiling from source |
| `face_recognition` | High-level wrapper over dlib (face detection + 128-d encodings); installed `--no-deps` |
| `face_recognition_models` | The dlib model files used by `face_recognition` |
| `pillow` | Image decoding |
| `numpy` | Array handling for image/embedding data |
| `python-multipart` | Lets FastAPI accept uploaded image bodies |
| `click` | CLI dependency of `face_recognition` |

## Setup (Windows)

`face_recognition` depends on **dlib**, a native C++ library. Building dlib from source needs
CMake + the Visual Studio C++ Build Tools, which often aren't installed. To avoid that we use the
**prebuilt `dlib-bin`** wheel.

Just run the setup script (creates the venv and installs everything):

```powershell
cd face_service
.\install.ps1
```

This is equivalent to:

```powershell
py -m venv venv
.\venv\Scripts\python.exe -m pip install --upgrade pip
.\venv\Scripts\python.exe -m pip install -r requirements.txt
# face_recognition declares a hard dependency on the source `dlib`, so install it
# without deps (we already have dlib-bin):
.\venv\Scripts\python.exe -m pip install face_recognition --no-deps
```

If you would rather build the real `dlib` from source, install **CMake** and the
**Visual Studio C++ Build Tools**, then `pip install dlib face_recognition`.

## Run

```powershell
venv\Scripts\Activate.ps1
uvicorn app:app --host 127.0.0.1 --port 8500
```

The WPF app auto-starts this service on launch (see `PythonServiceLauncher`), but you can run it
manually for development/testing.

## Matching

Matching is **not** done here. The 128-d embeddings are stored in SQL Server at enrolment, and
`AuthenticationService.LoginWithFace` compares a probe against the per-student average by
Euclidean (L2) distance, with a threshold of **`0.45`**. That is deliberately stricter than dlib's
default of `0.6`, which separates a person from a random stranger but is too loose for lookalikes
and siblings.

> **Licence note:** although `face_recognition_models` is CC0, the 68-point landmark predictor it
> ships was trained on the **iBUG 300-W** dataset, whose licence excludes commercial use. See
> `THIRD-PARTY-NOTICES.md` in the repo root.

## Quick test

```powershell
curl http://127.0.0.1:8500/health
curl -X POST --data-binary "@some_face.jpg" http://127.0.0.1:8500/encode
```
