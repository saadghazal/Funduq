using funduq.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using funduq.Services;

namespace funduq.Controllers
{
    public class AdminController : Controller
    {
        private readonly HotelReservationContext _context;

        public AdminController(HotelReservationContext context){
            _context = context;
        }

         public void GetSessionProperties() {
            ViewBag.userId = HttpContext.Session.GetInt32("admin_user_id");
            ViewBag.userFName = HttpContext.Session.GetString("admin_firstName");
            ViewBag.userLName = HttpContext.Session.GetString("admin_lastName");
            ViewBag.userPhoneNumber = HttpContext.Session.GetString("admin_phoneNumber");
            ViewBag.userProfilePicture = HttpContext.Session.GetString("admin_profilePicture");
            ViewBag.userEmail = HttpContext.Session.GetString("admin_email");
        }

        public void GetHomePageData() {
            var homeData = _context.HomePages.FirstOrDefault();
            if (homeData == null) {
                ViewBag.Logo = "~/AdminDesign/img/logo.png";
                ViewBag.CompanyName = "Funduq";
                
                ViewBag.Favicon = "";
            }
            else {
                ViewBag.Logo = Url.Content("~/LogoImages/"+homeData.HomeLogoImage);
                ViewBag.Favicon = Url.Content("~/FaviconImages/" + homeData.HomeFavicon);
                ViewBag.CompanyName = homeData.HomeLogoText;
                
                ViewBag.AboutUsImage = Url.Content("~/AboutImages/"+ homeData.AboutUsPicture);
                ViewBag.BackgroundImage = Url.Content("~/HomeBackgrounds/"+homeData.BackgroundImage);
                ViewBag.TBackgroundImage = Url.Content("~/TBackgroundImages/"+ homeData.TestimonialsBackgroundPicture);
                ViewBag.WelcomeSentance = homeData.TopTitle;
                ViewBag.Slogan = homeData.TopSubtitle;
                ViewBag.XUrl = homeData.XUrl;
                ViewBag.FacebookUrl = homeData.FacebookUrl;
                ViewBag.InstagramUrl = homeData.InstagramUrl;
                
            }
            
            
        }
        public IActionResult Dashboard(){

            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null) {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            ViewBag.SelectedSidebar = "Dashboard";

            ReservationsIncome();
            CustomersCalculation();
            ReservationsCalculation();
            ViewBag.FirstWeek = DateTime.Now.AddDays(-30);
            ViewBag.SecondWeek = DateTime.Now.AddDays(-22);
            ViewBag.ThirdWeek = DateTime.Now.AddDays(-14);
            ViewBag.ForthWeek = DateTime.Now;
            
            GetSessionProperties();

            var recentReservations = _context.RoomReservations.Include(r => r.Room).Include(r => r.Room.Hotel).Include(r => r.User).ToList();

            return View(recentReservations);
        }
        void ReservationsIncome() {
            var reservations = _context.RoomReservations.ToList();

            var monthIncome = reservations.Where(r => r.CreatedAt.Value >= DateTime.Now.AddDays(-30) && r.CreatedAt.Value <= DateTime.Now).Sum(r => r.TotalPrice);
            var monthSales = reservations.Where(r => r.CreatedAt.Value >= DateTime.Now.AddDays(-30) && r.CreatedAt.Value <= DateTime.Now).Count();

            var firstWeekIncome = reservations.Where(r => r.CreatedAt.Value >= DateTime.Now.AddDays(-30) && r.CreatedAt.Value <= DateTime.Now.AddDays(-23)).Sum(r => r.TotalPrice);
            var secondtWeekIncome = reservations.Where(r => r.CreatedAt.Value >= DateTime.Now.AddDays(-22) && r.CreatedAt.Value <= DateTime.Now.AddDays(-15)).Sum(r => r.TotalPrice);
            var thirdWeekIncome = reservations.Where(r => r.CreatedAt.Value >= DateTime.Now.AddDays(-14) && r.CreatedAt.Value <= DateTime.Now.AddDays(-8)).Sum(r => r.TotalPrice);
            var forthWeekIncome = reservations.Where(r => r.CreatedAt.Value >= DateTime.Now.AddDays(-8) && r.CreatedAt.Value <= DateTime.Now).Sum(r => r.TotalPrice);

            ViewBag.FirstWeekIncome = firstWeekIncome;
            ViewBag.SecondWeekIncome = secondtWeekIncome;
            ViewBag.ThirdWeekIncome = thirdWeekIncome;
            ViewBag.ForthWeekIncome = forthWeekIncome;
            ViewBag.MonthSales = monthSales;
            ViewBag.MonthIncome = monthIncome;

        }
        void ReservationsCalculation()
        {
            var reservations = _context.RoomReservations.ToList();
            
            var firstWeekSales = reservations.Where(r => r.CreatedAt.Value >= DateTime.Now.AddDays(-30) && r.CreatedAt.Value <= DateTime.Now.AddDays(-23)).Count();
            var secondtWeekSales = reservations.Where(r => r.CreatedAt.Value >= DateTime.Now.AddDays(-22) && r.CreatedAt.Value <= DateTime.Now.AddDays(-15)).Count();
            var thirdWeekSales = reservations.Where(r => r.CreatedAt.Value >= DateTime.Now.AddDays(-14) && r.CreatedAt.Value <= DateTime.Now.AddDays(-8)).Count();
            var forthWeekSales = reservations.Where(r => r.CreatedAt.Value >= DateTime.Now.AddDays(-8) && r.CreatedAt.Value <= DateTime.Now).Count();

            ViewBag.FirstWeekSales = firstWeekSales;
            ViewBag.SecondWeekSales = secondtWeekSales;
            ViewBag.ThirdWeekSales = thirdWeekSales;
            ViewBag.ForthWeekSales = forthWeekSales;


        }
        void CustomersCalculation()
        {
            var users = _context.UserCredentials.Include(u => u.User).Where(u => u.RoleId == 2).ToList();
            ViewBag.CustomersQuantity = users.Count();
            var firstWeekCustomers = users.Where(c => c.User.CreatedAt.Value >= DateTime.Now.AddDays(-30) && c.User.CreatedAt.Value <= DateTime.Now.AddDays(-23)).Count();
            var secondWeekCustomers = users.Where(c => c.User.CreatedAt.Value >= DateTime.Now.AddDays(-22) && c.User.CreatedAt.Value <= DateTime.Now.AddDays(-15)).Count();
            var thirdWeekCustomers = users.Where(c => c.User.CreatedAt.Value >= DateTime.Now.AddDays(-14) && c.User.CreatedAt.Value <= DateTime.Now.AddDays(-8)).Count();
            var forthWeekCustomers = users.Where(c => c.User.CreatedAt.Value >= DateTime.Now.AddDays(-7) && c.User.CreatedAt.Value <= DateTime.Now).Count();
            ViewBag.FirstWeekCustomers = firstWeekCustomers;
            ViewBag.SecondWeekCustomers = secondWeekCustomers;
            ViewBag.ThirdWeekCustomers = thirdWeekCustomers;
            ViewBag.ForthWeekCustomers = forthWeekCustomers;
        }

