using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using AdminApp.Common;
using AdminApp.Models;
using Common;

namespace AdminApp.ViewModels.Reflection
{
    // ReSharper disable once InconsistentNaming
    public class AddingReflectionVM : BasicReflectionViewModel
    {

        public AddingReflectionVM(Type type) : base(type, Activator.CreateInstance(type))
        {
            Btn2Cmd = new CommandHandler(OnAddClick, true);
        }

        public void OnAddClick()
        {
            Entity = Activator.CreateInstance(EntityType);
            var properties = Entity.GetType().GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(DynamicExtractable)));

            var writedForms = new Dictionary<string, object>();

            //Adding content from text boxes
            foreach (var tb in VisualFinder.FindVisualChildren<TextBox>(Owner).Where(t => t.Name != null))
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
            foreach (var tb in VisualFinder.FindVisualChildren<ComboBox>(Owner).Where(t => t.Name != null))
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
