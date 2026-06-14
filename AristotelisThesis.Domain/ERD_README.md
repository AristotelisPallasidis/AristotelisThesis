# AristotelisThesis.Domain - Entity Relationship Diagram (ERD)

This document visualizes the domain models and their relationships within the `AristotelisThesis.Domain` project.

## ERD Diagram

```mermaid
erDiagram
    Student {
        int Id PK
        string Name
        string Surname
        string Username
        string PasswordHash
        string Sex
        DateTime DateOfBirth
        string Phone
        string Address
        string AcademicEmail
        int AEM
        string Department
        int Semester
        int YearOfEntry
        bool IsPostgraduate
    }

    Account {
        int Id PK
        int AccountHolderId FK
    }

    FaceImage {
        int Id PK
        int StudentId FK
        byte[] ImageData
        DateTime DateCaptured
    }

    PalmprintImage {
        int Id PK
        int StudentId FK
        byte[] ImageData
        DateTime DateCaptured
    }

    SessionHistory {
        int Id PK
        int StudentId FK
        DateTime Date
        TimeSpan ActiveTime
    }

    AttendanceStatistics {
        int Id PK
        int StudentId FK
        double MonthlyAttendancePercentage
        TimeSpan TodayActiveTime
        int DaysAttendedThisWeek
        double MonthlyActiveHours
        int WeekLoginStreak
        TimeSpan WeeklyAverageActiveTime
        DateTime TodayCheckIn "nullable"
    }

    WeeklyAttendanceDataPoint {
        int Id PK
        int AttendanceStatisticsId FK
        DateTime Date
        TimeSpan ActiveTime
    }

    Student ||--o| Account : "AccountHolder"
    Student ||--o{ FaceImage : "has"
    Student ||--o{ PalmprintImage : "has"
    Student ||--o{ SessionHistory : "has"
    Student ||--o| AttendanceStatistics : "has"
    AttendanceStatistics ||--o{ WeeklyAttendanceDataPoint : "WeeklyAttendanceGraph"
```

## Notes
- All entities inherit from the base `DomainObject` class, which provides the `Id` primary key property.
- `Student` acts as the primary entity and has relationships with images, session history, and attendance.
- `Account` holds a reference to a `Student` (the `AccountHolder`).
- `AttendanceStatistics` tracks overall attendance metrics and has a one-to-many relationship with `WeeklyAttendanceDataPoint` to construct a graph/history of the week's data.
