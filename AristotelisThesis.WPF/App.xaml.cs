using AristotelisThesis.Domain.Models;
using AristotelisThesis.Domain.Services;
using AristotelisThesis.Domain.Services.AuthenticationServices;
using AristotelisThesis.EntityFramework;
using AristotelisThesis.EntityFramework.Services;
using AristotelisThesis.WPF.State.Accounts;
using AristotelisThesis.WPF.State.Authenticators;
using AristotelisThesis.WPF.State.Navigators;
using AristotelisThesis.WPF.ViewModels;
using AristotelisThesis.WPF.ViewModels.Factories;
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
        protected override void OnStartup(StartupEventArgs e)
        {
            IServiceProvider serviceProvider = CreateServiceProvider();

            IAuthenticationService authentication = serviceProvider.GetRequiredService<IAuthenticationService>();
            //authentication.Login("Aris", "aris");
            //authentication.Register(
            //   "aristotelis.pallasid@gmail.com", // email
            //   "aristotelis",                    // username
            //   "aris",                           // password
            //   "aris",                           // confirmPassword
            //   "Aristotelis",                    // name
            //   "Pallasidis",                     // surname
            //   "Male",                           // sex
            //   "6933015797",                     // phone
            //   "Anagenniseos 19 Pefka Thessalonikis",// address
            //   "Computer Science",               // department
            //   2,                                // semester
            //   4509,                             // aem
            //   new DateTime(2000, 10, 30),       // dateOfBirth
            //   2019,                             // yearOfEntry
            //   false                             // isPostgraduate
            //);
            



            Window window = serviceProvider.GetRequiredService<MainWindow>();
            window.Show();

            base.OnStartup(e);
        }

        public IServiceProvider CreateServiceProvider()
        {
            IServiceCollection services= new ServiceCollection();

            // 1. Singleton => Only one instance of the service is created and shared across the application.
            // 2. Transient => A new instance of the service is created every time it is requested.
            // 3. Scoped => A new instance of the service is created once per request within the scope.

            // Register services
            services.AddSingleton<AristotelisThesisDbContextFactory>();
            services.AddSingleton<IAuthenticationService, AuthenticationService>();
            services.AddSingleton<IDataService<Account>, AccountDataService>();
            services.AddSingleton<IAccountService, AccountDataService>();

            services.AddSingleton<IPasswordHasher, PasswordHasher>();


            // Register factories
            services.AddSingleton<IAristotelisThesisViewModelFactory, AristotelisThesisViewModelFactory>();


            services.AddSingleton<LoginViewModel>();
            services.AddSingleton<DashboardViewModel>();
            services.AddSingleton<FaceRecognitionViewModel>();
            services.AddSingleton<PalmprintRecognitionViewModel>();
            services.AddSingleton<ProfileViewModel>();
            services.AddSingleton<StatisticsViewModel>();
            services.AddSingleton<SettingsViewModel>();

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

            services.AddSingleton<ViewModelDelegateRenavigator<DashboardViewModel>>();
            services.AddSingleton<CreateViewModel<LoginViewModel>>(services =>
            {
                return () => new LoginViewModel(
                    services.GetRequiredService<IAuthenticator>(),
                    services.GetRequiredService<ViewModelDelegateRenavigator<DashboardViewModel>>());
            });


            // This is were we manage the STATES of the application
            services.AddSingleton<INavigator, Navigator>();
            services.AddSingleton<IAuthenticator, Authenticator>();
            services.AddSingleton<IAccountStore, AccountStore>();


            //services.AddScoped<LoginWithFaceViewModel>();
            //services.AddScoped<LoginWithPalmprintViewModel>();
            //services.AddScoped<Register01ViewModel>();
            //services.AddScoped<Register02WithInformationViewModel>();
            //services.AddScoped<Register03InstructionsForPalmprintViewModel>();
            //services.AddScoped<Register04WithPalmprintViewModel>();
            //services.AddScoped<Register05InstructionsForFaceViewModel>();
            //services.AddScoped<Register06WithFaceViewModel>();
            //services.AddScoped<DashboardViewModel>();
            //services.AddScoped<FaceRecognitionViewModel>();
            //services.AddScoped<PalmprintRecognitionViewModel>();
            //services.AddScoped<ProfileViewModel>();
            //services.AddScoped<StatisticsViewModel>();
            //services.AddScoped<SettingsViewModel>();
            services.AddScoped<MainViewModel>();


            services.AddScoped<MainWindow>(s => new MainWindow(s.GetRequiredService<MainViewModel>()));

            return services.BuildServiceProvider();
        }
    }

}