        public IActionResult RegisteredUsers() {
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            ViewBag.SelectedSidebar = "Users";
            GetSessionProperties();

            var users = _context.UserCredentials.Where(u => u.RoleId != 1).Include(u => u.User);

            return View(users);
        }
        public IActionResult UserDetails(int userId)
        {
            var adminUserId = HttpContext.Session.GetInt32("admin_user_id");

            GetHomePageData();
            ViewBag.SelectedSidebar = "Users";
            if (adminUserId == null)
            {

                return RedirectToAction("AdminLogin", "Authentication");
            }
            else
            {
                GetSessionProperties();
                var reservations = _context.RoomReservations.Where(r => r.UserId == userId).OrderByDescending(r => r.CreatedAt).Include(r => r.Room).Include(r => r.Room.Hotel).ToList();

                var user = _context.UserProfiles.Where(u => u.UserId == userId).SingleOrDefault();
                var userEmail = _context.UserCredentials.Where(u => u.UserId == userId).SingleOrDefault().Email;
                ViewBag.userEmail = userEmail;
                ViewBag.user = user;
                var profileData = Tuple.Create<UserProfile, IEnumerable<RoomReservation>>(user, reservations);
                return View(profileData);
            }
        }
        public async Task<IActionResult> Hotels(){
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            ViewBag.SelectedSidebar = "Hotels";
            GetSessionProperties();
             var hotelReservationContext = _context.Hotels.Include(h => h.HotelCityNavigation);
             
            return View(await hotelReservationContext.ToListAsync());
         
        }
       

        [HttpGet]
        public IActionResult CreateHotel(){
            GetHomePageData();
            
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            GetSessionProperties();
            return RedirectToAction("Create","Hotel");
        }

        public IActionResult HotelDetails(int id){
            GetHomePageData();
            
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            GetSessionProperties();
            ViewBag.SelectedSidebar = "Hotels";
            
            return RedirectToAction("Details","Hotel",new {id = id});
        }


        public IActionResult Cities(){
            GetHomePageData();
           
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            ViewBag.SelectedSidebar = "Cities";
            GetSessionProperties();
            var cities = _context.Cities.ToList();
            return View(cities);
        }
        
