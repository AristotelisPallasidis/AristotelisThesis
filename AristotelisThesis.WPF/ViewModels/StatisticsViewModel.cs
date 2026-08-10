using System;
using System.Threading.Tasks;
using AristotelisThesis.Domain.Models;
using AristotelisThesis.Domain.Services;
using AristotelisThesis.WPF.State.Authenticators;

namespace AristotelisThesis.WPF.ViewModels
{
    public class StatisticsViewModel : ViewModelBase
    {
        private readonly IAuthenticator _authenticator;
        private readonly IStatisticsService _statisticsService;

        private AttendanceStatistics _statistics;
        public AttendanceStatistics Statistics
        {
            get => _statistics;
            private set
            {
                _statistics = value;
                OnPropertyChanged(nameof(Statistics));
            }
        }

        /// <summary>
        /// Shared session start time; the live duration counter on the page reads this
        /// so it counts from the actual login moment, consistently across the app.
        /// </summary>
        public DateTime? LoginTime => _authenticator.LoginTime;

        public StatisticsViewModel(IAuthenticator authenticator, IStatisticsService statisticsService)
        {
            _authenticator = authenticator;
            _statisticsService = statisticsService;

            // No subscription to the authenticator here: this view model is resolved fresh on
            // every navigation, so LoginTime is already set by the time the bindings read it,
            // and a handler on the long-lived authenticator would keep each instance alive.

            // Fire-and-forget the initial load; the cards bind to Statistics and refresh
            // via INotifyPropertyChanged once the data arrives.
            _ = Load();
        }

        public async Task Load()
        {
            int? studentId = _authenticator.CurrentAccount?.AccountHolder?.Id;
            if (!studentId.HasValue)
            {
                return;
            }

            try
            {
                Statistics = await _statisticsService.GetForStudent(studentId.Value);
            }
            catch (Exception)
            {
                // Leave Statistics null; cards fall back to their empty-state values.
            }
        }
    }
}
