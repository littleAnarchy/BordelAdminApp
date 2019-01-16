using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
        public object Entity { get; set; }
        public Action Btn2Cmd { get; set; }
        public WorkTypes WorkType { get; set; }

        #region CustomProperties

        public string Btn2Content
        {
            get => Btn2.Content.ToString();
            set => Btn2.Content = value;
        }

        #endregion

        public BasicInstanceControl(Window owner)
        {
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

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }


        public void UpdateChangingControl(object entity, Func<Type, List<object>> dataGrabber)
        {
            FormsPanel.Children.Clear();
            var properties = entity.GetType().GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(DynamicExtractable)));

            foreach (var property in properties)
            {
                FormsPanel.Children.Add(new Label { Content = property.Name });
                if (!property.GetGetMethod().IsVirtual)
                {
                    var text = entity.GetType().GetProperty(property.Name)?.GetValue(entity, null);
                    FormsPanel.Children.Add(new TextBox
                    {
                        Name = property.Name,
                        Text = text?.ToString() ?? ""
                    });
                }
                else
                {
                    var items = dataGrabber.Invoke(property.PropertyType);
                    var selectedItem = property.GetValue(entity, null);
                    FormsPanel.Children.Add(new ComboBox
                    {
                        Name = property.Name,
                        ItemsSource = items,
                        SelectedIndex = items.FindIndex(x => selectedItem != null && ((IIdentable)x).Id == (int?)selectedItem.GetType().GetProperty("Id")?.GetValue(selectedItem))
                    });
                }
            }
        }

        public void AddEntitytoDb()
        {
            Entity = Activator.CreateInstance(EntityType);
            var properties = Entity.GetType().GetProperties()
                .Where(prop => Attribute.IsDefined((MemberInfo)prop, typeof(DynamicExtractable)));

            var writedForms = new Dictionary<string, object>();

            //Adding content from text boxes
            foreach (var tb in Enumerable.Where<TextBox>(VisualFinder.FindVisualChildren<TextBox>(this), t => t.Name != null))
            {
                var oProp = Entity.GetType().GetProperty(tb.Name);
                if (oProp == null) continue;
                var tProp = oProp.PropertyType;

                if (tProp.IsGenericType
                    && tProp.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    tProp = new NullableConverter(oProp.PropertyType).UnderlyingType;
                }

                writedForms.Add(tb.Name, Convert.ChangeType(tb.Text, tProp));
            }

            //Adding content from Combo boxes
            foreach (var tb in Enumerable.Where<ComboBox>(VisualFinder.FindVisualChildren<ComboBox>(this), t => t.Name != null))
            {
                var oProp = Entity.GetType().GetProperty(tb.Name);
                if (oProp == null) continue;

                writedForms.Add(tb.Name, ((IEntityKeepable)tb.SelectedItem).Entity);
            }

            //Form object with grabbed parameters
            foreach (var property in properties)
            {
                Entity.GetType().InvokeMember(property.Name,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
                    Type.DefaultBinder, Entity, new[] { writedForms[property.Name] });
            }

            Owner.DialogResult = true;
        }
    }
}
