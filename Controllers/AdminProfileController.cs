using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using funduq.Models;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace funduq.Controllers
{
    public class AdminProfileController : Controller
    {
        private readonly HotelReservationContext _context;
        private readonly IWebHostEnvironment _environment;


        public AdminProfileController(HotelReservationContext context, IWebHostEnvironment environment) {

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


        // GET: /<controller>/
        public IActionResult Index()
        {
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            var userId = HttpContext.Session.GetInt32("admin_user_id");
            GetSessionProperties();
            var profile = _context.UserProfiles.Where(p => p.UserId == userId).SingleOrDefault();
            if(profile == null)
            {
                return RedirectToAction("Dashboard", "Admin");
            }

            return View(profile);
        }

        [HttpPost]
        public async Task<IActionResult> Index(int? id,[Bind("UserId,ProfileImage,FirstName,LastName,PhoneNumber")] UserProfile profile,string? email) {
            GetHomePageData();

            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            if (email == null)
            {
                return RedirectToAction("Index");
            }
            var isAlreadyExist = _context.UserCredentials.Where(u => email == u.Email && profile.UserId != u.UserId).Count() != 0;
            if (isAlreadyExist)
            {
                TempData["Alert"] = "Email already in use by another account.";
                return RedirectToAction("Index");

            }
            else {
                if (profile.ProfileImage != null)
                {

                    string w3RootPath = _environment.WebRootPath;// it returns the path of the wwwroot Folder
                    string fileName = Guid.NewGuid().ToString() + "_" + profile.ProfileImage.FileName; // the name of the file of the uploaded image
                    string newPath = Path.Combine(w3RootPath + "/AdminImages/", fileName);

                    using (var fileStream = new FileStream(newPath, FileMode.Create))
                    {
                        await profile.ProfileImage.CopyToAsync(fileStream);
                    }

                    profile.ProfilePicture = fileName;
                }
                else {
                    profile.ProfilePicture = HttpContext.Session.GetString("profilePicture");
                }

             

                var auth = _context.UserCredentials.Where(a => a.UserId == profile.UserId).SingleOrDefault();
                if (auth == null)
                {
                    return NotFound();
                }
                else {
                    auth.Email = email;
                    _context.Update(auth);
                    _context.Update(profile);
                    await _context.SaveChangesAsync();
                    HttpContext.Session.SetInt32("admin_user_id", profile.UserId);
                    HttpContext.Session.SetString("admin_email", auth.Email);
                    HttpContext.Session.SetString("admin_firstName", profile.FirstName);
                    HttpContext.Session.SetString("admin_lastName", profile.LastName);
                    HttpContext.Session.SetString("admin_phoneNumber", profile.PhoneNumber);
                    HttpContext.Session.SetString("admin_profilePicture", profile.ProfilePicture);
                    return RedirectToAction("Index");
                }
              
            }

        }
    }
}

