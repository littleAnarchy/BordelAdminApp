using System.Windows;
using AdminApp.ViewModels;
using AdminApp.Windows;
using DbController;
using MahApps.Metro.Controls;

namespace AdminApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
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
            InstanceControl.OnBtn2Click += (sender, args) => _vm.UpdateView();

            _dataContext = new MsSqlContext();
        }

        private void Selector_OnSelected(object sender, RoutedEventArgs e)
        {
            if (_vm.SelectedObj == null) return;
            InstanceControl.UpdateControl(_vm.SelectedObj, _dataContext.GetListValuesByType);
            InstanceControl.Btn2Cmd = _dataContext.GetSavingMethodByType(_vm.SelectedObj.GetType().BaseType);
        }
    }
}
