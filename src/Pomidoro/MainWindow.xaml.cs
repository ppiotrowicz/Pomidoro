using System.Windows;
using System.Windows.Input;

namespace Pomidoro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AppController _appController;

        public MainWindow()
        {
            InitializeComponent();
            
            var configuration = new JsonConfiguration();
            configuration.Load();

            _appController = new AppController(configuration);

            _appController.OnStartup();

            DataContext = _appController;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            _appController.OnExit();
            base.OnClosing(e);
        }
    }
}
