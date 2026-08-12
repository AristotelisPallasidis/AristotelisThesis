using AristotelisThesis.Domain.Services;
using AristotelisThesis.WPF.Commands;
using AristotelisThesis.WPF.Services;
using AristotelisThesis.WPF.State;
using AristotelisThesis.WPF.State.Navigators;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace AristotelisThesis.WPF.ViewModels
{
    /// <summary>
    /// Registration step 4: capture palm photos, encode each to a feature vector, and stash them
    /// in the shared <see cref="RegistrationStore"/>. The account (and the actual save of palms +
    /// faces) happens at the end of the wizard in Register06.
    /// </summary>
    public class Register04WithPalmprintViewModel : ViewModelBase
    {
        // Number of palm photos the user must capture to advance.
        private const int RequiredPalmCount = 7;

        private readonly CameraCaptureService _camera = new CameraCaptureService();
        private readonly RegistrationStore _registration;
        private readonly IPalmprintRecognitionService _palmRecognition;
        private readonly IRenavigator _register05Renavigator;

        public ICommand GoToViewRegister03InstructionsForPalmprintCommand { get; } // Back
        public ICommand GoToViewRegister05InstructionsForFaceCommand { get; }      // Next (gated)

        public event EventHandler<BitmapSource>? FrameReady;
        public ObservableCollection<string> Cameras => _camera.Cameras;

        private string _statusText = $"Χρειάζεστε {RequiredPalmCount} φωτογραφίες της δεξιάς παλάμης (0/{RequiredPalmCount}).";
        public string StatusText
        {
            get => _statusText;
            private set { _statusText = value; OnPropertyChanged(nameof(StatusText)); }
        }

        public Register04WithPalmprintViewModel(
            RegistrationStore registration,
            IPalmprintRecognitionService palmRecognition,
            IRenavigator register03Renavigator,
            IRenavigator register05Renavigator)
        {
            _registration = registration;
            _palmRecognition = palmRecognition;
            _register05Renavigator = register05Renavigator;

            GoToViewRegister03InstructionsForPalmprintCommand = new RenavigateCommand(register03Renavigator);
            GoToViewRegister05InstructionsForFaceCommand = new RelayCommand(_ => GoNext());

            _camera.FrameReady += (s, bmp) => FrameReady?.Invoke(this, bmp);
        }

        public Task RefreshCamerasAsync() => _camera.RefreshCamerasAsync();
        public void StartCamera(int deviceIndex) => _camera.StartCamera(deviceIndex);
        public void StopCamera() => _camera.StopCamera();

        /// <summary>
        /// Captures the current frame, encodes it, and (if a palm is found) adds it to the set of
        /// palms to enroll. Capped at <see cref="RequiredPalmCount"/>.
        /// </summary>
        public async Task<string> CapturePhotoAsync()
        {
            if (_registration.CapturedPalms.Count >= RequiredPalmCount)
            {
                StatusText = $"Έχετε ήδη λάβει {RequiredPalmCount} φωτογραφίες. Πατήστε 'Επόμενο' ή 'Επανάληψη'.";
                return StatusText;
            }

            byte[]? jpeg = _camera.CaptureJpeg();
            if (jpeg == null)
            {
                StatusText = "Δεν υπάρχει διαθέσιμο καρέ από την κάμερα.";
                return StatusText;
            }

            float[]? embedding = await _palmRecognition.EncodeAsync(jpeg);
            if (embedding == null)
            {
                StatusText = "Δεν εντοπίστηκε παλάμη. Δείξτε τη δεξιά σας παλάμη και δοκιμάστε ξανά.";
                return StatusText;
            }

            _registration.CapturedPalms.Add((jpeg, embedding));
            int count = _registration.CapturedPalms.Count;
            StatusText = count >= RequiredPalmCount
                ? $"Ελήφθησαν {count}/{RequiredPalmCount}. Μπορείτε να συνεχίσετε."
                : $"Ελήφθησαν {count}/{RequiredPalmCount} φωτογραφίες.";
            return StatusText;
        }

        /// <summary>Clears all captured palms so the user can retake them.</summary>
        public void Repeat()
        {
            _registration.CapturedPalms.Clear();
            StatusText = $"Οι φωτογραφίες διαγράφηκαν. Λάβετε {RequiredPalmCount} ξανά (0/{RequiredPalmCount}).";
        }

        private void GoNext()
        {
            if (_registration.CapturedPalms.Count < RequiredPalmCount)
            {
                StatusText = $"Πρέπει να λάβετε {RequiredPalmCount} φωτογραφίες της δεξιάς παλάμης (έχετε {_registration.CapturedPalms.Count}).";
                return;
            }

            StopCamera();
            _register05Renavigator.Renavigate();
        }
    }
}
