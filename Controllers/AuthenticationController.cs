using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using funduq.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace funduq.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly HotelReservationContext _context;
        private readonly IWebHostEnvironment _environment;

        public AuthenticationController(HotelReservationContext context,IWebHostEnvironment environment) {

            _context = context;
            _environment = environment;

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
                ViewBag.Logo = Url.Content( "~/LogoImages/" + homeData.HomeLogoImage);
                ViewBag.Favicon = Url.Content("~/FaviconImages/" + homeData.HomeFavicon);
                ViewBag.CompanyName = homeData.HomeLogoText;
            }

        }

        public IActionResult AdminRegister() {
            if (HttpContext.Session.GetInt32("admin_user_id") != null) {
                return RedirectToAction("Dashboard", "Admin");
            }
            GetHomePageData();
   
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminRegister([Bind("UserId,FirstName,LastName,PhoneNumber,ProfileImage")] UserProfile profile,string? email,string? password) {
            GetHomePageData();
            if (ModelState.IsValid) {
                
                if (email == null) {

                    return View(profile);
                }
                var isAlreadyExist = _context.UserCredentials.Where(u => email == u.Email).Count() != 0;
                if (isAlreadyExist) {
                     TempData["Alert"] = "User already exist.";
                        return View();
                   
                }
                if (password == null) {
                    return View(profile);
                }
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
                else
                {
                    return View();

                }



                _context.Add(profile);
                await _context.SaveChangesAsync();
                UserCredential userCredential = new UserCredential();
                userCredential.Email = email;
                userCredential.UserPassword = password;
                userCredential.RoleId = 1;
                userCredential.UserId = profile.UserId;

                _context.Add(userCredential);
                await _context.SaveChangesAsync();
                return RedirectToAction("AdminLogin");
            }
            return View(profile);
        }
        public IActionResult AdminLogin() {
            if (HttpContext.Session.GetInt32("admin_user_id") != null)
            {
                return RedirectToAction("Dashboard", "Admin");
            }
            GetHomePageData();
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminLogin(string? email,string? password) {
            GetHomePageData();
            if (email != null && password != null && _context.UserCredentials != null)
            {
                var auth = _context.UserCredentials.Where(u => u.Email == email && u.UserPassword == password).SingleOrDefault();

                if (auth == null)
                {
                    TempData["Alert"] = "Either the email or password you entered is incorrect.";
                    return View();
                }
                else {
                    if (auth.RoleId != 1) {
                        TempData["Alert"] = "Not Authorized";
                        return View();
                    }
                    var userProfile =  _context.UserProfiles.Where(u => u.UserId == auth.UserId).SingleOrDefault();
                    if (userProfile == null) {
                        return View();
                    }
                    HttpContext.Session.SetInt32("admin_user_id", userProfile.UserId);
                    HttpContext.Session.SetString("admin_email", auth.Email);
                    HttpContext.Session.SetString("admin_firstName", userProfile.FirstName);
                    HttpContext.Session.SetString("admin_lastName", userProfile.LastName);
                    HttpContext.Session.SetString("admin_phoneNumber", userProfile.PhoneNumber);
                    HttpContext.Session.SetString("admin_profilePicture", userProfile.ProfilePicture);
                    return RedirectToAction("Dashboard", "Admin");
                }
            }
            else {
                return View();
            }

        }

        public IActionResult UserLogin() {
            if (HttpContext.Session.GetInt32("user_id") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            GetHomePageData();
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserLogin([Bind("UserId,Email,UserPassword")] UserCredential user) {
            GetHomePageData();
            
            if (user.Email != null && user.UserPassword != null && _context.UserCredentials != null)
            {
                var auth = _context.UserCredentials.Where(u => u.Email == user.Email && u.UserPassword == user.UserPassword).SingleOrDefault();

                if (auth == null)
                {
                    TempData["Alert"] = "Either the email or password you entered is incorrect.";
                    return View();
                }
                else
                {
                    if (auth.RoleId != 2)
                    {
                        TempData["Alert"] = "Not Authorized";
                        return View();
                    }
                    var userProfile = _context.UserProfiles.Where(u => u.UserId == auth.UserId).SingleOrDefault();
                    if (userProfile == null)
                    {
                        return View();
                    }
                    HttpContext.Session.SetInt32("user_id", userProfile.UserId);
                    HttpContext.Session.SetString("email", auth.Email);
                   
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return View();
            }
            

        }

        public IActionResult AdminLogout() {
           
            HttpContext.Session.Remove("admin_user_id" );
            HttpContext.Session.Remove("admin_email");
            HttpContext.Session.Remove("admin_firstName");
            HttpContext.Session.Remove("admin_lastName");
            HttpContext.Session.Remove("admin_phoneNumber");
            HttpContext.Session.Remove("admin_profilePicture");
            return RedirectToAction("AdminLogin");
        }
        public IActionResult UserRegister()
        {
            if (HttpContext.Session.GetInt32("user_id") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            GetHomePageData();
           
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserRegister([Bind("UserId,FirstName,LastName,PhoneNumber,ProfileImage")] UserProfile profile, string? email, string? password) {

            GetHomePageData();
            
            if (ModelState.IsValid)
            {
                if (email == null)
                {
                    return View(profile);
                }
                if (password == null)
                {
                    return View(profile);
                }
                var isAlreadyExist = _context.UserCredentials.Where(u => email == u.Email).Count() != 0;
                if (isAlreadyExist)
                {
                    TempData["Alert"] = "User already exist.";
                    return View();

                }
                if (profile.ProfileImage != null)
                {

                    string w3RootPath = _environment.WebRootPath;// it returns the path of the wwwroot Folder
                    string fileName = Guid.NewGuid().ToString() + "_" + profile.ProfileImage.FileName; // the name of the file of the uploaded image
                    string newPath = Path.Combine(w3RootPath + "/UsersImages/", fileName);

                    using (var fileStream = new FileStream(newPath, FileMode.Create))
                    {
                        await profile.ProfileImage.CopyToAsync(fileStream);
                    }

                    profile.ProfilePicture = fileName;
                }
                else {

                    return View(profile);

                }
                _context.Add(profile);
                await _context.SaveChangesAsync();
                UserCredential userCredential = new UserCredential();
                userCredential.Email = email;
                userCredential.UserPassword = password;
                userCredential.RoleId = 2;
                userCredential.UserId = profile.UserId;

                _context.Add(userCredential);
                await _context.SaveChangesAsync();
                return RedirectToAction("UserLogin");
            }
            return View(profile);
        }

        public IActionResult UserLogout()
        {
            HttpContext.Session.Remove("user_id");

            return RedirectToAction("UserLogin");
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}

