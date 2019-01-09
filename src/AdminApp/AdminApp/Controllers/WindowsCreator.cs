using System;
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
        public static BasicAddingWindow CreateWhoesAddingWindow()
        {
            var properties = typeof(Whore).GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(DynamicExtractable)));
            var wnd = new BasicAddingWindow(typeof(Whore));
            var view = wnd.FormsPanel;
            foreach (var property in properties)
            {
                view.Children.Add(new Label {Content = property.Name});
                view.Children.Add(new TextBox{Name = property.Name});
            }

            return wnd;
        }
    }
}
