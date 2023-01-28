using Microsoft.AspNetCore.Mvc;
using MDPActual.Models;

namespace MDPActual.Controllers
{
    public class TestController : Controller
    {
        /*public IActionResult Index()
        {
            Index(HomeController.items1);
            return View();
        }*/
        public IActionResult Index()
        {
            //List<Item> i = HomeController.items1;
            string currentItem = Request.Query["item"];
            List<Item> i = GetCurrentItem(currentItem);
            return View(i);
        }
        private List<Item> GetCurrentItem(string s)
        {
            int index = 0;
            List<Item> selectedItem = new List<Item>();
            List<Item> i = HomeController.items1;
            foreach (Item item in i)
                if (item.Email == s)
                {
                    selectedItem.Add(item);
                    break;
                }
            return selectedItem;
        }
    }
}
