"""
Palmprint embedding microservice for the AristotelisThesis WPF app.

Stateless encoder: it takes a hand image, crops the central palm region (the user aligns
their open palm inside the on-screen guide box), and returns a fixed-length texture feature
vector built from a Gabor filter bank (the classical palmprint approach). All matching/storage
lives in the C# side - this service only encodes.

Run (dev):
    uvicorn app:app --host 127.0.0.1 --port 8501
"""
from io import BytesIO

import numpy as np
import cv2
from PIL import Image
from fastapi import FastAPI, Request
from fastapi.responses import JSONResponse

app = FastAPI(title="AristotelisThesis Palmprint Service", version="1.0")

# --- Palm ROI / feature settings -------------------------------------------------
ROI_FRACTION = 0.6        # central square crop = ROI_FRACTION * min(height, width)
ROI_SIZE = 128            # palm ROI is normalized to ROI_SIZE x ROI_SIZE
MIN_ROI_STD = 10.0        # reject near-uniform crops (no palm in frame)
GABOR_THETAS = 8          # orientations
GABOR_SIGMAS = (2.0, 4.0) # scales
GRID = 4                  # ROI split into GRID x GRID blocks, pooled per filter

# Precompute the Gabor kernel bank once.
_GABOR_KERNELS = []
for sigma in GABOR_SIGMAS:
    for t in range(GABOR_THETAS):
        theta = np.pi * t / GABOR_THETAS
        kernel = cv2.getGaborKernel((21, 21), sigma, theta, 10.0, 0.5, 0,
                                    ktype=cv2.CV_32F)
        _GABOR_KERNELS.append(kernel)


@app.get("/health")
def health():
    return {"status": "ok"}


def _extract_palm_roi(rgb: np.ndarray):
    """Crop the central square (where the guide box sits), normalize it, and return a grayscale
    ROI - or None if the crop is near-uniform (no palm in frame)."""
    h, w = rgb.shape[:2]
    side = int(min(h, w) * ROI_FRACTION)
    if side < 20:
        return None
    cx, cy = w // 2, h // 2
    half = side // 2
    crop = rgb[cy - half:cy + half, cx - half:cx + half]

    gray = cv2.cvtColor(crop, cv2.COLOR_RGB2GRAY)
    gray = cv2.resize(gray, (ROI_SIZE, ROI_SIZE))

    # Basic "is there a palm here" guard: a blank/empty frame has very low texture.
    if float(np.std(gray)) < MIN_ROI_STD:
        return None

    gray = cv2.createCLAHE(clipLimit=2.0, tileGridSize=(8, 8)).apply(gray)
    return gray


def _gabor_features(gray: np.ndarray) -> np.ndarray:
    """Build an L2-normalized feature vector from per-block Gabor energy."""
    g = gray.astype(np.float32) / 255.0
    block = ROI_SIZE // GRID
    feats = []
    for kernel in _GABOR_KERNELS:
        resp = cv2.filter2D(g, cv2.CV_32F, kernel)
        for by in range(GRID):
            for bx in range(GRID):
                cell = resp[by * block:(by + 1) * block, bx * block:(bx + 1) * block]
                feats.append(float(np.mean(np.abs(cell))))
    vec = np.asarray(feats, dtype=np.float32)
    norm = np.linalg.norm(vec)
    if norm > 0:
        vec = vec / norm
    return vec


@app.post("/encode-palm")
async def encode_palm(request: Request):
    """Decode the posted image and return the palmprint feature vector.

    Returns:
        {"found": true,  "embedding": [floats]}  when a palm crop is usable
        {"found": false}                          when the central crop is near-empty
    """
    body = await request.body()
    if not body:
        return JSONResponse({"found": False, "error": "empty body"}, status_code=400)

    try:
        image = Image.open(BytesIO(body)).convert("RGB")
    except Exception as ex:  # noqa: BLE001 - report any decode failure to the caller
        return JSONResponse({"found": False, "error": f"decode failed: {ex}"}, status_code=400)

    rgb = np.array(image)
    roi = _extract_palm_roi(rgb)
    if roi is None:
        return {"found": False}

    embedding = _gabor_features(roi)
    return {"found": True, "embedding": embedding.tolist()}
