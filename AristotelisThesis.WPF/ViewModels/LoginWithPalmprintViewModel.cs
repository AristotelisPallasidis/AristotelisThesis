using AristotelisThesis.Domain.Services;
using AristotelisThesis.WPF.Commands;
using AristotelisThesis.WPF.Services;
using AristotelisThesis.WPF.State.Authenticators;
using AristotelisThesis.WPF.State.Navigators;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace AristotelisThesis.WPF.ViewModels
{
    /// <summary>
    /// Palmprint-recognition login screen. Captures a frame from the webcam, asks the Python
    /// palmprint service for its feature vector, and logs the matching student in.
    /// </summary>
    public class LoginWithPalmprintViewModel : ViewModelBase
    {
        private readonly CameraCaptureService _camera = new CameraCaptureService();
        private readonly IPalmprintRecognitionService _palmRecognition;
        private readonly IAuthenticator _authenticator;
        private readonly IRenavigator _dashboardRenavigator;

        public ICommand GoToViewLoginCommand { get; }

        /// <summary>Live preview frames forwarded from the capture service.</summary>
        public event EventHandler<BitmapSource>? FrameReady;

        public ObservableCollection<string> Cameras => _camera.Cameras;

        public LoginWithPalmprintViewModel(
            IPalmprintRecognitionService palmRecognition,
            IAuthenticator authenticator,
            IRenavigator dashboardRenavigator,
            IRenavigator loginRenavigator)
        {
            _palmRecognition = palmRecognition;
            _authenticator = authenticator;
            _dashboardRenavigator = dashboardRenavigator;

            GoToViewLoginCommand = new RenavigateCommand(loginRenavigator);

            _camera.FrameReady += (s, bmp) => FrameReady?.Invoke(this, bmp);
        }

        public Task RefreshCamerasAsync() => _camera.RefreshCamerasAsync();
        public void StartCamera(int deviceIndex) => _camera.StartCamera(deviceIndex);
        public void StopCamera() => _camera.StopCamera();

        /// <summary>
        /// Captures the current frame, encodes it via the Python palmprint service, and attempts
        /// a palmprint login. On success the user is navigated to the Dashboard.
        /// </summary>
        public async Task<FaceLoginResult> RecognizeAndLoginAsync()
        {
            byte[]? jpeg = _camera.CaptureJpeg();
            if (jpeg == null)
            {
                return new FaceLoginResult(false, "Δεν υπάρχει διαθέσιμο καρέ από την κάμερα.");
            }

            float[]? embedding = await _palmRecognition.EncodeAsync(jpeg);
            if (embedding == null)
            {
                return new FaceLoginResult(false, "Δεν εντοπίστηκε παλάμη. Δείξτε τη δεξιά σας παλάμη και δοκιμάστε ξανά.");
            }

            bool success = await _authenticator.LoginWithPalmprint(embedding);
            if (!success)
            {
                return new FaceLoginResult(false, "Δεν βρέθηκε αντιστοίχιση. Η σύνδεση απέτυχε.");
            }

            StopCamera();
            _dashboardRenavigator.Renavigate();
            return new FaceLoginResult(true, "Επιτυχής σύνδεση!");
        }
    }
}
