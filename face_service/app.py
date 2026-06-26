"""
Face embedding microservice for the AristotelisThesis WPF app.

Stateless encoder: it takes an image and returns a 128-d face embedding produced
by dlib's ResNet-34 face recognition model (via the `face_recognition` library).
All matching/storage lives in the C# side - this service only encodes.

Run (dev):
    uvicorn app:app --host 127.0.0.1 --port 8500
"""
from io import BytesIO

import numpy as np
import face_recognition
from PIL import Image
from fastapi import FastAPI, Request
from fastapi.responses import JSONResponse

app = FastAPI(title="AristotelisThesis Face Service", version="1.0")


@app.get("/health")
def health():
    return {"status": "ok"}


@app.post("/encode")
async def encode(request: Request):
    """Decode the posted image and return the 128-d embedding of the largest face.

    Accepts the raw image bytes as the request body (any Pillow-readable format:
    JPEG/PNG/etc.). Returns:
        {"found": true,  "embedding": [128 floats]}  when a face is detected
        {"found": false}                              when no face is detected
    """
    body = await request.body()
    if not body:
        return JSONResponse({"found": False, "error": "empty body"}, status_code=400)

    try:
        image = Image.open(BytesIO(body)).convert("RGB")
    except Exception as ex:  # noqa: BLE001 - report any decode failure to the caller
        return JSONResponse({"found": False, "error": f"decode failed: {ex}"}, status_code=400)

    rgb = np.array(image)

    # Detect faces. If several are present, keep the largest box (closest subject).
    locations = face_recognition.face_locations(rgb)
    if not locations:
        return {"found": False}

    def area(box):
        top, right, bottom, left = box
        return (bottom - top) * (right - left)

    largest = max(locations, key=area)

    encodings = face_recognition.face_encodings(rgb, known_face_locations=[largest])
    if not encodings:
        return {"found": False}

    return {"found": True, "embedding": encodings[0].tolist()}
