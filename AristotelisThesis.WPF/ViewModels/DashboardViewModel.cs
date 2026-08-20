using AristotelisThesis.Domain.Models;
using AristotelisThesis.Domain.Services;
using AristotelisThesis.WPF.State.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AristotelisThesis.WPF.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        private readonly IAccountStore _accountStore;
        private readonly IStatisticsService _statisticsService;

        private AttendanceStatistics _statistics;

        public DashboardViewModel(IAccountStore accountStore, IStatisticsService statisticsService)
        {
            _accountStore = accountStore;
            _statisticsService = statisticsService;

            // Fire-and-forget the initial load; the percentage binding refreshes via
            // INotifyPropertyChanged once the data arrives.
            _ = LoadStatistics();
        }

        /// <summary>
        /// Share of the current week the student has attended, using the same
        /// days-out-of-seven measure as the weekly graph on the Statistics page.
        /// Shows an em dash until the statistics have loaded.
        /// </summary>
        public string WeeklyAttendancePercentage
        {
            get
            {
                if (_statistics == null)
                {
                    return "—";
                }

                double percent = Math.Round(_statistics.DaysAttendedThisWeek / 7.0 * 100);
                return $"{percent}%";
            }
        }

        public async Task LoadStatistics()
        {
            int? studentId = _accountStore.CurrentAccount?.AccountHolder?.Id;
            if (!studentId.HasValue)
            {
                return;
            }

            try
            {
                _statistics = await _statisticsService.GetForStudent(studentId.Value);
            }
            catch (Exception)
            {
                // Leave the statistics null; the card falls back to its empty state.
                _statistics = null;
            }

            OnPropertyChanged(nameof(WeeklyAttendancePercentage));
        }

        public string StudentFullName => _accountStore.CurrentAccount.AccountHolder.Name + " " + _accountStore.CurrentAccount.AccountHolder.Surname;
        public string StudentDepartment => "Τμήμα " + _accountStore.CurrentAccount.AccountHolder.Department;

        public bool StudentIsPostgraduate => _accountStore.CurrentAccount.AccountHolder.IsPostgraduate;
        public string StudentSex => _accountStore.CurrentAccount.AccountHolder.Sex;


        public string StudentLevelAndGender
        {
            get
            {
                if (StudentSex == "Male" || StudentSex == "male")
                {
                    return StudentIsPostgraduate ? "Μεταπτυχιακός Φοιτητής -" : "Προπτυχιακός Φοιτητής -";
                }
                else if (StudentSex == "Female")
                {
                    return StudentIsPostgraduate ? "Μεταπτυχιακή Φοιτήτρια -" : "Προπτυχιακή Φοιτήτρια -";
                }

                return string.Empty;
            }
        }


    }
}
