using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using funduq.Models;

namespace funduq.Controllers
{
    public class HotelServiceController : Controller
    {
        private readonly HotelReservationContext _context;

        public HotelServiceController(HotelReservationContext context)
        {
            _context = context;
        }
        public void GetSessionProperties()
        {
            ViewBag.userId = HttpContext.Session.GetInt32("admin_user_id");
            ViewBag.userFName = HttpContext.Session.GetString("admin_firstName");
            ViewBag.userLName = HttpContext.Session.GetString("admin_lastName");
            ViewBag.userPhoneNumber = HttpContext.Session.GetString("admin_phoneNumber");
            ViewBag.userProfilePicture = HttpContext.Session.GetString("admin_profilePicture");
            ViewBag.userEmail = HttpContext.Session.GetString("admin_email");
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

        // GET: HotelService
        public async Task<IActionResult> Index()
        {
            var hotelReservationContext = _context.HotelServices.Include(h => h.Hotel);
            return View(await hotelReservationContext.ToListAsync());
        }

        // GET: HotelService/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            ViewBag.SelectedSidebar = "Hotels";
            GetSessionProperties();

            if (id == null || _context.HotelServices == null)
            {
                return NotFound();
            }

            var hotelService = await _context.HotelServices
                .Include(h => h.Hotel)
                .FirstOrDefaultAsync(m => m.ServiceId == id);
            if (hotelService == null)
            {
                return NotFound();
            }
            
            return View(hotelService);
        }

        // GET: HotelService/Create
        public IActionResult Create()
        {
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            ViewBag.SelectedSidebar = "Hotels";
            ViewData["HotelId"] = new SelectList(_context.Hotels, "HotelId", "HotelName");
            GetSessionProperties();
            return View();
        }

        // POST: HotelService/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceId,ServiceType,HotelId")] HotelService hotelService)
        {
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            ViewBag.SelectedSidebar = "Hotels";
            GetSessionProperties();
            
            if (ModelState.IsValid)
            {
                _context.Add(hotelService);
                await _context.SaveChangesAsync();
                
                return RedirectToAction("Hotels", "Admin");
            }
            ViewData["HotelId"] = new SelectList(_context.Hotels, "HotelId", "HotelName", hotelService.HotelId);

            return View(hotelService);
        }

        // GET: HotelService/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            ViewBag.SelectedSidebar = "Hotels";
            GetSessionProperties();
            if (id == null || _context.HotelServices == null)
            {
                return NotFound();
            }

            var hotelService = await _context.HotelServices.FindAsync(id);
            if (hotelService == null)
            {
                return NotFound();
            }
            ViewData["HotelId"] = new SelectList(_context.Hotels, "HotelId", "HotelName", hotelService.HotelId);
            return View(hotelService);
        }

        // POST: HotelService/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServiceId,ServiceType,HotelId")] HotelService hotelService)
        {
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            ViewBag.SelectedSidebar = "Hotels";
            GetSessionProperties();
            if (id != hotelService.ServiceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hotelService);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HotelServiceExists(hotelService.ServiceId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Hotels", "Admin");
            }
            ViewData["HotelId"] = new SelectList(_context.Hotels, "HotelId", "HotelName", hotelService.HotelId);
            return View(hotelService);
        }

        // GET: HotelService/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            ViewBag.SelectedSidebar = "Hotels";
            GetSessionProperties();
            if (id == null || _context.HotelServices == null)
            {
                return NotFound();
            }

            var hotelService = await _context.HotelServices
                .Include(h => h.Hotel)
                .FirstOrDefaultAsync(m => m.ServiceId == id);
            if (hotelService == null)
            {
                return NotFound();
            }

            return View(hotelService);
        }

        // POST: HotelService/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            ViewBag.SelectedSidebar = "Hotels";
            GetSessionProperties();
            if (_context.HotelServices == null)
            {
                return Problem("Entity set 'HotelReservationContext.HotelServices'  is null.");
            }
            var hotelService = await _context.HotelServices.FindAsync(id);
            if (hotelService != null)
            {
                _context.HotelServices.Remove(hotelService);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Hotels","Admin");
        }

        private bool HotelServiceExists(int id)
        {
          return (_context.HotelServices?.Any(e => e.ServiceId == id)).GetValueOrDefault();
        }
    }
}
