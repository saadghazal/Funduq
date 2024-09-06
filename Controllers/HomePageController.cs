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
    public class HomePageController : Controller
    {
        private readonly HotelReservationContext _context;
        private readonly IWebHostEnvironment _environment;

        public HomePageController(HotelReservationContext context,IWebHostEnvironment environment)
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
        // GET: HomePage
        public async Task<IActionResult> Index()
        {
            var hotelReservationContext = _context.HomePages.Include(h => h.Contact);
            return View(await hotelReservationContext.ToListAsync());
        }

        // GET: HomePage/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            ViewBag.SelectedSidebar = "HomePage";
            GetSessionProperties();
            if (id == null || _context.HomePages == null)
            {
                return NotFound();
            }

            var homePage = await _context.HomePages
                .Include(h => h.Contact)
                .FirstOrDefaultAsync(m => m.HomeId == id);
            if (homePage == null)
            {
                return NotFound();
            }

            return View(homePage);
        }

        // GET: HomePage/Create
        public IActionResult Create()
        {
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            ViewBag.SelectedSidebar = "HomePage";
            GetSessionProperties();
            ViewData["ContactId"] = new SelectList(_context.ContactUs, "ContactId", "ContactId");
            return View();
        }

        public async Task<string> UploadImage(IFormFile imageFile,string folderName)
        {

            string w3RootPath = _environment.WebRootPath;// it returns the path of the wwwroot Folder
            string fileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName; // the name of the file of the uploaded image
            string newPath = Path.Combine(w3RootPath + $"/{folderName}/", fileName);

            using (var fileStream = new FileStream(newPath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return fileName;
        }
        // POST: HomePage/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HomeId,FaviconFile,LogoFile,BackgroundImageFile,XUrl,FacebookUrl,InstagramUrl,TopTitle,TopSubtitle,SearchPlacholder,TestimonialsBackgroundImage,AboutUsImageFile,HomeLogoText,AboutUsParagraph,ContactId")] HomePage homePage)
        {
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            ViewBag.SelectedSidebar = "HomePage";
            GetSessionProperties();
            if (ModelState.IsValid)
            {
                
                

                    homePage.HomeLogoImage = await UploadImage(homePage.LogoFile, "LogoImages");

               
                

                    homePage.HomeFavicon = await UploadImage(homePage.FaviconFile, "FaviconImages");

                homePage.AboutUsPicture = await UploadImage(homePage.AboutUsImageFile, "AboutImages");
                homePage.TestimonialsBackgroundPicture = await UploadImage(homePage.TestimonialsBackgroundImage, "TBackgroundImages");
                homePage.BackgroundImage = await UploadImage(homePage.BackgroundImageFile, "HomeBackgrounds");

               
                
                
                
                _context.Add(homePage);
                await _context.SaveChangesAsync();
                return RedirectToAction("HomePageSettings","Admin");
            }
            ViewData["ContactId"] = new SelectList(_context.ContactUs, "ContactId", "ContactId", homePage.ContactId);
            return View(homePage);
        }

        // GET: HomePage/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            ViewBag.SelectedSidebar = "HomePage";
            GetSessionProperties();
            if (id == null || _context.HomePages == null)
            {
                return NotFound();
            }

            var homePage = await _context.HomePages.FindAsync(id);
            if (homePage == null)
            {
                return NotFound();
            }
            ViewData["ContactId"] = new SelectList(_context.ContactUs, "ContactId", "ContactId", homePage.ContactId);
            return View(homePage);
        }

        // POST: HomePage/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HomeId,FaviconFile,LogoFile,BackgroundImageFile,XUrl,FacebookUrl,InstagramUrl,TopTitle,TopSubtitle,SearchPlacholder,TestimonialsBackgroundImage,AboutUsImageFile,HomeLogoText,AboutUsParagraph,ContactId")] HomePage homePage)
        {
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            ViewBag.SelectedSidebar = "HomePage";
            GetSessionProperties();
            if (id != homePage.HomeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    var currentHomePage = await _context.HomePages.Where(p => p.HomeId == id).SingleOrDefaultAsync();
                    if (currentHomePage == null)
                    {
                        return NotFound();
                    }

                    // Update the currentHomePage entity with new values
                    if (homePage.LogoFile != null)
                    {
                        currentHomePage.HomeLogoImage = await UploadImage(homePage.LogoFile, "LogoImages");
                    }

                    if (homePage.FaviconFile != null)
                    {
                        currentHomePage.HomeFavicon = await UploadImage(homePage.FaviconFile, "FaviconImages");
                    }

                    if (homePage.AboutUsImageFile != null)
                    {
                        currentHomePage.AboutUsPicture = await UploadImage(homePage.AboutUsImageFile, "AboutImages");
                    }

                    if (homePage.TestimonialsBackgroundImage != null)
                    {
                        currentHomePage.TestimonialsBackgroundPicture = await UploadImage(homePage.TestimonialsBackgroundImage, "TBackgroundImages");
                    }

                    if (homePage.BackgroundImageFile != null)
                    {
                        currentHomePage.BackgroundImage = await UploadImage(homePage.BackgroundImageFile, "HomeBackgrounds");
                    }

                    // Update other properties that are not files
                    currentHomePage.XUrl = homePage.XUrl;
                    currentHomePage.FacebookUrl = homePage.FacebookUrl;
                    currentHomePage.InstagramUrl = homePage.InstagramUrl;
                    currentHomePage.TopTitle = homePage.TopTitle;
                    currentHomePage.TopSubtitle = homePage.TopSubtitle;
                    currentHomePage.SearchPlacholder = homePage.SearchPlacholder;
                    currentHomePage.HomeLogoText = homePage.HomeLogoText;
                    currentHomePage.AboutUsParagraph = homePage.AboutUsParagraph;
                    currentHomePage.ContactId = homePage.ContactId;

                    _context.Update(currentHomePage);
                    await _context.SaveChangesAsync();

                    GetHomePageData();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomePageExists(homePage.HomeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("HomePageSettings", "Admin");
            }
            ViewData["ContactId"] = new SelectList(_context.ContactUs, "ContactId", "ContactId", homePage.ContactId);
            return View(homePage);
        }

        // GET: HomePage/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            ViewBag.SelectedSidebar = "HomePage";
            GetSessionProperties();
            if (id == null || _context.HomePages == null)
            {
                return NotFound();
            }

            var homePage = await _context.HomePages
                .Include(h => h.Contact)
                .FirstOrDefaultAsync(m => m.HomeId == id);
            if (homePage == null)
            {
                return NotFound();
            }

            return View(homePage);
        }

        // POST: HomePage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            GetHomePageData(); 
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            ViewBag.SelectedSidebar = "HomePage";
            GetSessionProperties();
            if (_context.HomePages == null)
            {
                return Problem("Entity set 'HotelReservationContext.HomePages'  is null.");
            }
            var homePage = await _context.HomePages.FindAsync(id);
            if (homePage != null)
            {
                _context.HomePages.Remove(homePage);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("HomePageSettings", "Admin");
        }

        private bool HomePageExists(int id)
        {
          return (_context.HomePages?.Any(e => e.HomeId == id)).GetValueOrDefault();
        }
    }
}
