# Registration Flow

The multi-step sign-up wizard. The palmprint steps are currently **skipped** (only face
recognition is implemented), so the info step goes straight to the face steps.

```mermaid
flowchart TD
    Login["Login screen"] --> R1["Register01<br/>Privacy policy / terms"]
    R1 --> R2["Register02<br/>Personal information form"]

    R2 --> Valid{"All required fields valid?<br/>(red * fields)"}
    Valid -- "No" --> R2note["Next button stays disabled<br/>(live validation as you type)"]
    R2note --> R2
    Valid -- "Yes" --> R5["Register05<br/>Face capture instructions"]

    R5 --> R6["Register06<br/>Capture 3-5 face photos<br/>(face inside the oval guide)"]
    R6 --> Encode["Each photo -> Python /encode -> 128-d embedding<br/>(stored in RegistrationStore)"]
    Encode --> Finish{"Click 'Ολοκλήρωση'"}

    Finish --> Create["Create Student + Account<br/>(biometric-only)"]
    Create --> Save["Save FaceImages (+ embeddings) to DB"]
    Save --> AutoLogin["Auto-login via the just-enrolled face<br/>(records attendance check-in)"]
    AutoLogin --> Dash["Dashboard"]

    %% palmprint steps exist but are bypassed
    R3["Register03/04<br/>Palmprint (placeholder - skipped)"]:::skipped
    classDef skipped fill:#eee,stroke:#999,stroke-dasharray:4 2,color:#777
```

**Notes**
- Personal-info fields are collected into a shared singleton `RegistrationStore`.
- The account is created **once**, at the end (Register06 *Finish*), so an abandoned wizard
  leaves no orphan record.
