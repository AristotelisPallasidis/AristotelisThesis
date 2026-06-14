using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace AristotelisThesis.WPF.Views
{
    public partial class LoginWithFaceView : UserControl
    {
        private ViewModels.LoginWithFaceViewModel? _vm;

        public LoginWithFaceView()
        {
            InitializeComponent();
            DataContextChanged += LoginWithFaceView_DataContextChanged;
        }

        private void LoginWithFaceView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (_vm != null)
            {
                _vm.FrameReady -= Vm_FrameReady;
            }

            _vm = e.NewValue as ViewModels.LoginWithFaceViewModel;

            if (_vm != null)
            {
                _vm.FrameReady += Vm_FrameReady;
            }
        }

        private void Vm_FrameReady(object? sender, BitmapSource e)
        {
            // Called from background thread but BitmapSource is frozen in VM, safe to set directly.
            CameraFeed.Dispatcher.Invoke(() => CameraFeed.Source = e);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_vm == null)
            {
                return;
            }

            // populate camera list on load
            _vm.RefreshCameras();
            CameraList.ItemsSource = _vm.Cameras;
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (_vm == null)
            {
                return;
            }

            _vm.RefreshCameras();
        }

        private void CameraList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_vm == null)
            {
                return;
            }

            if (CameraList.SelectedItem is string s && int.TryParse(s, out int idx))
            {
                _vm.StartCamera(idx);
            }
        }

        private void StopCameraButton_Click(object sender, RoutedEventArgs e)
        {
            if (_vm == null)
            {
                return;
            }

            _vm.StopCamera();
            CameraFeed.Source = null;
        }

        private async void TakePhotoButton_Click(object sender, RoutedEventArgs e)
        {
            if (_vm == null)
            {
                return;
            }

            var result = await _vm.TakePhotoAndRecognizeAsync();
            MessageBox.Show(result, "Αναγνώριση Προσώπου", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void RepeatButton_Click(object sender, RoutedEventArgs e)
        {
            if (_vm == null)
            {
                return;
            }

            // Repeat = restart camera (if a camera was selected)
            if (CameraList.SelectedItem is string s && int.TryParse(s, out int idx))
            {
                _vm.StopCamera();
                _vm.StartCamera(idx);
            }
        }
    }
}