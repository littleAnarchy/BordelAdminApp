using System;
using System.Linq;
using System.Windows.Controls;
using AdminApp.Models;
using AdminApp.Windows;
using Common;
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
            var wnd = new BasicWindow(typeof(T));
            var view = wnd.FormsPanel;
            foreach (var property in properties)
            {
                view.Children.Add(new Label {Content = property.Name});
                if (!property.GetGetMethod().IsVirtual)
                    view.Children.Add(new TextBox{Name = property.Name});
                else
                {
                    var items = _context.GetListValuesByType(Type.GetType($"AdminApp.Models.{property.PropertyType.FullName}Model"));
                    view.Children.Add(new ComboBox{ Name = property.Name, ItemsSource = items });
                }
            }

            return wnd;
        }

        //Todo: refactor this methods
        public BasicWindow CreateChangingWindow<T>(object entity) where T : class
        {
            var properties = typeof(T).GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(DynamicExtractable)));
            var wnd = new BasicWindow(typeof(T));
            var view = wnd.FormsPanel;
            foreach (var property in properties)
            {
                view.Children.Add(new Label { Content = property.Name });
                if (!property.GetGetMethod().IsVirtual)
                    view.Children.Add(new TextBox {
                        Name = property.Name,
                        Text = entity.GetType().GetProperty(property.Name)?.GetValue(entity, null).ToString() ?? throw new InvalidOperationException()
                    });
                else
                {
                    var items = _context.GetListValuesByType(Type.GetType($"AdminApp.Models.{property.PropertyType.FullName}Model"));
                    var selectedItem = property.GetValue(entity, null);
                    view.Children.Add(new ComboBox
                    {
                        Name = property.Name,
                        ItemsSource = items,
                        SelectedIndex = items.FindIndex(x => selectedItem != null && ((IIdentable) x).Id == (int?) selectedItem.GetType().GetProperty("Id")?.GetValue(selectedItem))
                    });
                }
            }

            return wnd;
        }
    }
}
