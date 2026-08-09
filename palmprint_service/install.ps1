# Sets up the palmprint_service Python environment on Windows.

$ErrorActionPreference = "Stop"
Set-Location $PSScriptRoot

if (-not (Test-Path "venv")) {
    py -m venv venv
}

$python = ".\venv\Scripts\python.exe"

& $python -m pip install --upgrade pip
& $python -m pip install -r requirements.txt

& $python -c "import cv2, numpy; print('palmprint_service ready; opencv', cv2.__version__)"
