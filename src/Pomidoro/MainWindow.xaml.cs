using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

namespace Pomidoro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1.0d),
            };

            _timer.Tick += OnTimerTick;

            _timer.IsEnabled = true;
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            Value += 0.05d;

            if (Value >= 1.0d)
            {
                //finish
                _timer.IsEnabled = false;
            }
        }

        private double _value;
        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
                NotifyPropertyChanged("Value");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}
