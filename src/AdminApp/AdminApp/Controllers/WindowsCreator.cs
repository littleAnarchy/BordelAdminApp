using System;
using System.Windows;
using System.Windows.Controls;

namespace AdminApp.Controllers
{
    public class WindowsCreator
    {
        public static Window CreateWhoesAddingWindow()
        {
            var wnd = new Window{ Width = 300, SizeToContent = SizeToContent.Height};
            var view = new StackPanel{ Margin = new Thickness(20) };
            wnd.Content = view;
            foreach (var property in typeof(Whore).GetProperties())
            {
                view.Children.Add(new Label {Content = property.Name});
                view.Children.Add(new TextBox());
            }

            return wnd;
        }
    }
}
