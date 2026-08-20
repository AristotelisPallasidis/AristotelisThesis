# «ΜΕΛΕΤΗ, ΣΧΕΔΙΑΣΗ ΚΑΙ ΑΝΑΠΤΥΞΗ ΕΦΑΡΜΟΓΗΣ ΛΟΓΙΣΜΙΚΟΥ ΓΙΑ ΤΗΝ ΤΑΥΤΟΠΟΙΗΣΗ ΑΤΟΜΩΝ ΜΕ ΤΗ ΒΟΗΘΕΙΑ ΤΗΣ ΠΑΛΑΜΗΣ ΚΑΙ ΤΟΥ ΠΡΟΣΩΠΟΥ ΤΟΥΣ»

by Aristotelis Pallasidis

## Περίληψη

Η πτυχιακή αυτή εργασία αφορά:
1) Στην έρευνα βιβλιογραφική και μέσω του διαδικτύου
α) για τα χαρακτηριστικά που μπορούν να καταγραφούν από μία παλάμη ώστε αυτά
να χρησιμοποιηθούν ως βιομετρικά μοναδικά χαρακτηριστικά για την έγκυρη
καταγραφή ενός ατόμου
β) τις μεθοδολογίες και τους αλγόριθμους που χρησιμοποιούνται για την ανίχνευση
των χαρακτηριστικών από μία παλάμη
γ) τη μορφή των συσκευών και διατάξεων που χρησιμοποιούνται για την ανίχνευση
και καταγραφή χαρακτηριστικών από μία παλάμη καθώς και προτεινόμενες λύσεις.
δ) τις μεθοδολογίες και τους αλγόριθμους που χρησιμοποιούνται για την αναγνώριση
προσώπου
ε) τη βιβλιοθήκη OpenCV και πως αυτή μπορεί να χρησιμοποιηθεί για τη λήψη
εικόνων από παλάμες ατόμων και των προσώπων τους και την ανίχνευση μέσα από
αυτές των επιθυμητών χαρακτηριστικών.
2) Τη σχεδίαση ή / και την κατασκευή ενός πρότυπου συστήματος ανίχνευσης των
χαρακτηριστικών από την παλάμη και το πρόσωπο ατόμων με τη βοήθεια ενός συστήματος
όρασης.
3) Τη μελέτη σχεδίαση και ανάπτυξη μιας εφαρμογής λογισμικού που θα δημιουργεί μία βάση
δεδομένων με τις φωτογραφίες και τα χαρακτηριστικά από την παλάμη, το πρόσωπο και τα
προσωπικά στοιχεία κάθε ατόμου. Στη συνέχεια καθώς θα τίθεται η παλάμη στο πρότυπο
σύστημα ανίχνευσης, θα ελέγχεται η ταυτοποίηση του ατόμου με τη βοήθεια των εικόνων που

θα λαμβάνονται από την παλάμη του και το πρόσωπό του. Εφόσον γίνει ταυτοποίηση θα
γίνεται καταγραφή της ημερομηνίας για το συγκεκριμένο άτομο. Από τις διαφορετικές
ημερομηνίες που θα καταγράφονται θα μπορούν να εξαχθούν συμπεράσματα (στατιστικά
στοιχεία) για το χρόνο προσέλευσης και αποχώρησης κ.τ.λ.
Η γλώσσα προγραμματισμού μπορεί να είναι η C++/C#, Java ή Python. Σκοπός της
πτυχιακής αυτής εργασίας είναι η απόκτηση γνώσεων και δεξιοτήτων σε θέματα που
αφορούν στην ανάπτυξη μιας ολοκληρωμένης εφαρμογής λογισμικού (ακολουθώντας τις
αρχές της Τεχνολογίας Λογισμικού) και η απόκτηση γνώσεων και εμπειρίας σε θέματα που
αφορούν την αναγνώριση και ταυτοποίηση ατόμων με τη βοήθεια των μοναδικών
χαρακτηριστικών της παλάμης των και του προσώπου τους.

## Σχετική Βιβλιογραφία

[1] Εργασίες και υλικό που θα δοθεί από τον Επιβλέποντα Καθηγητή.

---

# Implementation & Setup

A Windows desktop application (.NET 10 / WPF) plus two small Python encoder services — one for
faces, one for palmprints. Both biometric modalities are implemented end to end: enrolment,
login, and attendance tracking.

## Architecture

| Project | Role |
|---|---|
| `AristotelisThesis.Domain` | Models, service interfaces, biometric matching logic (no DB/UI dependencies) |
| `AristotelisThesis.EntityFramework` | EF Core data access (SQL Server LocalDB) + migrations |
| `AristotelisThesis.WPF` | WPF/MVVM front-end, camera capture, Python bridge |
| `face_service/` | Python FastAPI service that turns a face image into a 128-d embedding (dlib ResNet-34 via `face_recognition`) — port **8500** |
| `palmprint_service/` | Python FastAPI service that turns a palm image into a Gabor texture feature vector (OpenCV) — port **8501** |

**Design.** Both Python services are *stateless encoders*: an image goes in, a feature vector
comes out. They have no database access and hold no state between requests. Feature vectors are
stored in SQL Server at enrolment, and **all matching happens in C#** by Euclidean (L2) distance
against the per-student average:

