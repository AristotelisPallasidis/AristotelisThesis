using AristotelisThesis.Domain.Models;
using AristotelisThesis.Domain.Services;
using AristotelisThesis.Domain.Services.AuthenticationServices;
using AristotelisThesis.EntityFramework;
using AristotelisThesis.EntityFramework.Services;
using AristotelisThesis.WPF.Services;
using AristotelisThesis.WPF.State;
using AristotelisThesis.WPF.State.Accounts;
using AristotelisThesis.WPF.State.Authenticators;
using AristotelisThesis.WPF.State.Navigators;
using AristotelisThesis.WPF.ViewModels;
using AristotelisThesis.WPF.ViewModels.Factories;
using AristotelisThesis.WPF.Views;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace AristotelisThesis.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider? _serviceProvider;
        private PythonServiceLauncher? _faceServiceLauncher;

        protected override void OnStartup(StartupEventArgs e)
        {
            IServiceProvider serviceProvider = CreateServiceProvider();
            _serviceProvider = serviceProvider;

            // Bring up the Python face-embedding service in the background so the model is
            // loaded by the time the user reaches the face-login screen. Non-blocking: the UI
            // starts regardless, and face login degrades gracefully if the service is down.
            _faceServiceLauncher = serviceProvider.GetRequiredService<PythonServiceLauncher>();
            _ = _faceServiceLauncher.StartAsync();

            IAuthenticationService authentication = serviceProvider.GetRequiredService<IAuthenticationService>();
            //authentication.Login("Aris", "aris");
            //authentication.Register(
            //   "maria.pallasid@gmail.com", // email
            //   "maria",                    // username
            //   "maria",                           // password
            //   "maria",                           // confirmPassword
            //   "Maria",                    // name
            //   "Pallasidou",                     // surname
            //   "Female",                           // sex
            //   "6933015797",                     // phone
            //   "Anagenniseos 19 Pefka Thessalonikis",// address
            //   "Φυσικής",                        // department
            //   2,                                // semester
            //   4510,                             // aem
            //   new DateTime(1998, 9, 23),       // dateOfBirth
            //   2019,                             // yearOfEntry
            //   true                             // isPostgraduate
            //);


            Window window = serviceProvider.GetRequiredService<MainWindow>();
            window.Show();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Kill the Python face service we spawned so it doesn't outlive the app.
            _faceServiceLauncher?.Dispose();
            base.OnExit(e);
        }

        public IServiceProvider CreateServiceProvider()
        {
            IServiceCollection services= new ServiceCollection();

            // 1. Singleton => Only one instance of the service is created and shared across the application.
            // 2. Transient => A new instance of the service is created every time it is requested.
            // 3. Scoped => A new instance of the service is created once per HTTP request (within the scope of HTTP request).

            // Register services
            services.AddSingleton<AristotelisThesisDbContextFactory>();
            services.AddSingleton<IAuthenticationService, AuthenticationService>();
            services.AddSingleton<IDataService<Account>, AccountDataService>();
            services.AddSingleton<IAccountService, AccountDataService>();

            // Attendance tracking + statistics
            services.AddSingleton<ISessionTrackingService, SessionTrackingService>();
            services.AddSingleton<IStatisticsService, StatisticsService>();

            // Profile photo source + face enrollment storage
            services.AddSingleton<IFaceImageService, FaceImageService>();

            // Face recognition: Python ResNet-34 embedding bridge + its process launcher.
            services.AddSingleton<IFaceRecognitionService, PythonFaceRecognitionService>();
            services.AddSingleton<PythonServiceLauncher>();

            // Carries registration-wizard data (personal info + captured faces) across steps.
            services.AddSingleton<RegistrationStore>();

            services.AddSingleton<IPasswordHasher, PasswordHasher>();


            // Register factories
            services.AddSingleton<IAristotelisThesisViewModelFactory, AristotelisThesisViewModelFactory>();


            services.AddSingleton<LoginViewModel>();
            services.AddSingleton<DashboardViewModel>();
            // Transient so the face gallery reloads for whoever is currently logged in.
            services.AddTransient<FaceRecognitionViewModel>();
            services.AddSingleton<PalmprintRecognitionViewModel>();
            // Per-user view state: resolve a fresh instance on each navigation so a
            // logout/login as a different student never shows the previous user's
            // cached fields or profile photo.
            services.AddTransient<ProfileViewModel>();
            services.AddSingleton<StatisticsViewModel>();
            services.AddTransient<SettingsViewModel>();

            // --------------------------------------------------------------------------------
            // Register Delegates
            // --------------------------------------------------------------------------------
            services.AddSingleton<CreateViewModel<DashboardViewModel>>(services =>
            {
                return () => services.GetRequiredService<DashboardViewModel>();

            });
            
            services.AddSingleton<CreateViewModel<FaceRecognitionViewModel>>(services =>
            {
                return () => services.GetRequiredService<FaceRecognitionViewModel>();

            });
            
            services.AddSingleton<CreateViewModel<PalmprintRecognitionViewModel>>(services =>
            {
                return () => services.GetRequiredService<PalmprintRecognitionViewModel>();

            });
            
            services.AddSingleton<CreateViewModel<ProfileViewModel>>(services =>
            {
                return () => services.GetRequiredService<ProfileViewModel>();

            });
            
            services.AddSingleton<CreateViewModel<StatisticsViewModel>>(services =>
            {
                return () => services.GetRequiredService<StatisticsViewModel>();

            });
            
            services.AddSingleton<CreateViewModel<SettingsViewModel>>(services =>
            {
                return () => services.GetRequiredService<SettingsViewModel>();

            });



            // --------------------------------------------------------------------------------
            // Register LoginWithFaceViewModel & LoginWithPalmprintViewModel
            // --------------------------------------------------------------------------------
            services.AddSingleton<ViewModelDelegateRenavigator<LoginViewModel>>();
            services.AddSingleton<CreateViewModel<LoginWithFaceViewModel>>(services =>
            {
                return () => new LoginWithFaceViewModel(
                    services.GetRequiredService<IFaceRecognitionService>(),
                    services.GetRequiredService<IAuthenticator>(),
                    services.GetRequiredService<ViewModelDelegateRenavigator<DashboardViewModel>>(),
                    services.GetRequiredService<ViewModelDelegateRenavigator<LoginViewModel>>()
                );
            });
            
            services.AddSingleton<CreateViewModel<LoginWithPalmprintViewModel>>(services =>
            {
                return () => new LoginWithPalmprintViewModel(
                    services.GetRequiredService<ViewModelDelegateRenavigator<LoginViewModel>>()
                );
            });

            // --------------------------------------------------------------------------------
            // Register
            // --------------------------------------------------------------------------------

            services.AddSingleton<ViewModelDelegateRenavigator<Register01ViewModel>>();
            services.AddSingleton<ViewModelDelegateRenavigator<Register02WithInformationViewModel>>();
            services.AddSingleton<ViewModelDelegateRenavigator<Register03InstructionsForPalmprintViewModel>>();
            services.AddSingleton<ViewModelDelegateRenavigator<Register04WithPalmprintViewModel>>();
            services.AddSingleton<ViewModelDelegateRenavigator<Register05InstructionsForFaceViewModel>>();
            services.AddSingleton<ViewModelDelegateRenavigator<Register06WithFaceViewModel>>();


            services.AddSingleton<CreateViewModel<Register01ViewModel>>(services =>
            {
                return () => new Register01ViewModel(
                    services.GetRequiredService<ViewModelDelegateRenavigator<LoginViewModel>>(),
                    services.GetRequiredService<ViewModelDelegateRenavigator<Register02WithInformationViewModel>>()
                );
            });

            services.AddSingleton<CreateViewModel<Register02WithInformationViewModel>>(services =>
            {
                return () => new Register02WithInformationViewModel(
                    services.GetRequiredService<RegistrationStore>(),
                    services.GetRequiredService<ViewModelDelegateRenavigator<Register01ViewModel>>(),
                    // Palmprint steps (03/04) skipped for now: go straight to the face instructions.
                    services.GetRequiredService<ViewModelDelegateRenavigator<Register05InstructionsForFaceViewModel>>()
                );
            });
            
            services.AddSingleton<CreateViewModel<Register03InstructionsForPalmprintViewModel>>(services =>
            {
                return () => new Register03InstructionsForPalmprintViewModel(
                    services.GetRequiredService<ViewModelDelegateRenavigator<Register02WithInformationViewModel>>(),
                    services.GetRequiredService<ViewModelDelegateRenavigator<Register04WithPalmprintViewModel>>()
                );
            });
            
            services.AddSingleton<CreateViewModel<Register04WithPalmprintViewModel>>(services =>
            {
                return () => new Register04WithPalmprintViewModel(
                    services.GetRequiredService<ViewModelDelegateRenavigator<Register03InstructionsForPalmprintViewModel>>(),
                    services.GetRequiredService<ViewModelDelegateRenavigator<Register05InstructionsForFaceViewModel>>()
                );
            });
            
            services.AddSingleton<CreateViewModel<Register05InstructionsForFaceViewModel>>(services =>
            {
                return () => new Register05InstructionsForFaceViewModel(
                    // Palmprint steps skipped: Back returns to the personal-info step.
                    services.GetRequiredService<ViewModelDelegateRenavigator<Register02WithInformationViewModel>>(),
                    services.GetRequiredService<ViewModelDelegateRenavigator<Register06WithFaceViewModel>>()
                );
            });
            
            services.AddSingleton<CreateViewModel<Register06WithFaceViewModel>>(services =>
            {
                return () => new Register06WithFaceViewModel(
                    services.GetRequiredService<RegistrationStore>(),
                    services.GetRequiredService<IFaceRecognitionService>(),
                    services.GetRequiredService<IFaceImageService>(),
                    services.GetRequiredService<IAccountService>(),
                    services.GetRequiredService<IAuthenticator>(),
                    services.GetRequiredService<ViewModelDelegateRenavigator<Register05InstructionsForFaceViewModel>>(),
                    services.GetRequiredService<ViewModelDelegateRenavigator<DashboardViewModel>>()
                );
            });



            services.AddSingleton<ViewModelDelegateRenavigator<DashboardViewModel>>();
            services.AddSingleton<ViewModelDelegateRenavigator<LoginWithFaceViewModel>>();
            services.AddSingleton<ViewModelDelegateRenavigator<LoginWithPalmprintViewModel>>();
            services.AddSingleton<CreateViewModel<LoginViewModel>>(services =>
            {
                return () => new LoginViewModel(
                    services.GetRequiredService<IAuthenticator>(),
                    services.GetRequiredService<ViewModelDelegateRenavigator<DashboardViewModel>>(),
                    services.GetRequiredService<ViewModelDelegateRenavigator<LoginWithFaceViewModel>>(),
                    services.GetRequiredService<ViewModelDelegateRenavigator<LoginWithPalmprintViewModel>>(),
                    services.GetRequiredService<ViewModelDelegateRenavigator<Register01ViewModel>>()
                );
            });


            // This is were we manage the STATES of the application
            services.AddSingleton<INavigator, Navigator>();
            services.AddSingleton<IAuthenticator, Authenticator>();
            services.AddSingleton<IAccountStore, AccountStore>();


            services.AddScoped<MainViewModel>(services =>
            {
                return new MainViewModel(
                    services.GetRequiredService<INavigator>(),
                    services.GetRequiredService<IAristotelisThesisViewModelFactory>(),
                    services.GetRequiredService<IAuthenticator>(),
                    services.GetRequiredService<ViewModelDelegateRenavigator<LoginViewModel>>()
                );
            });


            services.AddScoped<MainWindow>(s => new MainWindow(s.GetRequiredService<MainViewModel>()));

            return services.BuildServiceProvider();
        }
    }

}
