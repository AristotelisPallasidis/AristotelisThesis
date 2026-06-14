using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AristotelisThesis.Domain.Models;

namespace AristotelisThesis.WPF.Controls
{
    /// <summary>
    /// Interaction logic for AttendanceMonthPercentageCard.xaml
    /// </summary>
    public partial class AttendanceMonthPercentageCard : UserControl
    {
        public static readonly DependencyProperty StatisticsProperty =
            DependencyProperty.Register(nameof(Statistics), typeof(AttendanceStatistics), typeof(AttendanceMonthPercentageCard),
                new PropertyMetadata(null, OnStatisticsChanged));

        public AttendanceStatistics Statistics
        {
            get => (AttendanceStatistics)GetValue(StatisticsProperty);
            set => SetValue(StatisticsProperty, value);
        }

        public AttendanceMonthPercentageCard()
        {
            InitializeComponent();
            Loaded += (_, __) => UpdateUI();
        }

        private static void OnStatisticsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AttendanceMonthPercentageCard)d).UpdateUI();
        }

        private void UpdateUI()
        {
            try
            {
                if (Statistics == null)
                {
                    SetMainText("—");
                    return;
                }

                var perc = Math.Round(Statistics.MonthlyAttendancePercentage);
                SetMainText($"{perc}%");
            }
            catch (FormatException fx)
            {
                Debug.WriteLine($"FormatException in AttendanceMonthPercentageCard: {fx}");
                SetMainText("—");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in AttendanceMonthPercentageCard: {ex}");
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
