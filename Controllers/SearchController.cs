using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using funduq.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace funduq.Controllers
{
    public class SearchController : Controller
    {
        readonly private HotelReservationContext _context;

        public SearchController(HotelReservationContext context) {
            _context = context;
        }

        public void GetHomePageData()
        {
            var homeData = _context.HomePages.FirstOrDefault();
            if (homeData == null)
            {
                ViewBag.Logo = "~/AdminDesign/img/logo.png";
                ViewBag.CompanyName = "Funduq";
                ViewBag.Favicon = "";
            }
            else
            {

                ViewBag.Logo = Url.Content("~/LogoImages/" + homeData.HomeLogoImage);
                ViewBag.Favicon = Url.Content("~/FaviconImages/" + homeData.HomeFavicon);
                ViewBag.CompanyName = homeData.HomeLogoText;
            }


        }
        [HttpGet]
        public IActionResult SearchedHotels(string? value) {
            if (value == null) {
                return RedirectToAction("HotelsPage", "Home");
            }
            GetHomePageData();
            var userId = HttpContext.Session.GetInt32("user_id");
            var hotels = _context.Hotels.Where(h => h.HotelName.Contains(value)).ToList();
            if (userId == null) {
                ViewBag.user = null;
                return View(hotels);
            }
            ViewBag.user = _context.UserProfiles.Where(u => u.UserId == userId).SingleOrDefault();
            return View(hotels);
        }
    }
}

