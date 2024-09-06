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
    public class RoomController : Controller
    {
        private readonly HotelReservationContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RoomController(HotelReservationContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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
                ViewBag.Favicon = "";
                ViewBag.CompanyName = "Funduq";
            }
            else
            {
                ViewBag.Logo = Url.Content("~/LogoImages/" + homeData.HomeLogoImage);
                ViewBag.Favicon = Url.Content("~/FaviconImages/" + homeData.HomeFavicon);
                ViewBag.CompanyName = homeData.HomeLogoText;
            }
        }
        // GET: Room
        public async Task<IActionResult> Index()
        {
            var hotelReservationContext = _context.Rooms.Include(r => r.Hotel).Include(r => r.Type);
            return View(await hotelReservationContext.ToListAsync());
        }

        // GET: Room/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            ViewBag.SelectedSidebar = "Hotels";
            GetSessionProperties(); 
            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(r => r.Hotel)
                .Include(r => r.Type)
                .FirstOrDefaultAsync(m => m.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // GET: Room/Create
        public IActionResult Create()
        {
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            ViewBag.SelectedSidebar = "Hotels";
            GetSessionProperties();
            ViewData["HotelId"] = new SelectList(_context.Hotels, "HotelId", "HotelName");
            ViewData["TypeId"] = new SelectList(_context.RoomTypes, "TypeId", "RoomType1");
            return View();
        }

        public async Task<string> UploadImage(IFormFile roomImageFile) {

            string w3RootPath = _webHostEnvironment.WebRootPath;// it returns the path of the wwwroot Folder
            string fileName = Guid.NewGuid().ToString() + "_" + roomImageFile.FileName; // the name of the file of the uploaded image
            string newPath = Path.Combine(w3RootPath + "/RoomsImages/", fileName);

            using (var fileStream = new FileStream(newPath, FileMode.Create))
            {
                await roomImageFile.CopyToAsync(fileStream);

            }
            return fileName;
        }

        // POST: Room/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomId,TypeId,RoomName,HotelId,NumberOfRooms,IsBreakfastIncluded,NightPrice,RoomDescription,NumberOfBeds,RoomImageFile")] Room room)
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
                if (room.NightPrice <= 0 || room.NumberOfBeds <= 0)
                {
                    TempData["Alert"] = "Invalid Data Input";
                    return RedirectToAction("Create");
                }
                if (room.RoomImageFile != null)
                {


                    room.RoomImage = await UploadImage(room.RoomImageFile);
                }
                
                
                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction("Hotels", "Admin");
            }
            
            ViewData["HotelId"] = new SelectList(_context.Hotels, "HotelId", "HotelName", room.HotelId);
            ViewData["TypeId"] = new SelectList(_context.RoomTypes, "TypeId", "RoomType1", room.TypeId);
            return View(room);
        }

        // GET: Room/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            ViewBag.SelectedSidebar = "Hotels";
            GetSessionProperties();

            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            ViewData["HotelId"] = new SelectList(_context.Hotels, "HotelId", "HotelName", room.HotelId);
            ViewData["TypeId"] = new SelectList(_context.RoomTypes, "TypeId", "RoomType1", room.TypeId);
            return View(room);
        }

        // POST: Room/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoomId,TypeId,RoomName,HotelId,NumberOfRooms,NightPrice,RoomDescription,NumberOfBeds,RoomImageFile,IsAvailable,HasOffer,IsBreakfastIncluded")] Room room)
        {
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            ViewBag.SelectedSidebar = "Hotels";
            GetSessionProperties();
            if (room.NightPrice <= 0 || room.NumberOfBeds <= 0)
            {
                TempData["Alert"] = "Invalid Data Input";
                return RedirectToAction("Edit");
            }
            if (id != room.RoomId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (room.RoomImageFile != null)
                {


                    room.RoomImage = await UploadImage(room.RoomImageFile);
                }
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.RoomId))
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
            ViewData["HotelId"] = new SelectList(_context.Hotels, "HotelId", "HotelName", room.HotelId);
            ViewData["TypeId"] = new SelectList(_context.RoomTypes, "TypeId", "RoomType1", room.TypeId);
            return View(room);
        }

        // GET: Room/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            ViewBag.SelectedSidebar = "Hotels";
            GetSessionProperties();
            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(r => r.Hotel)
                .Include(r => r.Type)
                .FirstOrDefaultAsync(m => m.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Room/Delete/5
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
            if (_context.Rooms == null)
            {
                return Problem("Entity set 'HotelReservationContext.Rooms'  is null.");
            }
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Hotels", "Admin");
        }

        private bool RoomExists(int id)
        {
            return (_context.Rooms?.Any(e => e.RoomId == id)).GetValueOrDefault();
        }
    }
}
