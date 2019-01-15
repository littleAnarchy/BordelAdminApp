using System;
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

        public Action Btn2Cmd { get; set; }

        #region CustomProperties

        public string Btn2Content
        {
            get
            {
                return Btn2.Content.ToString();
            }
            set { Btn2.Content = value; }
        }

        //public string Btn2Content
        //{
        //    get { return (string)GetValue(Btn2ContentProperty); }
        //    set { SetValue(Btn2ContentProperty, value); }
        //}

        //public static readonly DependencyProperty Btn2ContentProperty = DependencyProperty.Register(
        //    "Btn2Content",
        //    typeof(string),
        //    typeof(BasicInstanceControl),
        //    new PropertyMetadata(""));
        #endregion

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

        private void Btn2_OnClick(object sender, RoutedEventArgs e)
        {
            Btn2Cmd.Invoke();
        }
    }
}
