# Diagrams

Mermaid diagrams documenting the architecture and main flows of the biometric identification
system. Useful for the thesis report and the defense presentation.

## How to view
- **GitHub / GitLab**: render Mermaid automatically — just open the `.md` file.
- **VS Code**: install the *Markdown Preview Mermaid Support* extension, then open Preview.
- **Export to image** (for slides): paste a diagram into <https://mermaid.live> and export PNG/SVG.

## draw.io / diagrams.net
- **`AristotelisThesis.drawio`** — open in <https://app.diagrams.net> (or the VS Code *Draw.io Integration*
  extension). It's a **multi-page** file (tabs at the bottom) with editable native shapes for **all 8**
  diagrams:
  1. System Architecture
  2. Runtime / Deployment
  3. ER (Relational, Crow's-Foot)
  4. Domain Class Diagram
  5. Registration Flow
  6. ER (Elmasri / Navathe — entities, diamonds, ovals, underlined keys, derived attribute)
  7. Face Enrollment (Sequence)
  8. Face Login (Sequence)
- The Mermaid `.md` files above remain the source of truth for inline rendering; the `.drawio`
  pages are the editable versions for slides/report figures.
- **Tip:** to bring any Mermaid block into draw.io as native shapes, use
  **Arrange ▸ Insert ▸ Advanced ▸ Mermaid…**, paste the block, and click **Insert**.

## Index

| # | Diagram | Type | Shows |
|---|---------|------|-------|
| 1 | [System Architecture](01-system-architecture.md) | Component | The four parts (WPF, Domain, EntityFramework, Python service) and how they depend on each other |
| 2 | [Runtime / Deployment](02-deployment-runtime.md) | Deployment | The processes that run at execution time and how they talk |
| 3 | [Database (ER)](03-database-er.md) | Entity-Relationship | Conceptual ER in **Elmasri/Navathe (Chen)** notation + the relational schema (Crow's-Foot) |
| 4 | [Domain Class Diagram](04-domain-class-diagram.md) | Class | Core models + service interfaces |
| 5 | [Registration Flow](05-registration-flow.md) | Flowchart | The multi-step sign-up wizard (palmprint skipped) |
| 6 | [Face Enrollment Sequence](06-face-enrollment-sequence.md) | Sequence | What happens when a new user records their face |
| 7 | [Face Login Sequence](07-face-login-sequence.md) | Sequence | What happens when a user logs in with their face |

## Suggested use in the presentation
1. Start with **#1 System Architecture** — the big picture (C# app + Python encoder + DB).
2. **#2 Runtime** — clarify that the app auto-starts the Python service.
3. **#3 ER** and **#4 Class** — the data and domain design (software-engineering rigor).
4. **#5 Registration Flow** — the user journey.
5. **#6 Enrollment** and **#7 Login** sequences — the core biometric contribution: where the
   ResNet-34 embedding is produced (Python) and where matching happens (C#).
