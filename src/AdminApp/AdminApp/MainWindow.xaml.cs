using AdminApp.ViewModels;
using AdminApp.Windows;

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
            BasicWindowCreator.UpdateMetroWindow(this);
            MainGrid.DataContext = new MainViewModel();
        }
    }
}
