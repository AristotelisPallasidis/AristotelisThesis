# Face Login Sequence

What happens when a returning user logs in with their face. Encoding is done in Python;
matching is done in C# against the stored embeddings.

```mermaid
sequenceDiagram
    actor User
    participant View as LoginWithFace View
    participant VM as LoginWithFaceViewModel
    participant Cam as CameraCaptureService
    participant Py as Python /encode
    participant Auth as IAuthenticator
    participant Svc as AuthenticationService
    participant Faces as IFaceImageService
    participant DB as Database

    User->>View: Select camera, "Take photo"
    View->>VM: RecognizeAndLoginAsync()
    VM->>Cam: CaptureJpeg()
    Cam-->>VM: JPEG bytes
    VM->>Py: POST /encode (JPEG)
    Py-->>VM: { found, embedding[128] }

    alt no face found
        Py-->>VM: { found: false }
        VM-->>View: "No face detected"
    else face encoded
        VM->>Auth: LoginWithFace(embedding)
        Auth->>Svc: LoginWithFace(embedding)
        Svc->>Faces: GetAllEmbeddings()
        Faces->>DB: SELECT StudentId, Embedding
        DB-->>Faces: enrolled embeddings
        Faces-->>Svc: list
        Svc->>Svc: avg L2 distance per student;<br/>best under 0.45 ?
        alt match found
            Svc-->>Auth: Account
            Auth->>Auth: set CurrentAccount, record check-in
            Auth-->>VM: true
            VM-->>View: navigate to Dashboard
        else no match
            Svc-->>Auth: null
            Auth-->>VM: false
            VM-->>View: "No match"
        end
    end
```
