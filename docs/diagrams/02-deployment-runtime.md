# Runtime / Deployment

The processes that exist at execution time and how they communicate. The WPF application
**auto-starts** the Python service on launch and stops it on exit.

```mermaid
flowchart LR
    User(["User"])

    subgraph Machine["Windows machine"]
        App["AristotelisThesis.WPF.exe<br/>(.NET 10 desktop app)"]
        PyProc["Python service process<br/>uvicorn app:app  @ 127.0.0.1:8500<br/>(dlib ResNet-34 model loaded once)"]
        DB[("SQL Server LocalDB<br/>AristotelisThesisDB")]
        Cam["Webcam"]
    end

    User -- interacts --> App
    Cam -- frames --> App
    App -- "spawns / kills (PythonServiceLauncher)" --> PyProc
    App -- "HTTP POST /encode (JPEG -> 128-d)" --> PyProc
    App -- "EF Core / SQL" --> DB
```

**Notes**
- First launch is slower while dlib loads its model; subsequent `/encode` calls are fast.
- If the Python service can't start, the app still runs — face login simply reports no match.
