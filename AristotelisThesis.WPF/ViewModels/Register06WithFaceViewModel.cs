using AristotelisThesis.Domain.Models;
using AristotelisThesis.Domain.Services;
using AristotelisThesis.WPF.Commands;
using AristotelisThesis.WPF.Services;
using AristotelisThesis.WPF.State;
using AristotelisThesis.WPF.State.Authenticators;
using AristotelisThesis.WPF.State.Navigators;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace AristotelisThesis.WPF.ViewModels
{
    /// <summary>
    /// Registration step 6: capture one or more face photos, encode each to a 128-d
    /// embedding, and on finish create the student account, persist the faces, log the
    /// student in, and navigate to the dashboard.
    /// </summary>
    public class Register06WithFaceViewModel : ViewModelBase
    {
        // Number of face photos the user must capture to complete enrollment.
        private const int RequiredFaceCount = 7;

        private readonly CameraCaptureService _camera = new CameraCaptureService();
        private readonly RegistrationStore _registration;
        private readonly IFaceRecognitionService _faceRecognition;
        private readonly IFaceImageService _faceImageService;
        private readonly IPalmprintImageService _palmImageService;
        private readonly IAccountService _accountService;
        private readonly IAuthenticator _authenticator;
        private readonly IRenavigator _dashboardRenavigator;

        public ICommand GoToViewRegister05InstructionsForFaceCommand { get; } // Back

        public event EventHandler<BitmapSource>? FrameReady;
        public ObservableCollection<string> Cameras => _camera.Cameras;

        private string _statusText = $"Χρειάζεστε {RequiredFaceCount} φωτογραφίες προσώπου (0/{RequiredFaceCount}).";
        public string StatusText
        {
            get => _statusText;
            private set { _statusText = value; OnPropertyChanged(nameof(StatusText)); }
        }

        public Register06WithFaceViewModel(
            RegistrationStore registration,
            IFaceRecognitionService faceRecognition,
            IFaceImageService faceImageService,
            IPalmprintImageService palmImageService,
            IAccountService accountService,
            IAuthenticator authenticator,
            IRenavigator register05Renavigator,
            IRenavigator dashboardRenavigator)
        {
            _registration = registration;
            _faceRecognition = faceRecognition;
            _faceImageService = faceImageService;
            _palmImageService = palmImageService;
            _accountService = accountService;
            _authenticator = authenticator;
            _dashboardRenavigator = dashboardRenavigator;

            GoToViewRegister05InstructionsForFaceCommand = new RenavigateCommand(register05Renavigator);

            _camera.FrameReady += (s, bmp) => FrameReady?.Invoke(this, bmp);
        }

        public Task RefreshCamerasAsync() => _camera.RefreshCamerasAsync();
        public void StartCamera(int deviceIndex) => _camera.StartCamera(deviceIndex);
        public void StopCamera() => _camera.StopCamera();

        /// <summary>
        /// Captures the current frame, encodes it, and (if a face is found) adds it to the
        /// set of faces to enroll. Returns a user-facing message.
        /// </summary>
        public async Task<string> CapturePhotoAsync()
        {
            if (_registration.CapturedFaces.Count >= RequiredFaceCount)
            {
                StatusText = $"Έχετε ήδη λάβει {RequiredFaceCount} φωτογραφίες. Πατήστε 'Ολοκλήρωση' ή 'Επανάληψη'.";
                return StatusText;
            }

            byte[]? jpeg = _camera.CaptureJpeg();
            if (jpeg == null)
            {
                StatusText = "Δεν υπάρχει διαθέσιμο καρέ από την κάμερα.";
                return StatusText;
            }

            float[]? embedding = await _faceRecognition.EncodeAsync(jpeg);
            if (embedding == null)
            {
                StatusText = "Δεν εντοπίστηκε πρόσωπο. Δοκιμάστε ξανά.";
                return StatusText;
            }

            _registration.CapturedFaces.Add((jpeg, embedding));
            int count = _registration.CapturedFaces.Count;
            StatusText = count >= RequiredFaceCount
                ? $"Ελήφθησαν {count}/{RequiredFaceCount}. Μπορείτε να ολοκληρώσετε."
                : $"Ελήφθησαν {count}/{RequiredFaceCount} φωτογραφίες.";
            return StatusText;
        }

        /// <summary>
        /// Clears all captured faces so the user can retake them.
        /// </summary>
        public void Repeat()
        {
            _registration.CapturedFaces.Clear();
            StatusText = $"Οι φωτογραφίες διαγράφηκαν. Λάβετε {RequiredFaceCount} ξανά (0/{RequiredFaceCount}).";
        }

        /// <summary>
        /// Creates the account from the collected info, stores the captured faces, logs the
        /// student in, and navigates to the dashboard. Returns success + a message.
        /// </summary>
        public async Task<FaceLoginResult> FinishAsync()
        {
            if (_registration.CapturedFaces.Count < RequiredFaceCount)
            {
                return new FaceLoginResult(false,
                    $"Πρέπει να λάβετε {RequiredFaceCount} φωτογραφίες προσώπου (έχετε {_registration.CapturedFaces.Count}).");
            }

            if (!TryBuildStudent(out Student student, out string error))
            {
                return new FaceLoginResult(false, error);
            }

            try
            {
                Account created = await _accountService.Create(new Account { AccountHolder = student });
                int studentId = created.AccountHolder.Id;

                foreach ((byte[] jpeg, float[] embedding) in _registration.CapturedFaces)
                {
                    await _faceImageService.SaveFaceImage(studentId, jpeg, embedding);
                }

                // Persist the palms captured earlier in the wizard (Register04).
                foreach ((byte[] jpeg, float[] embedding) in _registration.CapturedPalms)
                {
                    await _palmImageService.SavePalmprintImage(studentId, jpeg, embedding);
                }

                // Log the new student in via the face we just enrolled (matches itself).
                float[] firstEmbedding = _registration.CapturedFaces[0].Embedding;
                bool loggedIn = await _authenticator.LoginWithFace(firstEmbedding);

                StopCamera();
                _registration.Reset();

                if (!loggedIn)
                {
                    return new FaceLoginResult(false, "Ο λογαριασμός δημιουργήθηκε αλλά η αυτόματη σύνδεση απέτυχε.");
                }

                _dashboardRenavigator.Renavigate();
                return new FaceLoginResult(true, "Η εγγραφή ολοκληρώθηκε!");
            }
            catch (Exception ex)
            {
                return new FaceLoginResult(false, $"Σφάλμα κατά την εγγραφή: {ex.Message}");
            }
        }

        private bool TryBuildStudent(out Student student, out string error)
        {
            student = null!;
            error = "";

            if (!int.TryParse(_registration.Aem, out int aem))
            {
                error = "Μη έγκυρο ΑΕΜ.";
                return false;
            }
            if (!int.TryParse(_registration.Semester, out int semester))
            {
                error = "Μη έγκυρο εξάμηνο.";
                return false;
            }
            if (!int.TryParse(_registration.YearOfEntry, out int yearOfEntry))
            {
                error = "Μη έγκυρη χρονιά εισαγωγής.";
                return false;
            }
            if (!DateTime.TryParseExact(_registration.DateOfBirth, RegistrationStore.DateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dob))
            {
                error = "Μη έγκυρη ημερομηνία γέννησης (μορφή dd/MM/yyyy).";
                return false;
            }

            student = new Student
            {
                Name = _registration.Name,
                Surname = _registration.Surname,
                Sex = _registration.Sex,
                Phone = _registration.Phone,
                Address = _registration.Address,
                AcademicEmail = _registration.AcademicEmail,
                Department = _registration.Department,
                AEM = aem,
                Semester = semester,
                YearOfEntry = yearOfEntry,
                DateOfBirth = dob,
                IsPostgraduate = _registration.IsPostgraduate,
                // Biometric-only account: no typed credentials. Username keyed off the email.
                Username = _registration.AcademicEmail,
                PasswordHash = ""
            };
            return true;
        }
    }
}
