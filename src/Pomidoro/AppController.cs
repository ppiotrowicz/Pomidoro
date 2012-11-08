using System;

namespace Pomidoro
{
    public class AppController : NotifyPropertyChangedBase
    {
        //private readonly TimeSpan WorkDuration = TimeSpan.FromMinutes(25);
        //private readonly TimeSpan BreakDuration = TimeSpan.FromMinutes(5);

        private readonly TimeSpan WorkDuration = TimeSpan.FromSeconds(10);
        private readonly TimeSpan BreakDuration = TimeSpan.FromSeconds(5);

        public AppController()
        {
            StartWork();
        }

        public void StartWork()
        {
            Mode = "Work";
            CurrentTimer = new TimerViewModel(WorkDuration);
            CurrentTimer.Finished += WorkFinished;
            CurrentTimer.Start();
        }

        private void WorkFinished(object sender, EventArgs e)
        {
            CurrentTimer.Finished -= WorkFinished;
            StartBreak();
        }

        public void StartBreak()
        {
            Mode = "Break";
            CurrentTimer = new TimerViewModel(BreakDuration);
            CurrentTimer.Finished += BreakFinished;
            CurrentTimer.Start();
        }

        private void BreakFinished(object sender, EventArgs e)
        {
            CurrentTimer.Finished -= BreakFinished;
            StartWork();
        }

        private string _mode;
        public string Mode
        {
            get { return _mode; }
            set
            {
                _mode = value;
                NotifyPropertyChanged("Mode");
            }
        }

        private TimerViewModel _currentTimer;
        public TimerViewModel CurrentTimer
        {
            get { return _currentTimer; }
            set
            {
                _currentTimer = value;
                NotifyPropertyChanged("CurrentTimer");
            }
        }
    }
}