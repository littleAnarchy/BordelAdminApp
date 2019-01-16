using System;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using AdminApp.Windows;
using DbController;
using WPFRelectionControls;
using WPFRelectionControls.Reflection;

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
                .Where(prop => Attribute.IsDefined((MemberInfo) prop, typeof(DynamicExtractable)));
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
            //UpdateChangingControl(entity, view);
            return wnd;
        }

        
    }
}
