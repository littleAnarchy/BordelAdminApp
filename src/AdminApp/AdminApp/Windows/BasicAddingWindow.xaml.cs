using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using AdminApp.Common;
using Common;

namespace AdminApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для BasicAddingWindow.xaml
    /// </summary>
    public partial class BasicAddingWindow
    {
        private readonly Type _entityType;

        public object Entity { get; private set; }

        public BasicAddingWindow(Type entityType)
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

            foreach (var tb in VisualFinder.FindVisualChildren<TextBox>(this))
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
