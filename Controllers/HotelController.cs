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
    public class HotelController : Controller
    {
        private readonly HotelReservationContext _context;
        private readonly IWebHostEnvironment _environment;

        public HotelController(HotelReservationContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
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

        // GET: Hotel
        public  IActionResult Index()
        {
            return RedirectToAction("Hotels", "Admin");
            //var hotelReservationContext = _context.Hotels.Include(h => h.HotelCityNavigation);
            //return View(await hotelReservationContext.ToListAsync());
        }

        // GET: Hotel/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            GetSessionProperties();

            ViewBag.SelectedSidebar = "Hotels";
            
            if (id == null || _context.Hotels == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotels
                .Include(h => h.HotelCityNavigation)
                .FirstOrDefaultAsync(m => m.HotelId == id);
            if (hotel == null)
            {
                return NotFound();
            }
            return View(hotel);
        }

        // GET: Hotel/Create
        public IActionResult Create()
        {
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            ViewBag.SelectedSidebar = "Hotels";
            ViewData["HotelCity"] = new SelectList(_context.Cities, "CityId", "CityName");
            GetSessionProperties();
            return View();
        }

        // POST: Hotel/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HotelId,HotelName,HotelCity,NumberOfStars,IsFeaturedHotel,HotelImage")] Hotel hotel)
        {
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            ViewBag.SelectedSidebar = "Hotels";
            GetSessionProperties();
            var isExist = _context.Hotels.Where(h => h.HotelName == hotel.HotelName).Count() != 0;
            if (isExist)
            {
                TempData["Alert"] = "Hotel Already Exist.";
                return View(hotel);
            }
            if (ModelState.IsValid)
            {

                if (hotel.HotelImage != null)
                {

                    string w3RootPath = _environment.WebRootPath;// it returns the path of the wwwroot Folder
                    string fileName = Guid.NewGuid().ToString() + "_" + hotel.HotelImage.FileName; // the name of the file of the uploaded image
                    string newPath = Path.Combine(w3RootPath + "/HotelsImages/", fileName);

                    using (var fileStream = new FileStream(newPath, FileMode.Create))
                    {
                        await hotel.HotelImage.CopyToAsync(fileStream);

                    }

                    hotel.HotelPicture = fileName;
                }

                _context.Add(hotel);
                await _context.SaveChangesAsync();
                return RedirectToAction("Hotels", "Admin");
            }
            ViewData["HotelCity"] = new SelectList(_context.Cities, "CityId", "CityName", hotel.HotelCity);
            return View(hotel);
        }

        // GET: Hotel/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            ViewBag.SelectedSidebar = "Hotels";
            GetSessionProperties();

            if (id == null || _context.Hotels == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }
            
            ViewData["HotelCity"] = new SelectList(_context.Cities, "CityId", "CityName", hotel.HotelCity);
            return View(hotel);
        }

        // POST: Hotel/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HotelId,HotelName,HotelImage,HotelCity,NumberOfStars,IsFeaturedHotel")] Hotel hotel)
        {
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            ViewBag.SelectedSidebar = "Hotels";
            GetSessionProperties();

            if (id != hotel.HotelId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (hotel.HotelImage != null)
                    {

                        string w3RootPath = _environment.WebRootPath;// it returns the path of the wwwroot Folder
                        string fileName = Guid.NewGuid().ToString() + "_" + hotel.HotelImage.FileName; // the name of the file of the uploaded image
                        string newPath = Path.Combine(w3RootPath + "/HotelsImages/", fileName);

                        using (var fileStream = new FileStream(newPath, FileMode.Create))
                        {
                            await hotel.HotelImage.CopyToAsync(fileStream);

                        }

                        hotel.HotelPicture = fileName;
                    }
                    _context.Update(hotel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HotelExists(hotel.HotelId))
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
            ViewData["HotelCity"] = new SelectList(_context.Cities, "CityId", "CityName", hotel.HotelCity);
            return View(hotel);
        }

        // GET: Hotel/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            ViewBag.SelectedSidebar = "Hotels";
            GetSessionProperties();

            if (id == null || _context.Hotels == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotels
                .Include(h => h.HotelCityNavigation)
                .FirstOrDefaultAsync(m => m.HotelId == id);
            if (hotel == null)
            {
                return NotFound();
            }
            return View(hotel);
        }

        // POST: Hotel/Delete/5
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

            if (_context.Hotels == null)
            {
                return Problem("Entity set 'HotelReservationContext.Hotels'  is null.");
            }
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel != null)
            {
                _context.Hotels.Remove(hotel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Hotels", "Admin");
        }

        private bool HotelExists(int id)
        {
            return (_context.Hotels?.Any(e => e.HotelId == id)).GetValueOrDefault();
        }
    }
}
