using System.Windows;
using AdminApp.ViewModels.Reflection;

namespace AdminApp.Controls
{
    /// <summary>
    /// Interaction logic for BasicInstanceControl.xaml
    /// </summary>
    public partial class BasicInstanceControl
    {
        private readonly BasicReflectionViewModel _vm;

        public BasicInstanceControl(BasicReflectionViewModel vm, Window owner)
        {
            _vm = vm;
            DataContext = vm;
            vm.Initialize(owner);
            InitializeComponent();
        }

        public BasicInstanceControl()
        {
            InitializeComponent();
        }
    }
}
