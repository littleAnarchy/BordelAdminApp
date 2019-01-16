using System.Web.Mvc;
using DbController;

namespace BordelServerApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly MsSqlContext _db = new MsSqlContext();
        
        public ActionResult Index()
        {
            ViewBag.Whores = _db.GetWhores();
            return View();
        }
    }
}