using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace AristotelisThesis.WPF.Controls
{
    public partial class ClockCard : UserControl
    {
        // Live "how long am I logged in" clock. It counts from the shared login moment
        // (LoginTime), so it shows the same elapsed time on every page of the app.
        private readonly DispatcherTimer _timer;

        public static readonly DependencyProperty LoginTimeProperty =
            DependencyProperty.Register(nameof(LoginTime), typeof(DateTime?), typeof(ClockCard),
                new PropertyMetadata(null, OnLoginTimeChanged));

        public DateTime? LoginTime
        {
            get => (DateTime?)GetValue(LoginTimeProperty);
            set => SetValue(LoginTimeProperty, value);
        }

        public ClockCard()
        {
            InitializeComponent();

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += (_, __) => Render();

            Loaded += (_, __) => UpdateUI();
            Unloaded += (_, __) => _timer.Stop();
        }

        private static void OnLoginTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ClockCard)d).UpdateUI();
        }

        private void UpdateUI()
        {
            try
            {
                if (LoginTime == null)
                {
                    _timer.Stop();
                    SessionTimeTextBlock.Text = "—";
                    return;
                }

                Render();
                _timer.Start();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in ClockCard: {ex}");
                _timer.Stop();
                SessionTimeTextBlock.Text = "—";
            }
        }

        private void Render()
        {
            DateTime? start = LoginTime;
            if (start == null)
            {
                _timer.Stop();
                SessionTimeTextBlock.Text = "—";
                return;
            }

            TimeSpan elapsed = DateTime.Now - start.Value;
            if (elapsed < TimeSpan.Zero)
            {
                elapsed = TimeSpan.Zero;
            }

            // Live duration since login, e.g. "00:05:21"
            SessionTimeTextBlock.Text = $"{(int)elapsed.TotalHours:D2}:{elapsed.Minutes:D2}:{elapsed.Seconds:D2}";
        }
    }
}
