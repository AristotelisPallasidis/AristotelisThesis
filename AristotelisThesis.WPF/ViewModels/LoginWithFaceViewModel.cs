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
    /// Face-recognition login screen. Captures a frame from the webcam, asks the Python
    /// service for its 128-d embedding, and logs the matching student in.
    /// </summary>
    public class LoginWithFaceViewModel : ViewModelBase
    {
        private readonly CameraCaptureService _camera = new CameraCaptureService();
        private readonly IFaceRecognitionService _faceRecognition;
        private readonly IAuthenticator _authenticator;
        private readonly IRenavigator _dashboardRenavigator;

        public ICommand GoToViewLoginCommand { get; }

        /// <summary>Live preview frames forwarded from the capture service.</summary>
        public event EventHandler<BitmapSource>? FrameReady;

        public ObservableCollection<string> Cameras => _camera.Cameras;

        public LoginWithFaceViewModel(
            IFaceRecognitionService faceRecognition,
            IAuthenticator authenticator,
            IRenavigator dashboardRenavigator,
            IRenavigator loginRenavigator)
        {
            _faceRecognition = faceRecognition;
            _authenticator = authenticator;
            _dashboardRenavigator = dashboardRenavigator;

            GoToViewLoginCommand = new RenavigateCommand(loginRenavigator);

            _camera.FrameReady += (s, bmp) => FrameReady?.Invoke(this, bmp);
        }

        public Task RefreshCamerasAsync() => _camera.RefreshCamerasAsync();
        public void StartCamera(int deviceIndex) => _camera.StartCamera(deviceIndex);
        public void StopCamera() => _camera.StopCamera();

        /// <summary>
        /// Captures the current frame, encodes it via the Python service, and attempts a
        /// face login. On success the user is navigated to the Dashboard.
        /// </summary>
        public async Task<FaceLoginResult> RecognizeAndLoginAsync()
        {
            byte[]? jpeg = _camera.CaptureJpeg();
            if (jpeg == null)
            {
                return new FaceLoginResult(false, "Δεν υπάρχει διαθέσιμο καρέ από την κάμερα.");
            }

            float[]? embedding = await _faceRecognition.EncodeAsync(jpeg);
            if (embedding == null)
            {
                return new FaceLoginResult(false, "Δεν εντοπίστηκε πρόσωπο. Δοκιμάστε ξανά.");
            }

            bool success = await _authenticator.LoginWithFace(embedding);
            if (!success)
            {
                return new FaceLoginResult(false, "Δεν βρέθηκε αντιστοίχιση. Η σύνδεση απέτυχε.");
            }

            StopCamera();
            _dashboardRenavigator.Renavigate();
            return new FaceLoginResult(true, "Επιτυχής σύνδεση!");
        }
    }

    public record FaceLoginResult(bool Success, string Message);
}
