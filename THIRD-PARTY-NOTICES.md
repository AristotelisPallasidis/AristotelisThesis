# Third-Party Notices

This application bundles or depends on third-party material. Licences below were checked
against the publishers' own pages in August 2026; the sources are listed at the end. This
file is a record, not legal advice — reconfirm before publishing if time has passed.

One item still needs you: the provenance of eight icons, marked **[confirm]** under
*Icons of unconfirmed origin*. Everything else has been established.

---

## Attribution required

Two of the bundled sets ask for a visible credit. Neither is currently given anywhere in
the application or the repository beyond this file.

- **Icons8** — the free tier requires one visible link to <https://icons8.com> somewhere the
  audience can see it. One link is enough for all icons; for a desktop application the
  convention is an About or Settings screen.
- **Font Awesome Free** — the icons are CC BY 4.0, which requires attribution. Font Awesome
  normally considers the comment embedded in a downloaded file sufficient, but the icons
  here were exported to PNG through Inkscape, which strips those comments. An explicit
  credit is therefore needed.

The thesis document is a reasonable place for both if you would rather not add an About
screen.

---

## Icons

### Icons8 — bundled

Downloaded from `img.icons8.com` (ios-filled, 50px, white); previously fetched from the
Icons8 CDN at runtime.

| File | Used for |
| --- | --- |
| `Assets/Icons/graduation-cap.png` | Department row, profile page |
| `Assets/Icons/calendar.png` | Semester row, profile page |
| `Assets/Icons/time-machine.png` | Year-of-entry row, profile page |
| `Assets/Icons/new-post.png` | Email row, profile page |

Icons by [Icons8](https://icons8.com), free licence — see *Attribution required* above.

### Font Awesome Free

Filenames match Font Awesome Free icon names exactly, and each was exported from an SVG
through Inkscape (recorded in the PNG `Software` tag).

- `Assets/Icons/arrow-right-from-bracket-solid.png`
- `Assets/Icons/chevron-right-solid.png`
- `Assets/Icons/fingerprint-solid.png`
- `Assets/Icons/weekly-streak-icons/circle-check-icon.png`
- `Assets/Icons/weekly-streak-icons/circle-notch-icon.png`
- `Assets/Icons/weekly-streak-icons/circle-xmark-icon.png`

[Font Awesome Free](https://fontawesome.com) — icons CC BY 4.0, fonts SIL OFL 1.1, code MIT.
Only the icons are used here.

### Icons of unconfirmed origin — **[confirm]**

These use a naming convention of their own, so the source cannot be read off the filename.
They carry the **same Inkscape export signature** as the Font Awesome icons above, which
suggests they were produced from the same source in the same sitting — but that is an
inference, not a record. Confirm where they came from; if they are Font Awesome, fold them
into the section above.

- `Assets/Icons/ChartIconSolid.png`
- `Assets/Icons/FaceIconSolid.png`
- `Assets/Icons/GearIconSolid.png`
- `Assets/Icons/HandIconSolid.png`
- `Assets/Icons/HouseIconSolid.png`
- `Assets/Icons/UserIconSolid.png`
- `Assets/Icons/Trash-Icon.png`

`Assets/Icons/facial-recognition-icon.png` is separate: it carries ImageMagick metadata
rather than Inkscape's, so it came through a different toolchain and probably a different
source.

### Institutional and own work

- `Assets/DUTH-Logo.jpg`, `Assets/DUTH-Logo.png`, `Assets/duth-logo-white.png`,
  `Assets/ico/DUTH-Logo.ico` — Democritus University of Thrace marks, used in a thesis
  project for that institution.
- `Assets/01.jpeg`, `Assets/aristotelis01_palm_sqrt.jpg` — photographs by the author.

---

## Fonts

The UI requests **Manrope** by `FontFamily`, but no font file ships with the application, so
nothing is redistributed. Without Manrope installed, WPF falls back to a default face.

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
| AForge.Video.DirectShow | 2.2.5 | **LGPL v3** — see below |

**AForge.Video.DirectShow is the one dependency with real redistribution conditions.**
AForge.NET 2.x is published under LGPL v3 (only `AForge.Video.FFMPEG`, which this project
does not use, is GPL v3). LGPL requires among other things that a recipient can replace the
library — satisfiable by shipping it as a separate assembly, which is how NuGet delivers it,
but worth stating deliberately rather than by accident.

It is referenced from exactly one place: `Services/CameraCaptureService.cs`, for DirectShow
device enumeration. OpenCvSharp already performs the capture itself, so moving enumeration
across would remove this dependency and its obligation entirely.

---

## Python services

Installed from `requirements.txt` at setup time. Not committed here, but they ship alongside
the application if the virtual environments are distributed.

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
| face_recognition_models | Public domain / CC0 1.0 |

**The pretrained models carry a restriction the package licence does not.** Although
`face_recognition_models` is itself CC0, the 68-point facial landmark predictor it ships was
trained on the **iBUG 300-W** dataset, whose licence **excludes commercial use**; dlib's own
documentation directs commercial users to contact Imperial College London. Academic thesis
work is comfortably inside those terms, but the restriction follows the model, so it matters
if this work is ever reused commercially.

### `palmprint_service`

| Package | Licence |
| --- | --- |
| fastapi | MIT |
| uvicorn[standard] | BSD-3-Clause |
| opencv-python | Apache-2.0 |
| numpy | BSD-3-Clause |
| pillow | HPND (MIT-CMU) |
| python-multipart | Apache-2.0 |

The palmprint encoder computes Gabor filter banks with OpenCV at runtime; no pretrained
model files are redistributed.

---

## Sources

- Font Awesome Free licence — <https://github.com/FortAwesome/Font-Awesome/blob/6.x/LICENSE.txt>
- Icons8 licence and attribution — <https://icons8.com/license> and
  <https://intercom.help/icons8-7fb7577e8170/en/articles/4725508-where-do-i-add-the-attribution-link>
- AForge.NET licence — <https://www.aforgenet.com/framework/license/> and
  <https://github.com/andrewkirillov/AForge.NET/blob/master/License.txt>
- dlib landmark model / iBUG 300-W restriction — <https://dlib.net/face_landmark_detection.py.html>
- face_recognition_models — <https://github.com/ageitgey/face_recognition_models>
