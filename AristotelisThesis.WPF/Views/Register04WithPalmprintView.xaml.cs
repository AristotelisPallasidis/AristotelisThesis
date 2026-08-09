using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace AristotelisThesis.WPF.Views
{
    /// <summary>
    /// Interaction logic for Register04WithPalmprintView.xaml
    /// </summary>
    public partial class Register04WithPalmprintView : UserControl
    {
        private ViewModels.Register04WithPalmprintViewModel? _vm;

        public Register04WithPalmprintView()
        {
            InitializeComponent();
            DataContextChanged += Register04WithPalmprintView_DataContextChanged;
        }

        private void Register04WithPalmprintView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (_vm != null)
            {
                _vm.FrameReady -= Vm_FrameReady;
            }

            _vm = e.NewValue as ViewModels.Register04WithPalmprintViewModel;

            if (_vm != null)
            {
                _vm.FrameReady += Vm_FrameReady;
            }
        }

        private void Vm_FrameReady(object? sender, BitmapSource e)
        {
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
    }
}
