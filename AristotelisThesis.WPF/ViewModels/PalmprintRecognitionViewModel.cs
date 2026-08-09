using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AristotelisThesis.Domain.Services;
using AristotelisThesis.WPF.State.Accounts;

namespace AristotelisThesis.WPF.ViewModels
{
    /// <summary>
    /// Palmprint-recognition page. Shows the gallery of palm images currently enrolled for the
    /// logged-in student. Mirror of <see cref="FaceRecognitionViewModel"/>.
    /// </summary>
    public class PalmprintRecognitionViewModel : ViewModelBase
    {
        private readonly IAccountStore _accountStore;
        private readonly IPalmprintImageService _palmImageService;

        /// <summary>All enrolled palm images for the current student.</summary>
        public ObservableCollection<ImageSource> PalmImages { get; } = new();

        private ImageSource _primaryImage;
        public ImageSource PrimaryImage
        {
            get => _primaryImage;
            private set { _primaryImage = value; OnPropertyChanged(nameof(PrimaryImage)); }
        }

        public PalmprintRecognitionViewModel(IAccountStore accountStore, IPalmprintImageService palmImageService)
        {
            _accountStore = accountStore;
            _palmImageService = palmImageService;

            _ = LoadImages();
        }

        private async Task LoadImages()
        {
            try
            {
                int studentId = _accountStore.CurrentAccount.AccountHolder.Id;
                IReadOnlyList<byte[]> all = await _palmImageService.GetAllImageData(studentId);

                PalmImages.Clear();
                foreach (byte[] data in all)
                {
                    if (data is { Length: > 0 })
                    {
                        PalmImages.Add(CreateImage(data));
                    }
                }

                if (PalmImages.Count > 0)
                {
                    PrimaryImage = PalmImages[0];
                }
            }
            catch
            {
                // Leave an empty gallery on failure.
            }
        }

        private static BitmapImage CreateImage(byte[] data)
        {
            var image = new BitmapImage();
            using (var ms = new MemoryStream(data))
            {
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }
    }
}
