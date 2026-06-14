using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AristotelisThesis.Domain.Models;

namespace AristotelisThesis.WPF.Controls
{
    /// <summary>
    /// Interaction logic for WeeklyAverageActiveTimeCard.xaml
    /// </summary>
    public partial class WeeklyAverageActiveTimeCard : UserControl
    {
        public static readonly DependencyProperty StatisticsProperty =
            DependencyProperty.Register(nameof(Statistics), typeof(AttendanceStatistics), typeof(WeeklyAverageActiveTimeCard),
                new PropertyMetadata(null, OnStatisticsChanged));

        public AttendanceStatistics Statistics
        {
            get => (AttendanceStatistics)GetValue(StatisticsProperty);
            set => SetValue(StatisticsProperty, value);
        }

        public WeeklyAverageActiveTimeCard()
        {
            InitializeComponent();
            Loaded += (_, __) => UpdateUI();
        }

        private static void OnStatisticsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((WeeklyAverageActiveTimeCard)d).UpdateUI();
        }

        private void UpdateUI()
        {
            try
            {
                if (Statistics == null)
                {
                    SetMainText("00", "00");
                    return;
                }

                var avg = Statistics.WeeklyAverageActiveTime;
                SetMainText(((int)avg.TotalHours).ToString("D2"), avg.Minutes.ToString("D2"));
            }
            catch (FormatException fx)
            {
                Debug.WriteLine($"FormatException in WeeklyAverageActiveTimeCard: {fx}");
                SetMainText("—", "—");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in WeeklyAverageActiveTimeCard: {ex}");
                SetMainText("—", "—");
            }
        }

        private void SetMainText(string hours, string minutes)
        {
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
