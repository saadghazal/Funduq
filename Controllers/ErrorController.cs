using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using funduq.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace funduq.Controllers
{

   
    public class ErrorController : Controller
    {
        private readonly HotelReservationContext _context;

        public ErrorController(HotelReservationContext context) {
            _context = context;
        }

        public void GetHomePageData()
        {
            var homeData = _context.HomePages.Include(h => h.Contact).FirstOrDefault();
            if (homeData == null)
            {
                ViewBag.Logo = "~/AdminDesign/img/logo.png";
                ViewBag.CompanyName = "Funduq";
                ViewBag.Favicon = "";
            }
            else
            {

                ViewBag.Favicon = Url.Content("~/FaviconImages/" + homeData.HomeFavicon);




            }
        }
            // GET: /<controller>/
            public IActionResult Index()
        {
            GetHomePageData();
            return View();
        }
    }
}

