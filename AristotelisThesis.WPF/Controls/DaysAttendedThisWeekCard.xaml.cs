using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AristotelisThesis.Domain.Models;

namespace AristotelisThesis.WPF.Controls
{
    /// <summary>
    /// Interaction logic for DaysAttendedThisWeekCard.xaml
    /// </summary>
    public partial class DaysAttendedThisWeekCard : UserControl
    {
        public static readonly DependencyProperty StatisticsProperty =
            DependencyProperty.Register(nameof(Statistics), typeof(AttendanceStatistics), typeof(DaysAttendedThisWeekCard),
                new PropertyMetadata(null, OnStatisticsChanged));

        public AttendanceStatistics Statistics
        {
            get => (AttendanceStatistics)GetValue(StatisticsProperty);
            set => SetValue(StatisticsProperty, value);
        }

        public DaysAttendedThisWeekCard()
        {
            InitializeComponent();
            Loaded += (_, __) => UpdateUI();
        }

        private static void OnStatisticsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DaysAttendedThisWeekCard)d).UpdateUI();
        }

        private void UpdateUI()
        {
            try
            {
                if (Statistics == null)
                {
                    SetMainText("0", "/7");
                    return;
                }

                var days = Statistics.DaysAttendedThisWeek;
                SetMainText(days.ToString(), "/7");
            }
            catch (FormatException fx)
            {
                Debug.WriteLine($"FormatException in DaysAttendedThisWeekCard: {fx}");
                SetMainText("—", "/7");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in DaysAttendedThisWeekCard: {ex}");
                SetMainText("—", "/7");
            }
        }

        private void SetMainText(string days, string slash)
        {
            var tbs = FindLargeTextBlocks(this, 2);
            if (tbs.Length >= 1) tbs[0].Text = days;
            if (tbs.Length >= 2) tbs[1].Text = slash;
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