        public IActionResult HotelRooms(int? id,string hotelName){
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            Console.WriteLine("HotelId: " + id);
            ViewBag.SelectedSidebar = "Hotels";
            ViewBag.HotelName = hotelName;
            GetSessionProperties();
            var rooms = _context.Rooms.Where(r => r.HotelId == id).Include(r => r.Type).ToList();
            return View(rooms);
        }
        public IActionResult HotelServices(int? id,string hotelName){
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            ViewBag.SelectedSidebar = "Hotels";
            ViewBag.HotelName = hotelName;
            GetSessionProperties();
            var services = _context.HotelServices.Where(s => s.HotelId == id).ToList();
            return View(services);
        }
        
        public async Task<IActionResult> ContactUs() {
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            ViewBag.SelectedSidebar = "ContactUs";
            GetSessionProperties();
            if (_context.ContactUs == null) {
                return View(null);
            }
            var contactUs = await _context.ContactUs.FirstOrDefaultAsync();

            return View(contactUs);
        }
        public async Task<IActionResult> HomePageSettings() {
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            ViewBag.SelectedSidebar = "HomePage";
            GetSessionProperties();
            if (_context.HomePages == null) {
                return View(null);
            }
            var home = await _context.HomePages.FirstOrDefaultAsync();
            return View(home);
        }
        public IActionResult AdminTestimonials() {
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            GetSessionProperties();
            ViewBag.SelectedSidebar = "Testimonials";

            var pendingTestimonials = _context.Testimonials.Where(t => t.TestimonialStatus == "Pending").Include(t => t.User).ToList();
            return View(pendingTestimonials);
        }
        public async Task<IActionResult> AcceptTestimonial(int testimonialId) {
            
            var accpetedTestimonial = _context.Testimonials.Where(t => t.TestimonialId == testimonialId).SingleOrDefault();
            accpetedTestimonial.TestimonialStatus = "Accepted";
            _context.Testimonials.Update(accpetedTestimonial);
            await _context.SaveChangesAsync();
            var newTestimonails = _context.Testimonials.Where(t => t.TestimonialStatus == "Accepted").ToList();
            var firstTestimonial = newTestimonails.FirstOrDefault();
            if (newTestimonails.Count() > 5) {
                 _context.Testimonials.Remove(firstTestimonial);
               await _context.SaveChangesAsync();
            }
            return RedirectToAction("AdminTestimonials");
          

        }
        public async Task<IActionResult> RejectTestimonial(int testimonialId)
        {

            var rejectedTestimonial = _context.Testimonials.Where(t => t.TestimonialId == testimonialId).SingleOrDefault();
            
            _context.Testimonials.Remove(rejectedTestimonial);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("AdminTestimonials");


        }
        public IActionResult Reservations() {
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            GetSessionProperties();
            ViewBag.SelectedSidebar = "Reservations";
            var reservations = _context.RoomReservations.Include(r => r.Room).Include(r => r.User).Include(r => r.Room.Hotel).ToList();
            ViewBag.BillsTotal = reservations.Sum(r => r.TotalPrice);
            return View(reservations) ;
        }
        [HttpPost]
        public IActionResult Reservations(DateTime? startDate,DateTime? endDate) {
            GetHomePageData();
            if (HttpContext.Session.GetInt32("admin_user_id") == null)
            {
                return RedirectToAction("AdminLogin", "Authentication");
            }
            GetSessionProperties();
            ViewBag.SelectedSidebar = "Reservations";
            var reservations = _context.RoomReservations.Include(r => r.Room).Include(r => r.User).Include(r => r.Room.Hotel).ToList();
            if (startDate == null && endDate == null)
            {

                ViewBag.BillsTotal = reservations.Sum(x => x.TotalPrice);
                return View(reservations);
            }
            else if (startDate != null && endDate == null)
            {
                reservations = reservations.Where(x => x.CreatedAt.Value >= startDate.Value).ToList();
                ViewBag.BillsTotal = reservations.Sum(x => x.TotalPrice);
                return View(reservations);

            }
            else if (startDate == null && endDate != null)
            {
                reservations = reservations.Where(x => x.CreatedAt.Value <= endDate.Value).ToList();
                ViewBag.BillsTotal = reservations.Sum(x => x.TotalPrice);
                return View(reservations);
            }
            else
            {
                reservations = reservations.Where(x => x.CreatedAt.Value >= startDate.Value && x.CreatedAt.Value <= endDate.Value).ToList();
                ViewBag.BillsTotal = reservations.Sum(x => x.TotalPrice);
                return View(reservations);
            }

        }
    }
}