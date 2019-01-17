using AdminApp.Windows;
using DbController;
using WPFRelectionControls.Controls;

namespace AdminApp.Controllers
{
    public class WindowsCreator
    {
        private readonly MsSqlContext _context;

        public WindowsCreator(MsSqlContext context)
        {
            _context = context;
        }

        public BasicWindow CreateReflectionWindow(object entity)
        {
            var control = new BasicInstanceControl();
            control.UpdateControl(entity, _context.GetListValuesByType);
            var wnd = new BasicWindow(control);
            return wnd;
        }        
    }
}
