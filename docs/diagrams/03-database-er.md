# Entity-Relationship Diagram

Two views of the same data model:
1. **Conceptual** — Elmasri / Navathe (Chen) notation.
2. **Relational** — the implemented tables (Crow's-Foot), showing foreign keys.

---

## 1. Conceptual ER (Elmasri / Navathe notation)

**Legend**
- **Rectangle** = entity type · **Diamond** = relationship type · **Oval** = attribute
- **Underlined** attribute = primary key · **Dashed** oval = derived attribute
- Edge labels = cardinality ratio (`1`, `N`); `total` = total participation (a double line in the
  textbook). Foreign keys are *not* shown as attributes — they are represented by relationships.

```mermaid
flowchart TB
    %% ===== Entity types =====
    STUDENT["STUDENT"]
    ACCOUNT["ACCOUNT"]
    FACEIMAGE["FACE_IMAGE"]
    SESSION["SESSION_HISTORY"]

    %% ===== Relationship types =====
    HOLDS{"HOLDS"}
    ENROLLS{"ENROLLS"}
    LOGS{"LOGS"}

    %% ===== STUDENT attributes =====
    st_id(("<u>Id</u>"))
    st_name(("Name"))
    st_sur(("Surname"))
    st_user(("Username"))
    st_pwd(("PasswordHash"))
    st_sex(("Sex"))
    st_dob(("DateOfBirth"))
    st_phone(("Phone"))
    st_addr(("Address"))
    st_email(("AcademicEmail"))
    st_aem(("AEM"))
    st_dep(("Department"))
    st_sem(("Semester"))
    st_year(("YearOfEntry"))
    st_post(("IsPostgraduate"))
    st_id --- STUDENT
    st_name --- STUDENT
    st_sur --- STUDENT
    st_user --- STUDENT
    st_pwd --- STUDENT
    st_sex --- STUDENT
    st_dob --- STUDENT
    st_phone --- STUDENT
    st_addr --- STUDENT
    st_email --- STUDENT
    st_aem --- STUDENT
    st_dep --- STUDENT
    st_sem --- STUDENT
    st_year --- STUDENT
    st_post --- STUDENT

    %% ===== ACCOUNT attributes =====
    ac_id(("<u>Id</u>"))
    ac_id --- ACCOUNT

    %% ===== FACE_IMAGE attributes =====
    fi_id(("<u>Id</u>"))
    fi_img(("ImageData"))
    fi_emb(("Embedding"))
    fi_date(("DateCaptured"))
    fi_id --- FACEIMAGE
    fi_img --- FACEIMAGE
    fi_emb --- FACEIMAGE
    fi_date --- FACEIMAGE

    %% ===== SESSION_HISTORY attributes =====
    se_id(("<u>Id</u>"))
    se_date(("Date"))
    se_in(("CheckIn"))
    se_out(("CheckOut"))
    se_active(("ActiveTime"))
    se_id --- SESSION
    se_date --- SESSION
    se_in --- SESSION
    se_out --- SESSION
    se_active --- SESSION

    %% ===== Relationships with cardinality + participation =====
    STUDENT ---|"1"| HOLDS
    HOLDS ---|"1 · total"| ACCOUNT
    STUDENT ---|"1"| ENROLLS
    ENROLLS ---|"N · total"| FACEIMAGE
    STUDENT ---|"1"| LOGS
    LOGS ---|"N · total"| SESSION

    %% derived attribute (computed from check-ins)
    classDef derived stroke-dasharray:5 4;
    class se_active derived;
```

> `ActiveTime` is **derived** (accumulated from the day's check-in/check-out), hence the dashed oval.
> `ACCOUNT` participates **totally** in `HOLDS` (every account has a holder); `STUDENT` participates
> partially. `FACE_IMAGE` and `SESSION_HISTORY` participate totally in their relationships.

---

## 2. Relational schema (implemented tables, Crow's-Foot)

This is what EF Core actually creates — foreign keys are explicit columns here. Deleting a
`Student` cascades to its `FaceImage` and `SessionHistory` rows.

```mermaid
erDiagram
    STUDENT ||--o| ACCOUNT : "is holder of"
    STUDENT ||--o{ FACEIMAGE : "enrolls"
    STUDENT ||--o{ SESSIONHISTORY : "logs"

    STUDENT {
        int Id PK
        string Name
        string Surname
        string Username
        string PasswordHash
        string Sex
        datetime DateOfBirth
        string Phone
        string Address
        string AcademicEmail
        int AEM
        string Department
        int Semester
        int YearOfEntry
        bool IsPostgraduate
    }
    ACCOUNT {
        int Id PK
        int AccountHolderId FK
    }
    FACEIMAGE {
        int Id PK
        int StudentId FK
        varbinary ImageData
        varbinary Embedding "128-d, nullable"
        datetime DateCaptured
    }
    SESSIONHISTORY {
        int Id PK
        int StudentId FK
        datetime Date
        timespan ActiveTime
        datetime CheckIn "nullable"
        datetime CheckOut "nullable"
    }
```
