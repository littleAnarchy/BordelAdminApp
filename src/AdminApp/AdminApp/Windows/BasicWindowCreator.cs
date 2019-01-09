using System.Windows.Media;
using MahApps.Metro.Controls;

namespace AdminApp.Windows
{
    public class BasicWindowCreator : MetroWindow
    {
        public static void UpdateMetroWindow(MetroWindow wnd)
        {
            wnd.Background = Brushes.SlateBlue;
            wnd.WindowTitleBrush = Brushes.SlateBlue;
        }
    }
}
