using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace AristotelisThesis.WPF.Views
{
    /// <summary>
    /// Interaction logic for Register06WithFaceView.xaml
    /// </summary>
    public partial class Register06WithFaceView : UserControl
    {
        private ViewModels.Register06WithFaceViewModel? _vm;

        public Register06WithFaceView()
        {
            InitializeComponent();
            DataContextChanged += Register06WithFaceView_DataContextChanged;
        }

        private void Register06WithFaceView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (_vm != null)
            {
                _vm.FrameReady -= Vm_FrameReady;
            }

            _vm = e.NewValue as ViewModels.Register06WithFaceViewModel;

            if (_vm != null)
            {
                _vm.FrameReady += Vm_FrameReady;
            }
        }

        private void Vm_FrameReady(object? sender, BitmapSource e)
        {
            // Async marshal so the capture thread isn't blocked on the UI per frame.
            CameraFeed.Dispatcher.BeginInvoke(() => CameraFeed.Source = e);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_vm == null)
            {
                return;
            }

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

        private async void TakePhotoButton_Click(object sender, RoutedEventArgs e)
        {
            if (_vm == null)
            {
                return;
            }

            await _vm.CapturePhotoAsync();
        }

        private void RepeatButton_Click(object sender, RoutedEventArgs e)
        {
            _vm?.Repeat();
        }

        private async void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            if (_vm == null)
            {
                return;
            }

            var result = await _vm.FinishAsync();
            MessageBox.Show(
                result.Message,
                "Εγγραφή",
                MessageBoxButton.OK,
                result.Success ? MessageBoxImage.Information : MessageBoxImage.Warning);
        }
    }
}
