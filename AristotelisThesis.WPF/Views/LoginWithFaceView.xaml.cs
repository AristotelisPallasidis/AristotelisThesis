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
            // BitmapSource is frozen in the VM, so marshal asynchronously (BeginInvoke) to keep
            // the capture thread from stalling on the UI for every frame.
            CameraFeed.Dispatcher.BeginInvoke(() => CameraFeed.Source = e);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_vm == null)
            {
                return;
            }

            // ItemsSource is the observable collection; the async refresh fills it in without
            // blocking the UI.
            CameraList.ItemsSource = _vm.Cameras;
            _ = _vm.RefreshCamerasAsync();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            _ = _vm?.RefreshCamerasAsync();
        }

        private void CameraList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_vm == null)
            {
                return;
            }

            if (CameraList.SelectedIndex >= 0)
            {
                _vm.StartCamera(CameraList.SelectedIndex);
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

            var result = await _vm.RecognizeAndLoginAsync();
            MessageBox.Show(
                result.Message,
                "Αναγνώριση Προσώπου",
                MessageBoxButton.OK,
                result.Success ? MessageBoxImage.Information : MessageBoxImage.Warning);
        }

        private void RepeatButton_Click(object sender, RoutedEventArgs e)
        {
            if (_vm == null)
            {
                return;
            }

            // Repeat = restart camera (if a camera was selected)
            if (CameraList.SelectedIndex >= 0)
            {
                _vm.StopCamera();
                _vm.StartCamera(CameraList.SelectedIndex);
            }
        }
    }
}