using System;
using System.Windows.Threading;

namespace Pomidoro
{
    public class TimerViewModel : NotifyPropertyChangedBase
    {
        private DispatcherTimer _timer;

        public TimerViewModel(TimeSpan timeBox)
        {
            TimeBox = timeBox;
            RemainingTime = timeBox;

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1.0d),
            };

            _timer.Tick += OnTimerTick;
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            RemainingTime -= TimeSpan.FromSeconds(1.0d);

            Value = ((TimeBox - RemainingTime).TotalSeconds/TimeBox.TotalSeconds);

            if (RemainingTime == TimeSpan.Zero)
            {
                _timer.Stop();
                Finished(this, EventArgs.Empty);
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

        public TimeSpan TimeBox { get; set; }

        private TimeSpan _remainingTime;
        public TimeSpan RemainingTime
        {
            get { return _remainingTime; }
            set
            {
                _remainingTime = value;
                NotifyPropertyChanged("RemainingTime");
            }
        }

        public void Start()
        {
            _timer.Start();
        }

        public event EventHandler Finished = delegate {};
    }
}