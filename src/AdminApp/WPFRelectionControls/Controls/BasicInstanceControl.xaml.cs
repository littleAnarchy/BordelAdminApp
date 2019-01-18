using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WPFRelectionControls.Common;
using WPFRelectionControls.Interfaces;

namespace WPFRelectionControls.Controls
{
    /// <summary>
    /// Interaction logic for BasicInstanceControl.xaml
    /// </summary>
    public partial class BasicInstanceControl
    {
        public Window Owner { get; set; }
        public Type EntityType { get; set; }
        private object _entity;
        public object Entity { get => _entity;
            set
            {
                _entity = value;
                if (_entity != null)
                    Btn2.IsEnabled = true;
            }
        }
        public Action<object> Btn2Cmd { get; set; }
        public event EventHandler OnBtn2Click;

        public bool IsAsync { get; set; }

        #region CustomProperties

        public string Btn2Content
        {
            get => Btn2.Content.ToString();
            set => Btn2.Content = value;
        }

        #endregion

        public BasicInstanceControl(Window owner, Action<object> btn2Cmd)
        {
            Owner = owner;
            Btn2Cmd = btn2Cmd;

            InitializeComponent();
        }

        public BasicInstanceControl()
        {
            InitializeComponent();
        }

        private async void Btn2_OnClick(object sender, RoutedEventArgs e)
        {
            FormOutEntity();

            if (IsAsync)
            {

                var task = new Task(() =>
                {
                    Btn2Cmd.Invoke(Entity);

                });
                task.Start();
                await task;
            }
            else
            {
                Btn2Cmd.Invoke(Entity);
            }
            OnBtn2Click?.Invoke(this, null);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Owner?.Close();
        }



        public void UpdateControl(object entity, Func<Type, List<object>> dataGrabber)
        {
            if (entity == null) return;
            FormsPanel.Children.Clear();
            Entity = entity;
            var properties = entity.GetType().GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(DynamicExtractable)));

            foreach (var property in properties)
            {
                FormsPanel.Children.Add(new Label { Content = property.Name });
                if (!property.GetGetMethod().IsVirtual)
                {
                    var text = entity.GetType().GetProperty(property.Name)?.GetValue(entity, null);
                    var textBox = new TextBox
                    {
                        Name = property.Name,
                        Text = text?.ToString() ?? ""
                    };
                    FormsPanel.Children.Add(textBox);
                }
                else
                {
                    var items = dataGrabber.Invoke(property.PropertyType);
                    var selectedItem = property.GetValue(entity, null);
                    var comboBox = new ComboBox
                    {
                        Name = property.Name,
                        ItemsSource = items,
                        SelectedIndex = items.FindIndex(x =>
                            selectedItem != null && ((IIdentable) x).Id ==
                            (int?) selectedItem.GetType().GetProperty("Id")?.GetValue(selectedItem))
                    };
                    FormsPanel.Children.Add(comboBox);
                }
            }
        }

        public object FormOutEntity()
        {
            var properties = Entity.GetType().GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(DynamicExtractable)));

            var writedForms = new Dictionary<string, object>();

            //Adding content from text boxes

            foreach (var tb in VisualFinder.FindVisualChildren<TextBox>(this).Where(t => t.Name != null))
            {
                var oProp = Entity.GetType().GetProperty(tb.Name);
                if (oProp == null) continue;
                var tProp = oProp.PropertyType;

                if (tProp.IsGenericType
                    && tProp.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    tProp = new NullableConverter(oProp.PropertyType).UnderlyingType;
                }

                writedForms.Add(tb.Name,
                    tb.Text != "" ? Convert.ChangeType(tb.Text, tProp) : Activator.CreateInstance(tProp));
            }

            //Adding content from Combo boxes
            foreach (var tb in VisualFinder.FindVisualChildren<ComboBox>(this).Where(t => t.Name != null))
            {
                var oProp = Entity.GetType().GetProperty(tb.Name);
                if (oProp == null) continue;

                writedForms.Add(tb.Name, tb.SelectedItem);
            }

            //Form object with grabbed parameters
            foreach (var property in properties)
            {
                Entity.GetType().InvokeMember(property.Name,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
                    Type.DefaultBinder, Entity, new[] { writedForms[property.Name] });
            }

            return Entity;
        }
    }
}
