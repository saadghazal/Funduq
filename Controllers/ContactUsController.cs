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
    public class ContactUsController : Controller
    {
        private readonly HotelReservationContext _context;

        public ContactUsController(HotelReservationContext context)
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


        // GET: ContactUs
        public async Task<IActionResult> Index()
        {
              return _context.ContactUs != null ? 
                          View(await _context.ContactUs.ToListAsync()) :
                          Problem("Entity set 'HotelReservationContext.ContactUs'  is null.");
        }

        // GET: ContactUs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            ViewBag.SelectedSidebar = "ContactUs";
            GetSessionProperties();
            if (id == null || _context.ContactUs == null)
            {
                return NotFound();
            }

            var contactU = await _context.ContactUs
                .FirstOrDefaultAsync(m => m.ContactId == id);
            if (contactU == null)
            {
                return NotFound();
            }

            return View(contactU);
        }

        // GET: ContactUs/Create
        public IActionResult Create()
        {
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            ViewBag.SelectedSidebar = "ContactUs";
            GetSessionProperties();
            return View();
        }

        // POST: ContactUs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContactId,ContactUsPhoneNumber,ContactUsEmail,ContactUsAddress,ContactUsOpenDays,ContactUsOpenHours")] ContactU contactU)
        {
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            ViewBag.SelectedSidebar = "ContactUs";
            GetSessionProperties();
            if (ModelState.IsValid)
            {
                _context.Add(contactU);
                await _context.SaveChangesAsync();
                return RedirectToAction("ContactUs","Admin");
            }
            return View(contactU);
        }

        // GET: ContactUs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            ViewBag.SelectedSidebar = "ContactUs";
            GetSessionProperties();
            if (id == null || _context.ContactUs == null)
            {
                return NotFound();
            }

            var contactU = await _context.ContactUs.FindAsync(id);
            if (contactU == null)
            {
                return NotFound();
            }
            return View(contactU);
        }

        // POST: ContactUs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContactId,ContactUsPhoneNumber,ContactUsEmail,ContactUsAddress,ContactUsOpenDays,ContactUsOpenHours")] ContactU contactU)
        {
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            ViewBag.SelectedSidebar = "ContactUs";
            GetSessionProperties();
            if (id != contactU.ContactId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contactU);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactUExists(contactU.ContactId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("ContactUs", "Admin");
            }
            return View(contactU);
        }

        // GET: ContactUs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            ViewBag.SelectedSidebar = "ContactUs";
            GetSessionProperties();
            if (id == null || _context.ContactUs == null)
            {
                return NotFound();
            }

            var contactU = await _context.ContactUs
                .FirstOrDefaultAsync(m => m.ContactId == id);
            if (contactU == null)
            {
                return NotFound();
            }

            return View(contactU);
        }

        // POST: ContactUs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            ViewBag.SelectedSidebar = "ContactUs";
            GetSessionProperties();
            if (_context.ContactUs == null)
            {
                return Problem("Entity set 'HotelReservationContext.ContactUs'  is null.");
            }
            var contactU = await _context.ContactUs.FindAsync(id);
            if (contactU != null)
            {
                _context.ContactUs.Remove(contactU);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("ContactUs","Admin");
        }

        private bool ContactUExists(int id)
        {
          return (_context.ContactUs?.Any(e => e.ContactId == id)).GetValueOrDefault();
        }
    }
}
