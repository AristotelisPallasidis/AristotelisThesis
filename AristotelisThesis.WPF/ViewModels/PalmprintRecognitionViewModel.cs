using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

        /// <summary>False while the student has nothing enrolled, so the page can say so.</summary>
        private bool _hasImages;
        public bool HasImages
        {
            get => _hasImages;
            private set { _hasImages = value; OnPropertyChanged(nameof(HasImages)); }
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

                HasImages = PalmImages.Count > 0;
            }
            catch (Exception ex)
            {
                // Leave an empty gallery, but say why in the debug output rather than
                // failing silently and looking like the student has nothing enrolled.
                Debug.WriteLine($"Could not load enrolled palm images: {ex}");
                HasImages = false;
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
