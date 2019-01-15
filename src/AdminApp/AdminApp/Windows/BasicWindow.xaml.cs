using System.Windows.Controls;
using AdminApp.Controls;
using AdminApp.ViewModels.Reflection;

namespace AdminApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для BasicWindow.xaml
    /// </summary>
    public partial class BasicWindow
    {
        private readonly BasicReflectionViewModel _vm;

        public StackPanel FormsPanel { get; }

        public BasicWindow(BasicReflectionViewModel vm)
        {
            _vm = vm;
            var control = new BasicInstanceControl(vm, this);
            FormsPanel = control.FormsPanel;
            DefaultControl.DataContext = control;
            InitializeComponent();
            BasicWindowCreator.UpdateMetroWindow(this);
        }

        public object GetEntity()
        {
            return _vm.Entity;
        }
    }
}
