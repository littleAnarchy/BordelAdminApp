using AdminApp.ViewModels.Reflection;

namespace AdminApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для BasicWindow.xaml
    /// </summary>
    public partial class BasicWindow
    {
        private readonly BasicReflectionViewModel _vm;

        public BasicWindow(BasicReflectionViewModel vm)
        {
            _vm = vm;
            DataContext = vm;
            vm.Initialize(this);
            InitializeComponent();
            BasicWindowCreator.UpdateMetroWindow(this);
        }

        public object GetEntity()
        {
            return _vm.Entity;
        }
    }
}
