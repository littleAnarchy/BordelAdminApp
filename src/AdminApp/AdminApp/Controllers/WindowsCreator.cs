using System;
using System.Linq;
using System.Windows.Controls;
using AdminApp.Controls;
using AdminApp.Models;
using AdminApp.ViewModels.Reflection;
using AdminApp.Windows;
using Common;
using Common.Interfaces;
using DbController;

namespace AdminApp.Controllers
{
    public class WindowsCreator
    {
        private readonly MsSqlContext _context;

        public WindowsCreator(MsSqlContext context)
        {
            _context = context;
        }

        public BasicWindow CreateAddingWindow<T>() where T : class
        {
            var properties = typeof(T).GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(DynamicExtractable)));
            var vm = new AddingReflectionVM(typeof(T));
            var wnd = new BasicWindow(vm);
            var view = wnd.FormsPanel;
            foreach (var property in properties)
            {
                view.Children.Add(new Label {Content = property.Name});
                if (!property.GetGetMethod().IsVirtual)
                    view.Children.Add(new TextBox{Name = property.Name});
                else
                {
                    var items = _context.GetListValuesByType(property.PropertyType);
                    view.Children.Add(new ComboBox{ Name = property.Name, ItemsSource = items });
                }
            }

            return wnd;
        }

        //Todo: refactor this methods
        public BasicWindow CreateChangingWindow<T>(object entity) where T : class
        {
            var vm = new AddingReflectionVM(typeof(T));
            var wnd = new BasicWindow(vm);
            var view = wnd.FormsPanel;
            UpdateChangingControl(entity, view);
            return wnd;
        }

        public void UpdateChangingControl(object entity, StackPanel view)
        {
            view.Children.Clear();
            var properties = entity.GetType().GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(DynamicExtractable)));

            foreach (var property in properties)
            {
                view.Children.Add(new Label { Content = property.Name });
                if (!property.GetGetMethod().IsVirtual)
                {
                    var text = entity.GetType().GetProperty(property.Name)?.GetValue(entity, null);
                    view.Children.Add(new TextBox
                    {
                        Name = property.Name,
                        Text = text?.ToString() ?? ""
                    });
                }
                else
                {
                    var items = _context.GetListValuesByType(property.PropertyType);
                    var selectedItem = property.GetValue(entity, null);
                    view.Children.Add(new ComboBox
                    {
                        Name = property.Name,
                        ItemsSource = items,
                        SelectedIndex = items.FindIndex(x => selectedItem != null && ((IIdentable)x).Id == (int?)selectedItem.GetType().GetProperty("Id")?.GetValue(selectedItem))
                    });
                }
            }
        }
    }
}
