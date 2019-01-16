using System.Windows;
using System.Windows.Controls;
using AdminApp.Controllers;
using AdminApp.ViewModels;
using AdminApp.Windows;
using DbController;

namespace AdminApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly MainViewModel _vm;
        private readonly StackPanel _selectedItemPanel;
        private readonly WindowsCreator _viewController;
        private readonly MsSqlContext _dataContext;

        public object SelectedItem { get; set; }

        public MainWindow()
        {  
            InitializeComponent();
            BasicWindowCreator.UpdateMetroWindow(this);
            _vm = new MainViewModel();
            MainGrid.DataContext = _vm;

            _dataContext = new MsSqlContext();
            _selectedItemPanel = InstanceControl.FormsPanel;
            _viewController = new WindowsCreator(_dataContext);
        }

        private void Selector_OnSelected(object sender, RoutedEventArgs e)
        {
            _viewController.UpdateChangingControl(_vm.SelectedObj, _selectedItemPanel);
        }
    }
}
