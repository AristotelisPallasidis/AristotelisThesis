# AristotelisThesis Code Review & General Improvements

After an initial review of the codebase across the `Domain`, `EntityFramework`, and `WPF` projects, the following improvements have been identified to enhance the architecture, maintainability, and testing capabilities of the application.

## 1. Architecture & MVVM Pattern Violations
- **Direct Database Access in ViewModels:** In `LoginWithFaceViewModel.cs`, a direct instantiation of `AristotelisThesisDbContextFactory` is used to load `FaceImages` from the database database. ViewModels shouldn't access the database directly. 
  - *Improvement:* Inject a service (e.g., `IBiometricDataService`) to fetch this data instead.
- **Hardware/OpenCV Logic in ViewModels:** Code like video capture, resizing, formatting `Mat` frames, and `CascadeClassifier` are deeply integrated in the ViewModel.
  - *Improvement:* Extract the computer vision and camera streaming logic into an isolated service (e.g., `ICameraService` and `IFaceRecognitionService`). The ViewModel should subscribe to events provided by these services and update the UI accordingly.

## 2. Dependency Injection (DI) Maintenance
- **Duplicate Registrations:** In `AristotelisThesis.WPF\App.xaml.cs`, `Register06WithFaceViewModel` is registered twice consecutively. 
  - *Improvement:* Remove the redundant block around lines 203-210.
- **Huge App.xaml.cs File:** The `App.xaml.cs` file is quite bloated with every single ViewModel and Navigator registration.
  - *Improvement:* Introduce modular extension methods (e.g., `services.AddViewModels()`, `services.AddNavigation()`, `services.AddBiometricServices()`) to organize the dependency graph.

## 3. Domain Model Improvements
- **Handling Constraints & Passwords:** In `Student.cs`, things like `PasswordHash` and `Username` sit on the student domain alongside heavy personal properties. Additionally, validation like DataAnnotations is mixed in.
  - *Improvement:* Depending on complex rules, keep authentication concerns strictly isolated in an `Account` or `User` entity, referencing a `Student` record, to separate Identity properties from pure student demographics.

## 4. Performance & Multithreading
- **Thread.Sleep(...) Usage:** `Thread.Sleep(30)` is used inside `Task.Run` loops in biometric viewmodels to throttle the FPS of the camera.
  - *Improvement:* Instead of `Thread.Sleep()`, ideally use an asynchronous delay (`await Task.Delay(30)`) within an `async/await` pattern so as not to block any thread pool thread synchronization contexts unnecessarily.

## 5. Security & Configuration
- **Hardcoded Filepaths:** There is a hardcoded file path (`haarcascade_frontalface_default.xml`) for facial classifiers in ViewModels.
  - *Improvement:* Move paths and thresholds (like `RecognitionThreshold = 60.0`) to `appsettings.json` and inject them via the `IOptions<T>` configurations pattern. 
