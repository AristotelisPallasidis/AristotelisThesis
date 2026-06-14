using AristotelisThesis.WPF.Commands;
using AristotelisThesis.WPF.State.Navigators;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using OpenCvSharp;
using OpenCvSharp.Face;
using OpenCvSharp.WpfExtensions;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using AristotelisThesis.EntityFramework;

namespace AristotelisThesis.WPF.ViewModels
{
    public class LoginWithFaceViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public ICommand GoToViewLoginCommand { get; }

        public LoginWithFaceViewModel(IRenavigator loginRenavigator)
        {
            GoToViewLoginCommand = new RenavigateCommand(loginRenavigator);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        // Publishes frozen BitmapSource frames to the view
        public event EventHandler<BitmapSource>? FrameReady;

        // Cameras as simple indices ("0", "1", ...)
        public ObservableCollection<string> Cameras { get; } = new ObservableCollection<string>();

        private VideoCapture? _capture;
        private CancellationTokenSource? _captureCts;
        private Mat? _lastFrame;
        private readonly object _frameLock = new object();

        private string _resultText = "";
        public string ResultText
        {
            get => _resultText;                         
            private set
            {
                if (_resultText != value)
                {
                    _resultText = value;
                    OnPropertyChanged(nameof(ResultText));
                }
            }
        }

        // Recognition threshold (tweakable)
        public double RecognitionThreshold { get; set; } = 60.0;

        // Path to Haar cascade file (add the file to your project and set CopyToOutputDirectory = Always)
        private readonly string _cascadePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "haarcascade_frontalface_default.xml");

        // Constructors to match existing usages
        public LoginWithFaceViewModel() : this(null) { }
        public LoginWithFaceViewModel(object? loginRenavigator)
        {
            // No-op: renavigator not used here
        }

        // Enumerate devices by probing indices only (no friendly names)
        public void RefreshCameras()
        {
            Cameras.Clear();
            // probe indices 0..10
            for (int i = 0; i <= 10; i++)
            {
                try
                {
                    using var cap = new VideoCapture(i);
                    if (cap.IsOpened())
                    {
                        Cameras.Add(i.ToString());
                    }
                }
                catch
                {
                    // ignore failures
                }
            }

            if (Cameras.Count == 0)
            {
                Cameras.Add("0"); // fallback
            }
        }

        public void StartCamera(int deviceIndex)
        {
            StopCamera();

            _capture = new VideoCapture(deviceIndex);
            if (!_capture.IsOpened())
            {
                _capture?.Dispose();
                _capture = null;
                return;
            }

            _captureCts = new CancellationTokenSource();
            var ct = _captureCts.Token;

            Task.Run(() =>
            {
                try
                {
                    while (!ct.IsCancellationRequested)
                    {
                        var mat = new Mat();
                        if (!_capture.Read(mat) || mat.Empty())
                        {
                            mat.Dispose();
                            Thread.Sleep(10);
                            continue;
                        }

                        // keep last frame for capture
                        lock (_frameLock)
                        {
                            _lastFrame?.Dispose();
                            _lastFrame = mat.Clone();
                        }

                        // Convert to BitmapSource for display (freeze so it can be used across threads)
                        try
                        {
                            var bmp = BitmapSourceConverter.ToBitmapSource(mat);
                            bmp.Freeze();
                            FrameReady?.Invoke(this, bmp);
                        }
                        catch
                        {
                            // ignore conversion errors
                        }

                        mat.Dispose();
                        Thread.Sleep(30); // ~30 fps throttle
                    }
                }
                catch
                {
                    // swallow loop exceptions
                }
            }, ct);
        }

        public void StopCamera()
        {
            try
            {
                _captureCts?.Cancel();
                _captureCts?.Dispose();
                _captureCts = null;
            }
            catch { }

            try
            {
                _capture?.Release();
                _capture?.Dispose();
                _capture = null;
            }
            catch { }

            lock (_frameLock)
            {
                _lastFrame?.Dispose();
                _lastFrame = null;
            }
        }

        public async Task<string> TakePhotoAndRecognizeAsync()
        {
            Mat? photo = null;
            lock (_frameLock)
            {
                if (_lastFrame != null)
                    photo = _lastFrame.Clone();
            }

            if (photo == null || photo.Empty())
            {
                ResultText = "No camera frame available.";
                return ResultText;
            }

            try
            {
                // Convert to grayscale
                Mat gray = new Mat();
                Cv2.CvtColor(photo, gray, ColorConversionCodes.BGR2GRAY);

                // Face detection: crop to the largest detected face if cascade exists
                Mat faceMat = null;
                if (File.Exists(_cascadePath))
                {
                    try
                    {
                        using var cascade = new CascadeClassifier(_cascadePath);
                        var rects = cascade.DetectMultiScale(gray, 1.1, 5, HaarDetectionTypes.ScaleImage, new OpenCvSharp.Size(50, 50));
                        if (rects != null && rects.Length > 0)
                        {
                            var best = rects.OrderByDescending(r => r.Width * r.Height).First();
                            faceMat = new Mat(gray, best);
                        }
                    }
                    catch
                    {
                        faceMat = null;
                    }
                }

                if (faceMat == null)
                {
                    // fallback to whole frame
                    faceMat = gray.Clone();
                }

                // Preprocess: resize + equalize
                if (faceMat.Width != 250 || faceMat.Height != 250)
                    Cv2.Resize(faceMat, faceMat, new OpenCvSharp.Size(250, 250));
                Cv2.EqualizeHist(faceMat, faceMat);

                // Load training images from DB and prepare recognizer
                var labels = new List<int>();
                var images = new List<Mat>();

                // Use the design-time factory to create a DbContext instance
                using var db = new AristotelisThesisDbContextFactory().CreateDbContext();
                foreach (var faceImage in db.FaceImages)
                {
                    Mat img = Mat.FromImageData(faceImage.ImageData, ImreadModes.Grayscale);
                    if (img.Width != 250 || img.Height != 250)
                        Cv2.Resize(img, img, new OpenCvSharp.Size(250, 250));
                    Cv2.EqualizeHist(img, img);
                    images.Add(img);
                    labels.Add(faceImage.StudentId);
                }

                if (images.Count == 0)
                {
                    ResultText = "No training images in DB.";
                    faceMat.Dispose();
                    gray.Dispose();
                    photo.Dispose();
                    return ResultText;
                }

                var recognizer = LBPHFaceRecognizer.Create();
                recognizer.Train(images, labels);

                int predictedLabel = -1;
                double confidence = double.MaxValue;
                recognizer.Predict(faceMat, out predictedLabel, out confidence);

                string result;
                if (predictedLabel != -1 && confidence < RecognitionThreshold)
                {
                    result = $"Matched with Student ID: {predictedLabel} (Confidence: {confidence:F2})";
                }
                else
                {
                    result = "No match found!";
                }

                foreach (var m in images) m.Dispose();
                faceMat.Dispose();
                gray.Dispose();
                photo.Dispose();

                ResultText = result;
                await Task.CompletedTask;
                return result;
            }
            catch (Exception ex)
            {
                photo.Dispose();
                ResultText = $"Error during recognition: {ex.Message}";
                return ResultText;
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
