using System;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;

namespace Pomidoro
{
    public class AppController : NotifyPropertyChangedBase
    {
        private readonly ISettingsProvider _settingsProvider;
        private readonly TimeSpan WorkDuration = TimeSpan.FromMinutes(25);
        private readonly TimeSpan BreakDuration = TimeSpan.FromMinutes(5);

        //private readonly TimeSpan WorkDuration = TimeSpan.FromSeconds(10);
        //private readonly TimeSpan BreakDuration = TimeSpan.FromSeconds(5);

        public AppController(ISettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
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

        public int Left { get; set; }
        public int Top { get; set; }

        protected void SaveCurrentPosition()
        {
            _settingsProvider.Left = Left;
            _settingsProvider.Top = Top;
        }

        protected void RestoreCurrentPosition()
        {
            Left = _settingsProvider.Left;
            Top = _settingsProvider.Top;
        }

        public void OnStartup()
        {
            RestoreCurrentPosition();
        }

        public void OnExit()
        {
            SaveCurrentPosition();
            _settingsProvider.Save();
        }
    }

    public class JsonConfiguration : ISettingsProvider
    {
        public const string FileName = "Settings.json";

        public int Left { get; set; }
        public int Top { get; set; }
        
        public void Load()
        {
            string data = null;

            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var file = Path.Combine(appDirectory, FileName);

            if (!File.Exists(file))
            {
                return;
            }

            using (var stream = File.OpenRead(file))
            using (var reader = new StreamReader(stream))
            {
                data = reader.ReadToEnd();
            }

            var json = JsonConvert.DeserializeObject<JsonConfiguration>(data);
            Left = json.Left;
            Top = json.Top;
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(new
            {
                Left, Top
            }, Formatting.Indented);

            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            using(var stream = new FileStream(Path.Combine(appDirectory, FileName), FileMode.Create))
            using(var writer = new StreamWriter(stream))
            {
                writer.Write(json);
            }
        }
    }

    public interface ISettingsProvider
    {
        int Left { get; set; }
        int Top { get; set; }

        void Load();
        void Save();
    }
}