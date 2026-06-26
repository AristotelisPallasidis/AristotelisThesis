using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AristotelisThesis.Domain.Services;
using AristotelisThesis.WPF.State.Accounts;

namespace AristotelisThesis.WPF.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        // Shown when the student has no enrolled face image. Embedded resource
        // (see the .csproj) so it resolves on any machine, not just the dev box.
        private const string FallbackPhotoUri =
            "pack://application:,,,/Assets/01.jpeg";

        private readonly IAccountStore _accountStore;
        private readonly IFaceImageService _faceImageService;

        public ProfileViewModel(IAccountStore accountStore, IFaceImageService faceImageService)
        {
            _accountStore = accountStore;
            _faceImageService = faceImageService;

            _ = LoadPhoto();
        }

        public int StudentAEM => _accountStore.CurrentAccount.AccountHolder.AEM;
        public string StudentName => _accountStore.CurrentAccount.AccountHolder.Name;
        public string StudentSurname => _accountStore.CurrentAccount.AccountHolder.Surname;
        public string StudentDepartment => "Τμήμα " + _accountStore.CurrentAccount.AccountHolder.Department;
        public string StudentSemester => $"{_accountStore.CurrentAccount.AccountHolder.Semester}ο εξάμηνο";
        public string StudentYearOfEntry => $"Έτος εισαγωγής {_accountStore.CurrentAccount.AccountHolder.YearOfEntry}";
        public string StudentAcademicEmail => _accountStore.CurrentAccount.AccountHolder.AcademicEmail;

        private ImageSource _studentPhoto;
        public ImageSource StudentPhoto
        {
            get => _studentPhoto;
            private set
            {
                _studentPhoto = value;
                OnPropertyChanged(nameof(StudentPhoto));
            }
        }

        private async Task LoadPhoto()
        {
            try
            {
                int studentId = _accountStore.CurrentAccount.AccountHolder.Id;
                byte[] data = await _faceImageService.GetFirstImageData(studentId);

                StudentPhoto = data is { Length: > 0 }
                    ? CreateImage(data)
                    : LoadFallback();
            }
            catch
            {
                StudentPhoto = LoadFallback();
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
