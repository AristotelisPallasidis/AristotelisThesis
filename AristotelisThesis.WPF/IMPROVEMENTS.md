# WPF Application Improvements

The `AristotelisThesis.WPF` project acts as the presentation layer. Because it contains heavy biometric rendering processing, strict architectural adherence is required to prevent UI blocking.

## 1. Eliminate Hardware & OpenCV Logic from ViewModels
**Current State:** In files like `LoginWithFaceViewModel.cs`, logic involving camera detection (`VideoCapture`), facial cascading (`CascadeClassifier`), and resizing matrices is written directly inside the UI-binding ViewModel.
**Improvement:** Extract this logic into an injected Service layer (e.g., `ICameraService` and `IFaceRecognitionService`). ViewModels should only call high-level service functions (`var result = await _faceRecognitionService.IdentifyFaceAsync(imageBytes);`) and bind the results to the UI. This allows you to test your ViewModels easily without requiring a physical camera.

## 2. Remove Direct Database Access
**Current State:** ViewModels are spinning up instances of database contexts locally (e.g., `using var db = new AristotelisThesisDbContextFactory().CreateDbContext()`).
**Improvement:** The Presentation Layer should never talk directly to the inner Entity Framework logic. Request your data through previously established `DataServices` injected via constructors. This avoids tight coupling and simplifies unit testing.

## 3. App.xaml.cs Refactoring
**Current State:** The Dependency Injection in `App.xaml.cs` has dozens of lines manually registering nav views, ViewModels, and navigation delegates. There is also a duplicated registration for `Register06WithFaceViewModel`.
**Improvement:** 
- Eliminate the duplicate DI registration for `Register06WithFaceViewModel`.
- Refactor the DI container into Extension Methods (e.g., creating static classes with `public static void AddViewModels(this IServiceCollection services)`). This will make the startup sequence much cleaner.

## 4. Modernize Throttles (Thread.Sleep)
**Current State:** Inside tasks capturing images, `Thread.Sleep(30)` is used to throttle camera frame rates.
**Improvement:** Using `Thread.Sleep` blocks threads from returning to the thread pool for other operations. Swap these out for `await Task.Delay(30)` inside an async layout.

## 5. Centralized Configuration
**Current State:** Magic strings, hardcoded paths (`haarcascade_frontalface_default.xml`), and thresholds (`60.0`) are scattered throughout specific classes.
**Improvement:** Push these variables to the `appsettings.json` file. This lets you tweak camera confidence thresholds globally without needing to recompile the source code.
