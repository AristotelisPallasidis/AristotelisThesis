# Sets up the face_service Python environment on Windows.
# Uses the prebuilt dlib-bin wheel so no CMake / Visual C++ build tools are required.

$ErrorActionPreference = "Stop"
Set-Location $PSScriptRoot

# Create the virtualenv if it doesn't exist (uses the Windows "py" launcher).
if (-not (Test-Path "venv")) {
    py -m venv venv
}

$python = ".\venv\Scripts\python.exe"

& $python -m pip install --upgrade pip
& $python -m pip install -r requirements.txt

# face_recognition pulls in source `dlib` as a hard dependency; we already have
# dlib-bin, so install it without deps to avoid the failing compile.
& $python -m pip install face_recognition --no-deps

& $python -c "import dlib, face_recognition; print('face_service ready; dlib', dlib.__version__)"
