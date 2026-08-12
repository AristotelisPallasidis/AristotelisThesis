# Diagrams

Diagrams documenting the architecture and main flows of the biometric identification
system. Useful for the thesis report and the defense presentation.

## How to view
- **VS Code**: install the *Draw.io Integration* extension and open `AristotelisThesis.drawio`.
- **Browser**: open the file in <https://app.diagrams.net>.
- **Export to image** (for slides): *File ▸ Export as ▸ PNG/SVG…* — export the current page only.

## draw.io / diagrams.net
- **`AristotelisThesis.drawio`** — open in <https://app.diagrams.net> (or the VS Code *Draw.io Integration*
  extension). It's a **multi-page** file (tabs at the bottom) with editable native shapes for **all 9**
  diagrams:
  1. System Architecture (WPF · Domain · EF Core · **two** Python services)
  2. Runtime / Deployment (face_service 8500 + palmprint_service 8501)
  3. ER (Relational, Crow's-Foot — incl. `PALMPRINT_IMAGE` and the derived statistics region)
  4. Domain Class Diagram (face + palmprint models, statistics read-model, service interfaces)
  5. Registration Flow (7 palms in Register04, 7 faces in Register06)
  6. ER (Elmasri / Navathe — entities, diamonds, ovals, underlined keys, derived attributes)
  7. Biometric Enrollment (Sequence — palms *and* faces through to auto-login)
  8. Face Login (Sequence)
  9. Palmprint Login (Sequence)
- The `.drawio` pages are the editable versions for slides/report figures, and are the single
  source of truth for the diagrams.
- **Tip:** to bring a Mermaid block into draw.io as native shapes, use
  **Arrange ▸ Insert ▸ Advanced ▸ Mermaid…**, paste the block, and click **Insert**.

### Persisted tables vs. derived read-model
Only five entities are actually stored: `STUDENT`, `ACCOUNT`, `FACE_IMAGE`, `PALMPRINT_IMAGE`
and `SESSION_HISTORY` — these are the `DbSet`s in `AristotelisThesisDbContext` and the only ones
with migrations.

`AttendanceStatistics` and `WeeklyAttendanceDataPoint` are **not** tables. `StatisticsService`
projects them from the student's `SESSION_HISTORY` rows every time the Statistics page is opened,
so nothing about them is written back. They appear in all three data diagrams, but always with a
**dashed** border (and dashed attribute ovals in the Chen diagram) to mark them as derived:
- page **3** — a separate "Derived read-model" block to the right of the real tables;
- page **4** — dashed classes plus `«interface» IStatisticsService`, which aggregates
  `SessionHistory` and creates the read-model;
- page **6** — a dashed `COMPUTED_FOR` / `HAS_POINT` region, where every measure is a derived
  attribute and `HAS_POINT` is fixed at 7 (Mon..Sun).

## Index

All diagrams live as pages inside `AristotelisThesis.drawio` (tabs at the bottom of the editor).

| Page | Diagram | Type | Shows |
|------|---------|------|-------|
| 1 | System Architecture | Component | The four parts (WPF, Domain, EntityFramework, two Python services) and how they depend on each other |
| 2 | Runtime / Deployment | Deployment | The processes that run at execution time (face_service 8500, palmprint_service 8501) and how they talk |
| 3 | ER (Relational) | Entity-Relationship | The relational schema in Crow's-Foot notation, plus the derived statistics read-model |
| 4 | Domain Class Diagram | Class | Core models, the statistics read-model, and the service interfaces |
| 5 | Registration Flow | Flowchart | The multi-step sign-up wizard (7 palms, then 7 faces) |
| 6 | ER (Elmasri / Navathe) | Entity-Relationship | Conceptual ER in **Chen** notation — entities, diamonds, ovals, derived attributes |
| 7 | Biometric Enrollment | Sequence | What happens when a new user records their palms and faces, through to auto-login |
| 8 | Face Login | Sequence | What happens when a user logs in with their face |
| 9 | Palmprint Login | Sequence | What happens when a user logs in with their palm |

## Suggested use in the presentation
1. Start with **#1 System Architecture** — the big picture (C# app + Python encoders + DB).
2. **#2 Runtime** — clarify that the app auto-starts both Python services.
3. **#3 / #6 ER** and **#4 Class** — the data and domain design (software-engineering rigor);
   good place to point out which entities are stored and which are computed.
4. **#5 Registration Flow** — the user journey.
5. **#7 Enrollment**, **#8 Face Login** and **#9 Palmprint Login** — the core biometric
   contribution: where the embedding is produced (Python) and where matching happens (C#).
