using WPFRelectionControls.Controls;

namespace AdminApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для BasicWindow.xaml
    /// </summary>
    public partial class BasicWindow
    {
        private readonly BasicInstanceControl _control;

        public BasicWindow(BasicInstanceControl control)
        {
            _control = control;
            InitializeComponent();
            MainGrid.Children.Add(control);
            BasicWindowCreator.UpdateMetroWindow(this);
        }

        public object GetEntity()
        {
            return _control.Entity;
        }
    }
}