| Modality | Endpoint | Vector | Threshold |
|---|---|---|---|
| Face | `POST /encode` | 128-d (dlib ResNet-34) | `0.45` (stricter than dlib's default `0.6`) |
| Palmprint | `POST /encode-palm` | Gabor bank, L2-normalised | `0.6` |

Camera capture stays in C# (OpenCvSharp); only the captured JPEG crosses to Python.

Architecture, ER, class and sequence diagrams live in `docs/diagrams/` — see its README.

## Prerequisites
- .NET 10 SDK, Windows, SQL Server LocalDB
- Python 3.11+ (the app launches both services via the `py` launcher or a `python` on PATH)

## Setup

> Setting this up on a machine that has none of the above? Follow
> [`docs/setup.md`](docs/setup.md) instead — the full clean-machine guide, with prerequisites,
> verification steps and troubleshooting. The steps below are the short version for a developer
> whose environment is already in place.

1. **Python services** (one time each):
   ```powershell
   cd face_service
   .\install.ps1
   cd ..\palmprint_service
   .\install.ps1
   ```
   The face installer uses the prebuilt `dlib-bin` wheel, so no CMake or VS build tools are
   needed. See each service's README for details.
2. **Database**:
   ```powershell
   dotnet ef database update -p AristotelisThesis.EntityFramework -s AristotelisThesis.EntityFramework
   ```
   The connection string is read from `AristotelisThesis.WPF/appsettings.json`
   (`ConnectionStrings:DefaultConnection`); if that file is missing or unreadable the app falls
   back to `(localdb)\MSSQLLocalDB`, database `AristotelisThesisDB`.
3. **Run the app**: build/run `AristotelisThesis.WPF`. It auto-starts **both** Python services on
   launch (the first launch is slower while the dlib model loads) and stops them on exit. If a
   service can't start, the app still runs — that modality just fails to encode.

## Using it

**Register** — a six-step wizard:

| Step | Screen | What happens |
|---|---|---|
| 1 | Terms | Accept to continue |
| 2 | Personal info | Every required field is marked with a red `*`; **Next** stays disabled until all of them are valid |
| 3 | Palm instructions | How to position the hand, with the on-screen guide previewed |
| 4 | Palm capture | Align the right palm in the guide box and take **7** photos |
| 5 | Face instructions | How to frame the face |
| 6 | Face capture | Frame your face in the oval and take **7** photos |

**Ολοκλήρωση** then creates the account, stores the 7 palms and 7 faces with their feature
vectors, logs you in with the face just enrolled, and opens the dashboard. If storing the
biometrics fails, the account is rolled back so no half-enrolled student is left behind.

**Login** — choose face or palmprint, look at / present to the camera, capture. A match logs you
in and records an attendance check-in. There is no username/password path: accounts are
biometric-only.

**Dashboard & statistics** — attendance is derived from `SessionHistory` rows at read time
(days attended this week, streak, monthly percentage, check-in time, weekly graph). Nothing about
the statistics is written back to the database.

## Dependencies

**NuGet** (per project — see each project's README for the full table):
- **Domain**: `OpenCvSharp4` (+ `runtime.win`, `WpfExtensions`) 4.13.0.20260627, `System.Drawing.Common` 10.0.11
- **EntityFramework**: `Microsoft.EntityFrameworkCore` (+ `SqlServer`, `Design`, `Tools`) 10.0.11, `OpenCvSharp4` (+ `runtime.win`, `WpfExtensions`) 4.13.0.20260627, `System.Drawing.Common` 10.0.11
- **WPF**: `OpenCvSharp4` (+ `Windows`, `runtime.win`) 4.13.0.20260627, `AForge.Video.DirectShow` 2.2.5, `System.Drawing.Common` 10.0.11

**Python** — `face_service/requirements.txt`: `fastapi`, `uvicorn[standard]`, `pillow`, `numpy`,
`python-multipart`, `click`, `dlib-bin` (prebuilt dlib), `face_recognition_models`, plus
`face_recognition` (installed `--no-deps`). `palmprint_service/requirements.txt`: `fastapi`,
`uvicorn[standard]`, `opencv-python`, `numpy`, `pillow`, `python-multipart`.

Third-party licences and attribution obligations are recorded in
[`THIRD-PARTY-NOTICES.md`](THIRD-PARTY-NOTICES.md).

## Notes & limitations
- **The palmprint threshold (`0.6`) is a starting value, not a calibrated one.** It needs tuning
  against real captures before any FAR/FRR claim is made about the palmprint modality.
- Face recognition can't reliably separate very similar faces (siblings/twins). The threshold
  (`0.45` — deliberately stricter than dlib's default `0.6`, which separates a person from a random
  stranger but is too loose for lookalikes) and consistent lighting and framing are the main
  accuracy levers.
- Palm captures depend heavily on lighting; the enrolment instructions ask for the capture rig's
  internal illumination so that enrolment and login conditions match.
- The dlib landmark model is trained on **iBUG 300-W**, whose licence excludes commercial use.
  Academic thesis work is inside those terms — see `THIRD-PARTY-NOTICES.md`.
