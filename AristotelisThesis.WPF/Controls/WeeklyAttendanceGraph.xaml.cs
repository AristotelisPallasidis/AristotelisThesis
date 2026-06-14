using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AristotelisThesis.Domain.Models;

namespace AristotelisThesis.WPF.Controls
{
    /// <summary>
    /// Interaction logic for WeeklyAttendanceGraph.xaml
    /// </summary>
    public partial class WeeklyAttendanceGraph : UserControl
    {
        // Smallest visible bar height (px) so days with no activity still show a baseline stub.
        private const double MinBarHeight = 4.0;

        // The bars currently rendered, paired with their value in hours (for height scaling).
        private readonly List<(Border Bar, double Hours)> _bars = new();
        private double _maxHours;

        public static readonly DependencyProperty StatisticsProperty =
            DependencyProperty.Register(nameof(Statistics), typeof(AttendanceStatistics), typeof(WeeklyAttendanceGraph),
                new PropertyMetadata(null, OnStatisticsChanged));

        public AttendanceStatistics Statistics
        {
            get => (AttendanceStatistics)GetValue(StatisticsProperty);
            set => SetValue(StatisticsProperty, value);
        }

        public WeeklyAttendanceGraph()
        {
            InitializeComponent();
            Loaded += (_, __) => UpdateUI();
        }

        private static void OnStatisticsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((WeeklyAttendanceGraph)d).UpdateUI();
        }

        private void UpdateUI()
        {
            try
            {
                if (Statistics == null)
                {
                    PercentText.Text = "—";
                    BuildChart(null);
                    return;
                }

                var days = Statistics.DaysAttendedThisWeek;
                var percent = Math.Round((days / 7.0) * 100);
                PercentText.Text = $"{percent}%";
                PercentText.Foreground = ResolveBrush(percent >= 60 ? "ColorSuccess" : "ColorDanger", Brushes.OrangeRed);

                BuildChart(Statistics.WeeklyAttendanceGraph);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in WeeklyAttendanceGraph: {ex}");
                PercentText.Text = "—";
                BuildChart(null);
            }
        }

        private void BuildChart(ICollection<WeeklyAttendanceDataPoint> points)
        {
            BarsGrid.Children.Clear();
            BarsGrid.ColumnDefinitions.Clear();
            LabelsGrid.Children.Clear();
            LabelsGrid.ColumnDefinitions.Clear();
            _bars.Clear();

            if (points == null || points.Count == 0)
            {
                _maxHours = 0;
                return;
            }

            var ordered = points.OrderBy(p => p.Date).ToList();
            _maxHours = ordered.Max(p => p.ActiveTime.TotalHours);

            var attendedBrush = ResolveBrush("ColorSuccess", Brushes.MediumSeaGreen);
            var emptyBrush = new SolidColorBrush(Color.FromRgb(0xC9, 0xC9, 0xC9));

            for (int i = 0; i < ordered.Count; i++)
            {
                var point = ordered[i];
                double hours = point.ActiveTime.TotalHours;

                BarsGrid.ColumnDefinitions.Add(new ColumnDefinition());
                LabelsGrid.ColumnDefinitions.Add(new ColumnDefinition());

                var bar = new Border
                {
                    Background = hours > 0 ? attendedBrush : emptyBrush,
                    CornerRadius = new CornerRadius(4, 4, 0, 0),
                    Margin = new Thickness(6, 0, 6, 0),
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Height = MinBarHeight,
                    ToolTip = FormatDuration(point.ActiveTime)
                };
                Grid.SetColumn(bar, i);
                BarsGrid.Children.Add(bar);
                _bars.Add((bar, hours));

                var label = new TextBlock
                {
                    Text = GreekDayAbbreviation(point.Date.DayOfWeek),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontSize = 13,
                    Foreground = new SolidColorBrush(Color.FromRgb(0x55, 0x55, 0x55))
                };
                Grid.SetColumn(label, i);
                LabelsGrid.Children.Add(label);
            }

            UpdateBarHeights();
        }

        private void BarsGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateBarHeights();
        }

        private void UpdateBarHeights()
        {
            double available = BarsGrid.ActualHeight;
            if (available <= 0 || _bars.Count == 0)
            {
                return;
            }

            foreach (var (bar, hours) in _bars)
            {
                double ratio = _maxHours > 0 ? hours / _maxHours : 0;
                bar.Height = Math.Max(MinBarHeight, ratio * available);
            }
        }

        private static string FormatDuration(TimeSpan ts)
        {
            return $"{(int)ts.TotalHours:D2}:{ts.Minutes:D2}";
        }

        private static string GreekDayAbbreviation(DayOfWeek day) => day switch
        {
            DayOfWeek.Monday => "Δε",
            DayOfWeek.Tuesday => "Τρ",
            DayOfWeek.Wednesday => "Τε",
            DayOfWeek.Thursday => "Πε",
            DayOfWeek.Friday => "Πα",
            DayOfWeek.Saturday => "Σα",
            DayOfWeek.Sunday => "Κυ",
            _ => string.Empty
        };

        private Brush ResolveBrush(string resourceKey, Brush fallback)
        {
            return TryFindResource(resourceKey) as Brush ?? fallback;
        }
    }
}
