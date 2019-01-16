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
        private readonly MsSqlContext _dataContext;

        public object SelectedItem { get; set; }

        public MainWindow()
        {  
            InitializeComponent();
            BasicWindowCreator.UpdateMetroWindow(this);
            _vm = new MainViewModel();
            MainGrid.DataContext = _vm;

            _dataContext = new MsSqlContext();
        }

        private void Selector_OnSelected(object sender, RoutedEventArgs e)
        {
            InstanceControl.UpdateChangingControl(_vm.SelectedObj, _dataContext.GetListValuesByType);
        }
    }
}
