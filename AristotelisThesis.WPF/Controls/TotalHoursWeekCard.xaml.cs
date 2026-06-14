using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AristotelisThesis.Domain.Models;

namespace AristotelisThesis.WPF.Controls
{
    /// <summary>
    /// Interaction logic for TotalHoursWeekCard.xaml
    /// </summary>
    public partial class TotalHoursWeekCard : UserControl
    {
        public static readonly DependencyProperty StatisticsProperty =
            DependencyProperty.Register(nameof(Statistics), typeof(AttendanceStatistics), typeof(TotalHoursWeekCard),
                new PropertyMetadata(null, OnStatisticsChanged));

        public AttendanceStatistics Statistics
        {
            get => (AttendanceStatistics)GetValue(StatisticsProperty);
            set => SetValue(StatisticsProperty, value);
        }

        public TotalHoursWeekCard()
        {
            InitializeComponent();
            Loaded += (_, __) => UpdateUI();
        }

        private static void OnStatisticsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TotalHoursWeekCard)d).UpdateUI();
        }

        private void UpdateUI()
        {
            try
            {
                if (Statistics == null)
                {
                    SetMainText("0", "0");
                    return;
                }

                // MonthlyActiveHours exists but here we want weekly total: try using WeeklyAverageActiveTime * DaysAttendedThisWeek
                var avg = Statistics.WeeklyAverageActiveTime;
                var days = Math.Max(0, Statistics.DaysAttendedThisWeek);
                var total = TimeSpan.FromTicks(avg.Ticks * days);

                var hours = (int)total.TotalHours;
                var minutes = total.Minutes;

                SetMainText(hours.ToString("D2"), minutes.ToString("D2"));
            }
            catch (FormatException fx)
            {
                Debug.WriteLine($"FormatException in TotalHoursWeekCard: {fx}");
                SetMainText("—", "—");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in TotalHoursWeekCard: {ex}");
                SetMainText("—", "—");
            }
        }

        private void SetMainText(string hours, string minutes)
        {
            // find two large TextBlocks by FontSize
            var tbs = FindLargeTextBlocks(this, 2);
            if (tbs.Length >= 1) tbs[0].Text = hours;
            if (tbs.Length >= 2) tbs[1].Text = minutes;
        }

        private static TextBlock[] FindLargeTextBlocks(DependencyObject parent, int count)
        {
            var list = new System.Collections.Generic.List<TextBlock>();
            CollectLarge(parent, list);
            return list.ToArray();

            static void CollectLarge(DependencyObject node, System.Collections.Generic.List<TextBlock> outList)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(node); i++)
                {
                    var ch = VisualTreeHelper.GetChild(node, i);
                    if (ch is TextBlock t && t.FontSize >= 60) outList.Add(t);
                    CollectLarge(ch, outList);
                }
            }
        }
    }
}
