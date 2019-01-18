using System.Web.Mvc;
using BordelServerApp.Models;
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

        [HttpGet]
        public ActionResult Buy(int id)
        {
            ViewBag.WhoreId = id;
            return View();
        }
        [HttpPost]
        public string Buy(OrderModel model)
        {
            var customerId = _db.GetCustomerIdByName(model.CustomerName);
            if (customerId == null)
            {
                _db.AddCustomer(model.CustomerName);
            }

            customerId = _db.GetCustomerIdByName(model.CustomerName);
            var order = new Order
            {
                 WhoreId = model.WhoreId,
                CustomerId = customerId
            };
            _db.AddOrder(order);
            return "Ваш заказ принят";
        }
    }
}