# Third-Party Notices

This application bundles or depends on third-party material. The list below is a
good-faith record compiled from the project files and the asset filenames.

**Before submitting or publishing:** confirm each licence against the version you
actually ship. Entries marked **[verify]** could not be established from the
repository alone and need you to fill in where the file came from. This file is a
record, not legal advice.

---

## Icons

### icons8 — bundled

Downloaded from `img.icons8.com` (ios-filled, 50px, white) and committed to the
repository. They were previously fetched from the icons8 CDN at runtime.

| File | Used for |
| --- | --- |
| `Assets/Icons/graduation-cap.png` | Department row, profile page |
| `Assets/Icons/calendar.png` | Semester row, profile page |
| `Assets/Icons/time-machine.png` | Year-of-entry row, profile page |
| `Assets/Icons/new-post.png` | Email row, profile page |

Icons by [Icons8](https://icons8.com). Their free tier asks for a visible credit;
paid plans remove that requirement. Check <https://icons8.com/license> for the
current terms — **[verify]**.

### Font Awesome Free — filenames match exactly

These filenames are exact Font Awesome Free icon names, so they almost certainly
come from that set. Font Awesome Free icons are CC BY 4.0, which also asks for
attribution. Confirm before relying on it — **[verify]**.

- `Assets/Icons/arrow-right-from-bracket-solid.png`
- `Assets/Icons/chevron-right-solid.png`
- `Assets/Icons/fingerprint-solid.png`
- `Assets/Icons/weekly-streak-icons/circle-check-icon.png`
- `Assets/Icons/weekly-streak-icons/circle-notch-icon.png`
- `Assets/Icons/weekly-streak-icons/circle-xmark-icon.png`

Font Awesome Free — <https://fontawesome.com>, icons licensed CC BY 4.0.

### Provenance unknown — **[verify]**

These use a naming convention of their own, so their source cannot be identified
from the repository. Record where each came from, or replace them with icons whose
licence you can point to.

- `Assets/Icons/ChartIconSolid.png`
- `Assets/Icons/FaceIconSolid.png`
- `Assets/Icons/GearIconSolid.png`
- `Assets/Icons/HandIconSolid.png`
- `Assets/Icons/HouseIconSolid.png`
- `Assets/Icons/UserIconSolid.png`
- `Assets/Icons/Trash-Icon.png`
- `Assets/Icons/facial-recognition-icon.png`

### Institutional and own work

- `Assets/DUTH-Logo.jpg`, `Assets/DUTH-Logo.png`, `Assets/duth-logo-white.png`,
  `Assets/ico/DUTH-Logo.ico` — Democritus University of Thrace marks, used in a
  thesis project for that institution.
- `Assets/01.jpeg`, `Assets/aristotelis01_palm_sqrt.jpg` — photographs by the
  author.

---

## Fonts

The UI asks for **Manrope** by `FontFamily`, but no font file ships with the
application, so nothing is redistributed. On a machine without Manrope installed,
WPF silently falls back to a default face.

---

## .NET packages

| Package | Version | Licence |
| --- | --- | --- |
| Microsoft.EntityFrameworkCore | 10.0.9 | MIT |
| Microsoft.EntityFrameworkCore.SqlServer | 10.0.9 | MIT |
| Microsoft.EntityFrameworkCore.Design | 10.0.9 | MIT |
| Microsoft.EntityFrameworkCore.Tools | 10.0.9 | MIT |
| System.Drawing.Common | 10.0.9 | MIT |
| OpenCvSharp4 (+ `.Windows`, `.runtime.win`, `.WpfExtensions`) | 4.13.0.20260602 | Apache-2.0 |
| Microsoft.AspNet.Identity.Core | 2.2.4 | **[verify]** |
| AForge.Video.DirectShow | 2.2.5 | LGPL-3.0 — see below **[verify]** |
| LoadingSpinner.WPF | 1.0.0 | **[verify]** — appears unused, see below |

**AForge.Video.DirectShow** is the one worth attention. The AForge.NET libraries
are published under the LGPL, which attaches real conditions to redistribution
(notably that users can replace the library). It is referenced from
`Services/CameraCaptureService.cs` for DirectShow device enumeration. Confirm the
exact licence for version 2.2.5 and satisfy its terms, or move device enumeration
onto OpenCvSharp, which the project already uses for capture.

**LoadingSpinner.WPF** is declared in `AristotelisThesis.WPF.csproj` but no code or
XAML references it. Removing the package would drop the dependency and its licence
question entirely.

---

## Python services

Both services install from `requirements.txt` at setup time; these packages are not
committed to this repository, but they ship alongside the application if you
distribute the virtual environments.

### `face_service`

| Package | Licence |
| --- | --- |
| fastapi | MIT |
| uvicorn[standard] | BSD-3-Clause |
| pillow | HPND (MIT-CMU) |
| numpy | BSD-3-Clause |
| python-multipart | Apache-2.0 |
| click | BSD-3-Clause |
| dlib-bin | Boost Software License 1.0 |
| face_recognition | MIT |
| face_recognition_models | **[verify]** — see below |

**Pretrained models.** `face_recognition` relies on dlib's pretrained face models.
The 68-point facial landmark predictor in particular is trained on the iBUG 300-W
dataset, whose terms restrict use to non-commercial purposes. Academic thesis work
sits comfortably inside that, but it is worth stating explicitly if the work is
ever reused commercially. Confirm which models your installation pulls in.

### `palmprint_service`

| Package | Licence |
| --- | --- |
| fastapi | MIT |
| uvicorn[standard] | BSD-3-Clause |
| opencv-python | Apache-2.0 |
| numpy | BSD-3-Clause |
| pillow | HPND (MIT-CMU) |
| python-multipart | Apache-2.0 |

The palmprint encoder uses Gabor filter banks computed with OpenCV at runtime; no
pretrained model files are redistributed.
