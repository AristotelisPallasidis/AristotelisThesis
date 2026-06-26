using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using AForge.Video.DirectShow;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using System.Windows.Media.Imaging;

namespace AristotelisThesis.WPF.Services
{
    /// <summary>
    /// Owns webcam access via OpenCvSharp: enumerates devices, streams frames to the UI,
    /// and snapshots the latest frame as JPEG bytes. Shared by the face-login and
    /// face-enrollment screens so the capture loop isn't duplicated.
    /// </summary>
    public class CameraCaptureService : IDisposable
    {
        /// <summary>Frozen BitmapSource frames for live preview.</summary>
        public event EventHandler<BitmapSource>? FrameReady;

        /// <summary>Camera device indices as strings ("0", "1", ...).</summary>
        public ObservableCollection<string> Cameras { get; } = new ObservableCollection<string>();

        private VideoCapture? _capture;
        private CancellationTokenSource? _captureCts;
        private Mat? _lastFrame;
        private readonly object _frameLock = new object();

        /// <summary>
        /// Lists connected webcams by name via DirectShow. This is instant and runs off the UI
        /// thread, unlike opening every index 0..10 (which froze the page for seconds). The list
        /// order matches the index passed to <see cref="StartCamera"/> (both use DirectShow).
        /// </summary>
        public async Task RefreshCamerasAsync()
        {
            List<string> names = await Task.Run(() =>
            {
                var result = new List<string>();
                try
                {
                    var devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                    foreach (FilterInfo device in devices)
                    {
                        result.Add(device.Name);
                    }
                }
                catch
                {
                    // ignore enumeration failures; fall back below
                }
                return result;
            });

            Cameras.Clear();
            if (names.Count == 0)
            {
                Cameras.Add("Κάμερα 0"); // fallback so the user can still try device 0
                return;
            }
            foreach (string name in names)
            {
                Cameras.Add(name);
            }
        }

        public void StartCamera(int deviceIndex)
        {
            StopCamera();

            // Use the DirectShow backend so the index lines up with RefreshCamerasAsync's order.
            _capture = new VideoCapture(deviceIndex, VideoCaptureAPIs.DSHOW);
            if (!_capture.IsOpened())
            {
                _capture?.Dispose();
                _capture = null;
                return;
            }

            _captureCts = new CancellationTokenSource();
            CancellationToken ct = _captureCts.Token;

            Task.Run(async () =>
            {
                try
                {
                    while (!ct.IsCancellationRequested)
                    {
                        var mat = new Mat();
                        if (!_capture.Read(mat) || mat.Empty())
                        {
                            mat.Dispose();
                            await Task.Delay(10, ct);
                            continue;
                        }

                        lock (_frameLock)
                        {
                            _lastFrame?.Dispose();
                            _lastFrame = mat.Clone();
                        }

                        try
                        {
                            BitmapSource bmp = BitmapSourceConverter.ToBitmapSource(mat);
                            bmp.Freeze();
                            FrameReady?.Invoke(this, bmp);
                        }
                        catch
                        {
                            // ignore conversion errors
                        }

                        mat.Dispose();
                        await Task.Delay(30, ct); // ~30 fps throttle
                    }
                }
                catch (OperationCanceledException)
                {
                    // normal stop
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

        /// <summary>
        /// Encodes the most recent frame as JPEG bytes, or returns null if no frame is available.
        /// </summary>
        public byte[]? CaptureJpeg()
        {
            Mat? photo = null;
            lock (_frameLock)
            {
                if (_lastFrame != null && !_lastFrame.Empty())
                {
                    photo = _lastFrame.Clone();
                }
            }

            if (photo == null)
            {
                return null;
            }

            try
            {
                Cv2.ImEncode(".jpg", photo, out byte[] buffer);
                return buffer;
            }
            finally
            {
                photo.Dispose();
            }
        }

        public void Dispose()
        {
            StopCamera();
        }
    }
}
