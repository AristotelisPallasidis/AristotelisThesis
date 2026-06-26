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
    /// Face-recognition page. Shows the gallery of face images currently enrolled for the
    /// logged-in student.
    /// </summary>
    public class FaceRecognitionViewModel : ViewModelBase
    {
        // Shown in the main preview when the student has no enrolled face image.
        private const string FallbackPhotoUri = "pack://application:,,,/Assets/01.jpeg";

        private readonly IAccountStore _accountStore;
        private readonly IFaceImageService _faceImageService;

        /// <summary>All enrolled face images for the current student.</summary>
        public ObservableCollection<ImageSource> FaceImages { get; } = new();

        private ImageSource _primaryImage;
        public ImageSource PrimaryImage
        {
            get => _primaryImage;
            private set { _primaryImage = value; OnPropertyChanged(nameof(PrimaryImage)); }
        }

        private bool _hasImages;
        public bool HasImages
        {
            get => _hasImages;
            private set { _hasImages = value; OnPropertyChanged(nameof(HasImages)); }
        }

        public FaceRecognitionViewModel(IAccountStore accountStore, IFaceImageService faceImageService)
        {
            _accountStore = accountStore;
            _faceImageService = faceImageService;

            _ = LoadImages();
        }

        private async Task LoadImages()
        {
            PrimaryImage = LoadFallback();
            try
            {
                int studentId = _accountStore.CurrentAccount.AccountHolder.Id;
                IReadOnlyList<byte[]> all = await _faceImageService.GetAllImageData(studentId);

                FaceImages.Clear();
                foreach (byte[] data in all)
                {
                    if (data is { Length: > 0 })
                    {
                        FaceImages.Add(CreateImage(data));
                    }
                }

                HasImages = FaceImages.Count > 0;
                if (HasImages)
                {
                    PrimaryImage = FaceImages[0];
                }
            }
            catch
            {
                // Leave the fallback preview and an empty gallery on failure.
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

        private static ImageSource LoadFallback()
        {
            try
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = new Uri(FallbackPhotoUri, UriKind.Absolute);
                image.EndInit();
                image.Freeze();
                return image;
            }
            catch
            {
                return null;
            }
        }
    }
}
