using System;
    using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AristotelisThesis.Domain.Models;

namespace AristotelisThesis.WPF.Controls
{
    /// <summary>
    /// Interaction logic for StreakTrackerCard.xaml
    /// </summary>
    public partial class StreakTrackerCard : UserControl
    {
        public static readonly DependencyProperty StatisticsProperty =
            DependencyProperty.Register(nameof(Statistics), typeof(AttendanceStatistics), typeof(StreakTrackerCard),
                new PropertyMetadata(null, OnStatisticsChanged));

        public AttendanceStatistics Statistics
        {
            get => (AttendanceStatistics)GetValue(StatisticsProperty);
            set => SetValue(StatisticsProperty, value);
        }

        public StreakTrackerCard()
        {
            InitializeComponent();
            Loaded += (_, __) => UpdateUI();
        }

        private static void OnStatisticsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((StreakTrackerCard)d).UpdateUI();
        }

        private void UpdateUI()
        {
            try
            {
                if (Statistics == null)
                {
                    SetMainText("0");
                    return;
                }

                SetMainText(Statistics.WeekLoginStreak.ToString());
            }
            catch (FormatException fx)
            {
                Debug.WriteLine($"FormatException in StreakTrackerCard: {fx}");
                SetMainText("—");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in StreakTrackerCard: {ex}");
                SetMainText("—");
            }
        }

        private void SetMainText(string text)
        {
            var tb = FindLargeTextBlock(this);
            if (tb != null) tb.Text = text;
        }

        private static TextBlock? FindLargeTextBlock(DependencyObject parent)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var ch = VisualTreeHelper.GetChild(parent, i);
                if (ch is TextBlock t && t.FontSize >= 60) return t;
                var found = FindLargeTextBlock(ch);
                if (found != null) return found;
            }
            return null;
        }
    }
}
