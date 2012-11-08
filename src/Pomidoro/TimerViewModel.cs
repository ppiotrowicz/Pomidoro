using System;
using System.Windows.Threading;

namespace Pomidoro
{
    public class TimerViewModel : NotifyPropertyChangedBase
    {
        private DispatcherTimer _timer;

        public TimerViewModel()
        {
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
    }
}