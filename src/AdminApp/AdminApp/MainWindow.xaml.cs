using AdminApp.ViewModels;

namespace AdminApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            MainGrid.DataContext = new MainViewModel();
        }
    }
}
