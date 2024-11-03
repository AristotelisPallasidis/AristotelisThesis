using System.Windows;
using System.Windows.Controls;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace AristotelisThesis.WPF.Views
{
    /// <summary>
    /// Interaction logic for LoginWithFaceView.xaml
    /// </summary>
    public partial class LoginWithFaceView : UserControl
    {
        private FilterInfoCollection _videoDevices; // List of cameras
        private VideoCaptureDevice _videoSource; // The selected camera device
        private int _frameCounter = 0;
        private const int FrameUpdateInterval = 5;
        
        public LoginWithFaceView()
        {
            InitializeComponent();
            _videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            _videoSource = null;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCameras();
        }

        // Method to load available cameras
        private void LoadCameras()
        {
            CameraList.Items.Clear();

            // Use AForge to find available video input devices
            _videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if (_videoDevices.Count > 0)
            {
                foreach (FilterInfo device in _videoDevices)
                {
                    CameraList.Items.Add(device.Name); // Display camera name
                }
            }
            else
            {
                CameraList.Items.Add("No cameras found");
            }
        }

        // Starts the camera feed when a camera is selected
        private void CameraList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (CameraList.SelectedIndex >= 0)
            {
                // If a video source is already running, stop it before starting a new one
                StopVideoSource();

                // Get the selected camera
                var selectedCamera = _videoDevices[CameraList.SelectedIndex];

                // Set up the video source using the selected camera
                _videoSource = new VideoCaptureDevice(selectedCamera.MonikerString);

                // Assign the event handler to process each frame
                _videoSource.NewFrame += new NewFrameEventHandler(Video_NewFrame);

                // Start the video feed
                _videoSource.Start();
            }
        }

        // This event handler processes each frame and displays it in the WPF Image control
        private void Video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                _frameCounter++;
                // Only update the UI every 'frameUpdateInterval' frames to avoid overload
                if (_frameCounter % FrameUpdateInterval == 0)
                {
                    // Get the current frame as a Bitmap
                    using (Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone())
                    {
                        // Convert the Bitmap to a BitmapSource (which WPF can display)
                        var bitmapSource = ConvertBitmapToBitmapSource(bitmap);

                        // Freeze the BitmapSource to make it cross-thread accessible
                        bitmapSource.Freeze();

                        // Update the WPF Image control (on the UI thread)
                        Dispatcher.Invoke(() => { CameraFeed.Source = bitmapSource; });
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error or display it for debugging purposes
                Console.WriteLine($"Error in video frame processing: {ex.Message}");
            }
        }

        // Converts a Bitmap to a BitmapSource for WPF
        private BitmapSource ConvertBitmapToBitmapSource(Bitmap bitmap)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Save the bitmap to a memory stream
                bitmap.Save(memoryStream, ImageFormat.Bmp);
                memoryStream.Seek(0, SeekOrigin.Begin);

                // Create a BitmapImage from the memory stream
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        // Handles the Stop Camera button click to stop the video feed
        private void StopCameraButton_Click(object sender, RoutedEventArgs e)
        {
            StopVideoSource();
        }

        // Method to stop the video feed and release resources
        private void StopVideoSource()
        {
            if (_videoSource != null && _videoSource.IsRunning)
            {
                try
                {
                    _videoSource.SignalToStop(); // Gracefully signal to stop the feed
                    _videoSource.WaitForStop(); // Wait for the thread to stop cleanly
                }
                catch (Exception ex)
                {
                    // Handle any exceptions during stop (thread destruction issues)
                    Console.WriteLine($"Error while stopping video source: {ex.Message}");
                }
                finally
                {
                    _videoSource = null; // Ensure we release the reference
                }
            }
        }

        // Handles the Refresh Button Click event to reload the camera list
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            StopVideoSource(); // Stop any current video feed
            LoadCameras(); // Reload available cameras
        }

    }
}