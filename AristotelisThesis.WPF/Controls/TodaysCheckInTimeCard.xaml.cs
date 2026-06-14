using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using AristotelisThesis.Domain.Models;

namespace AristotelisThesis.WPF.Controls
{
    public partial class TodaysCheckInTimeCard : UserControl
    {
        public static readonly DependencyProperty StatisticsProperty =
            DependencyProperty.Register(nameof(Statistics), typeof(AttendanceStatistics), typeof(TodaysCheckInTimeCard),
                new PropertyMetadata(null, OnStatisticsChanged));

        public AttendanceStatistics Statistics
        {
            get => (AttendanceStatistics)GetValue(StatisticsProperty);
            set => SetValue(StatisticsProperty, value);
        }

        public TodaysCheckInTimeCard()
        {
            InitializeComponent();
            Loaded += (_, __) => UpdateUI();
        }

        private static void OnStatisticsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TodaysCheckInTimeCard)d).UpdateUI();
        }

        private void UpdateUI()
        {
            try
            {
                if (Statistics == null)
                {
                    SetValue("—", string.Empty);
                    return;
                }

                // Prefer the explicit check-in timestamp recorded on login.
                if (Statistics.TodayCheckIn.HasValue)
                {
                    DateTime dt = Statistics.TodayCheckIn.Value;
                    SetValue(dt.ToString("HH:mm", CultureInfo.InvariantCulture), Meridiem(dt.Hour));
                    return;
                }

                // No check-in recorded today.
                SetValue("--:--", string.Empty);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in TodaysCheckInTimeCard: {ex}");
                SetValue("—", string.Empty);
            }
        }

        // Greek meridiem: π.μ. before noon, μ.μ. from noon onwards.
        private static string Meridiem(int hour) => hour < 12 ? "π.μ" : "μ.μ";

        private void SetValue(string time, string meridiem)
        {
            TimeText.Text = time;
            MeridiemText.Text = meridiem;
        }
    }
}
