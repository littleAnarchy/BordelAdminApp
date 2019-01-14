using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using AdminApp.Common;
using AdminApp.Models;
using Common;

namespace AdminApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для BasicWindow.xaml
    /// </summary>
    public partial class BasicWindow
    {
        private readonly Type _entityType;

        public object Entity { get; private set; }

        public BasicWindow(Type entityType)
        {
            _entityType = entityType;

            InitializeComponent();
            BasicWindowCreator.UpdateMetroWindow(this);
        }

        private void OnAddClick(object sender, RoutedEventArgs e)
        {
            Entity = Activator.CreateInstance(_entityType);
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
                
                writedForms.Add(tb.Name, Convert.ChangeType(tb.Text, tProp));
            }

            //Adding content from Combo boxes
            foreach (var tb in VisualFinder.FindVisualChildren<ComboBox>(this).Where(t => t.Name != null))
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
                    Type.DefaultBinder, Entity, new []{ writedForms[property.Name] });
            }

            DialogResult = true;
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
