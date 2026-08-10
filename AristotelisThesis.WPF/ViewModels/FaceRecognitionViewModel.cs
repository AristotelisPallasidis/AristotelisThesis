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
    /// Face-recognition page. Shows the gallery of face images currently enrolled for the
    /// logged-in student.
    /// </summary>
    public class FaceRecognitionViewModel : ViewModelBase
    {
        private readonly IAccountStore _accountStore;
        private readonly IFaceImageService _faceImageService;

        /// <summary>All enrolled face images for the current student.</summary>
        public ObservableCollection<ImageSource> FaceImages { get; } = new();

        /// <summary>False while the student has nothing enrolled, so the page can say so.</summary>
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
            }
            catch (Exception ex)
            {
                // Leave an empty gallery, but say why in the debug output rather than
                // failing silently and looking like the student has nothing enrolled.
                Debug.WriteLine($"Could not load enrolled face images: {ex}");
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
