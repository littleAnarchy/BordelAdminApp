using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using AdminApp.ViewModels;
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

        public BasicAddingWindow CreateAddingWindow<T>()
        {
            var properties = typeof(T).GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(DynamicExtractable)));
            var wnd = new BasicAddingWindow(typeof(T));
            var view = wnd.FormsPanel;
            foreach (var property in properties)
            {
                view.Children.Add(new Label {Content = property.Name});
                if (!property.GetGetMethod().IsVirtual)
                    view.Children.Add(new TextBox{Name = property.Name});
                else
                {
                    var items = _context.GetListValuesByType(Type.GetType($"AdminApp.Models.{property.PropertyType.FullName}Model"));
                    view.Children.Add(new ComboBox{ ItemsSource = items });
                }
            }

            return wnd;
        }
    }
}
