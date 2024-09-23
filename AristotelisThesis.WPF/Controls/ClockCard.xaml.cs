using System;
using System.Windows.Controls;
using System.Windows.Threading;
using System.ComponentModel;

namespace AristotelisThesis.WPF.Controls
{
    public partial class ClockCard : UserControl, INotifyPropertyChanged
    {
        private DispatcherTimer _timer;
        private TimeSpan _sessionDuration;

        public event PropertyChangedEventHandler PropertyChanged;

        private string _sessionTime;
        public string SessionTime
        {
            get { return _sessionTime; }
            set
            {
                _sessionTime = value;
                OnPropertyChanged(nameof(SessionTime));
            }
        }

        public ClockCard()
        {
            InitializeComponent();
            DataContext = this; // Set DataContext for data binding

            _sessionDuration = TimeSpan.Zero;
            SessionTime = _sessionDuration.ToString(@"hh\:mm\:ss");

            StartSessionTimer();
        }

        private void StartSessionTimer()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1) // Set the timer to tick every second
            };

            _timer.Tick += (s, e) =>
            {
                _sessionDuration = _sessionDuration.Add(TimeSpan.FromSeconds(1));
                SessionTime = _sessionDuration.ToString(@"hh\:mm\:ss"); // Update session time
            };

            _timer.Start(); // Start the timer
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
