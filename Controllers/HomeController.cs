using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using funduq.Models;
using Microsoft.EntityFrameworkCore;
using funduq.Services;

namespace funduq.Controllers;



public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly HotelReservationContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly PdfService _pdfService;
    private readonly EmailService _emailService;

    public HomeController(ILogger<HomeController> logger, HotelReservationContext context,PdfService pdfService,EmailService emailService,IWebHostEnvironment environment)
    {
        _logger = logger;
        _context = context;
        _pdfService = pdfService;
        _emailService = emailService;
        _environment = environment;
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

            ViewBag.Logo = Url.Content("~/LogoImages/" + homeData.HomeLogoImage);
            ViewBag.Favicon = Url.Content("~/FaviconImages/" + homeData.HomeFavicon);
            ViewBag.CompanyName = homeData.HomeLogoText;
            ViewBag.AboutUsImage = Url.Content("~/AboutImages/" + homeData.AboutUsPicture);
            ViewBag.SearchPlaceholder = homeData.SearchPlacholder;
            ViewBag.BackgroundImage = Url.Content("~/HomeBackgrounds/" + homeData.BackgroundImage);
            ViewBag.TBackgroundImage = Url.Content("~/TBackgroundImages/" + homeData.TestimonialsBackgroundPicture);
            ViewBag.WelcomeSentance = homeData.TopTitle;
            ViewBag.Slogan = homeData.TopSubtitle;
            ViewBag.XUrl = homeData.XUrl;
            ViewBag.FacebookUrl = homeData.FacebookUrl;
            ViewBag.InstagramUrl = homeData.InstagramUrl;
            ViewBag.ContactUsOpenDays = homeData.Contact.ContactUsOpenDays;
            ViewBag.ContactUsOpenHours = homeData.Contact.ContactUsOpenHours;
            ViewBag.ContactUsPhone = homeData.Contact.ContactUsPhoneNumber;
            ViewBag.ContactUsAddress = homeData.Contact.ContactUsAddress;
            ViewBag.ContactUsEmail = homeData.Contact.ContactUsEmail;
        }


    }

    public IActionResult Index()
    {
        GetHomePageData();
        ViewBag.page = "Home";
        var featuredHotels = _context.Hotels.Where(h => (bool)h.IsFeaturedHotel).ToList();
        var homePageComponents = _context.HomePages.FirstOrDefault();
        var contactUs = _context.ContactUs.FirstOrDefault();
        var userId = HttpContext.Session.GetInt32("user_id");
        var acceptedTestimonials = _context.Testimonials.Where(t => t.TestimonialStatus == "Accepted").Include(t => t.User).ToList();
        var homeData = Tuple.Create<HomePage, IEnumerable<Hotel>, ContactU,IEnumerable<Testimonial>>(homePageComponents, featuredHotels, contactUs,acceptedTestimonials);

        if (userId == null)
        {
            ViewBag.user = null;
            return View(homeData);
        }
        else {

            ViewBag.user = _context.UserProfiles.Where(u => u.UserId == userId).SingleOrDefault();

            return View(homeData);
        }

    }
    public IActionResult ProfilePage() {
        var userId = HttpContext.Session.GetInt32("user_id");

        GetHomePageData();
        if (userId == null)
        {
           
            return RedirectToAction("UserLogin","Authentication");
        }
        else
        {
            var reservations = _context.RoomReservations.Where(r => r.UserId == userId).OrderByDescending(r => r.CreatedAt).Include(r => r.Room).Include(r => r.Room.Hotel).ToList();

            var user = _context.UserProfiles.Where(u => u.UserId == userId).SingleOrDefault();
            var userEmail = _context.UserCredentials.Where(u => u.UserId == userId).SingleOrDefault().Email;
            ViewBag.userEmail = userEmail;
            ViewBag.user = user;
            var profileData = Tuple.Create<UserProfile, IEnumerable<RoomReservation>>(user, reservations);
            return View(profileData);
        }
    }

    [HttpPost]
    public async Task<IActionResult> ProfilePage(int uid,string? FirstName, string? LastName, string email, string? PhoneNumber, IFormFile? ProfileImage) {
        var userData = _context.UserProfiles.Where(u => u.UserId == uid).SingleOrDefault();
        var userCredentials = _context.UserCredentials.Where(u => u.UserId == uid).SingleOrDefault();
        if (userCredentials == null) {
            return NotFound();
        }
        if (userData == null) {
            return NotFound();
        }
        userData.FirstName = FirstName ?? userData.FirstName;
        userData.LastName = LastName ?? userData.LastName;
        userData.PhoneNumber = PhoneNumber ?? userData.PhoneNumber;
        if (ProfileImage == null)
        {
            userData.ProfilePicture = userData.ProfilePicture;
        }
        else {

            string w3RootPath = _environment.WebRootPath;// it returns the path of the wwwroot Folder
            string fileName = Guid.NewGuid().ToString() + "_" + ProfileImage.FileName; // the name of the file of the uploaded image
            string newPath = Path.Combine(w3RootPath + "/UsersImages/", fileName);

            using (var fileStream = new FileStream(newPath, FileMode.Create))
            {
                await ProfileImage.CopyToAsync(fileStream);
            }

            userData.ProfilePicture = fileName;
        }
        userCredentials.Email = email ?? userCredentials.Email;
        _context.Update(userData);
        _context.Update(userCredentials);

        await _context.SaveChangesAsync();

        return RedirectToAction("ProfilePage");
    }
   

    public IActionResult HotelsPage() {
        ViewBag.page = "Hotels";
        GetHomePageData();
        var hotels = _context.Hotels.ToList();
        var homePageComponents = _context.HomePages.FirstOrDefault();
        var contactUs = _context.ContactUs.FirstOrDefault();
        var userId = HttpContext.Session.GetInt32("user_id");
        var cities = _context.Cities.ToList();
        var homeData = Tuple.Create<HomePage, IEnumerable<Hotel>, ContactU,IEnumerable<City>>(homePageComponents, hotels, contactUs,cities);
        if (userId == null)
        {
            ViewBag.user = null;
            return View(homeData);
        }
        else
        {

            ViewBag.user = _context.UserProfiles.Where(u => u.UserId == userId).SingleOrDefault();
            return View(homeData);
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult AboutUs() {
        
        GetHomePageData();
        var homePage = _context.HomePages.FirstOrDefault();
        var userId = HttpContext.Session.GetInt32("user_id");
        ViewBag.page = "About Us";

        if (userId == null)
        {
            ViewBag.user = null;
            return View(homePage);
        }
        else
        {
            ViewBag.user = _context.UserProfiles.Where(u => u.UserId == userId).SingleOrDefault();
            return View(homePage);
        }
    }
    public IActionResult ContactUs() {
        GetHomePageData();
        var contactUs = _context.ContactUs.FirstOrDefault();
        var userId = HttpContext.Session.GetInt32("user_id");
        ViewBag.page = "Contact Us";

        if (userId == null)
        {
            ViewBag.user = null;
            return View(contactUs);
        }
        else
        {
            ViewBag.user = _context.UserProfiles.Where(u => u.UserId == userId).SingleOrDefault();
            return View(contactUs);
        }
    }
    [HttpPost]
    public IActionResult ContactUs(string name,string userEmail,string subject,string message,string contactUsEmail) {
        try
        {
            _emailService.ContactUs(name, userEmail, subject, message, contactUsEmail);

            GetHomePageData();
            var contactUs = _context.ContactUs.FirstOrDefault();
            var userId = HttpContext.Session.GetInt32("user_id");
            ViewBag.page = "Contact Us";

            _emailService.ContactUsAutomaticReply(name, contactUsEmail, userEmail);
            TempData["Alert"] = "Thank you for contacting us.\nCheck your email.";
            if (userId == null)
            {

                ViewBag.user = null;
                return View(contactUs);
            }
            else
            {
                ViewBag.user = _context.UserProfiles.Where(u => u.UserId == userId).SingleOrDefault();
                return View(contactUs);
            }
        }
        catch (InvalidEmailException e) {
            var contactUs = _context.ContactUs.FirstOrDefault();
            var userId = HttpContext.Session.GetInt32("user_id");
            ViewBag.page = "Contact Us";
            TempData["Alert"] = e.getMessage();
            if (userId == null)
            {

                ViewBag.user = null;
                return View(contactUs);
            }
            else
            {
                ViewBag.user = _context.UserProfiles.Where(u => u.UserId == userId).SingleOrDefault();
                return View(contactUs);
            }
          
        }
        
    }
    public IActionResult HotelPage(int hotelId) {
        ViewBag.page = "Hotels";
        GetHomePageData();
        if (_context.Hotels == null) {
            return NotFound();
        }

        var hotel = _context.Hotels.Where(h => h.HotelId == hotelId).FirstOrDefault();
        if (hotel == null) {
            return NotFound();
        }
        var rooms = _context.Rooms.Where(r => r.HotelId == hotelId).Include(r => r.Type).ToList();
        if (rooms == null) {
            return NotFound();
        }
        var services = _context.HotelServices.Where(s => s.HotelId == hotelId).ToList();
        if (services == null) {
            return NotFound();
        }
        var userId = HttpContext.Session.GetInt32("user_id");
        var allHomeData = Tuple.Create<Hotel, IEnumerable<Room>, IEnumerable<HotelService>>(hotel, rooms, services);

        if (userId == null)
        {
            ViewBag.user = null;
            return View(allHomeData);
        }
        else
        {
            ViewBag.user = _context.UserProfiles.Where(u => u.UserId == userId).SingleOrDefault();
            return View(allHomeData);
        }
    }

    public IActionResult RoomPage(int roomId) {
        ViewBag.page = "Hotels";
        GetHomePageData();
        if (_context.Rooms == null)
        {
            return NotFound();
        }

        var room = _context.Rooms.Where(r => r.RoomId == roomId).Include(r => r.Hotel).Include(r => r.Type).FirstOrDefault();
        if (room == null)
        {
            return NotFound();
        }

        var userId = HttpContext.Session.GetInt32("user_id");


        if (userId == null)
        {
            ViewBag.user = null;
            return View(room);
        }
        else
        {
            ViewBag.user = _context.UserProfiles.Where(u => u.UserId == userId).SingleOrDefault();
            return View(room);
        }
    }
    public IActionResult Testimonials() {

        ViewBag.page = "Testimonials";
        GetHomePageData();
        var userId = HttpContext.Session.GetInt32("user_id");
        var testimonials = _context.Testimonials.Where(t => t.TestimonialStatus == "Accepted").Include(t => t.User).ToList();
        if (userId == null)
        {
            ViewBag.user = null;
            return View(testimonials);
        }
        else
        {
            ViewBag.user = _context.UserProfiles.Where(u => u.UserId == userId).SingleOrDefault();
            return View(testimonials);
        }


    }

    [HttpPost]
    public async Task<IActionResult> Testimonials(int? rating, string? userOpinion)
    {

        if (rating == null || userOpinion == null)
        {
            return RedirectToAction("Testimonials");
        }
        else {
            Testimonial testimonial = new Testimonial();
            testimonial.Rating = rating;
            testimonial.UserOpinion = userOpinion;
            testimonial.UserId = HttpContext.Session.GetInt32("user_id");
            _context.Add(testimonial);
            await _context.SaveChangesAsync();
            TempData["Alert"] = "Testimonial Sent Successfully!";
            return RedirectToAction("Testimonials");
        }
    }

    public IActionResult Payment(int? rId, decimal price, DateOnly? checkInDate, DateOnly? checkOutDate) {
        var userId = HttpContext.Session.GetInt32("user_id");
        if (userId == null) {
            return RedirectToAction("UserLogin", "Authentication");
        }

        if (checkInDate == null || checkOutDate == null) {
            TempData["Alert"] = "Please fill all fields";
            return RedirectToAction("RoomPage", new { roomId = rId });
        }

        int validateDates = checkInDate.Value.CompareTo(checkOutDate);
        if (validateDates > 0) {
            TempData["Alert"] = "Invalid data input";
            return RedirectToAction("RoomPage", new { roomId = rId });

        }
        if (validateDates == 0) {
            TempData["Alert"] = "Please choose different dates";
            return RedirectToAction("RoomPage", new { roomId = rId });
        }
        var isAvailable = _context.RoomReservations.Where(rr => rr.RoomId == rId &&  checkInDate <= rr.CheckOutDate && checkOutDate >= rr.CheckInDate).Count() == 0;
        if (!isAvailable) {
            TempData["Alert"] = "The room reserved in this date";
            return RedirectToAction("RoomPage", new { roomId = rId });
        }
        ViewBag.TotalAmount = (checkOutDate.Value.DayOfYear - checkInDate.Value.DayOfYear) * price;
        ViewBag.checkIn = checkInDate;
        ViewBag.checkOut = checkOutDate;
        ViewBag.roomId = rId;
        ViewBag.user = _context.UserProfiles.Where(u => u.UserId == userId).SingleOrDefault();

        GetHomePageData();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Book(string? cardNumber, string? cardCvv, string? expiryDate, int? rId, decimal? totalAmount, DateOnly? checkInDate, DateOnly? checkOutDate) {
        var userId = HttpContext.Session.GetInt32("user_id");
        if (userId == null)
        {
            return RedirectToAction("UserLogin", "Authentication");
        }
        if (expiryDate == null || cardNumber == null || cardCvv == null) {
            TempData["Alert"] = "Please fill all fields";
            return RedirectToAction("RoomPage", new { roomId = rId });
        }
        var expiryDateType = DateOnly.Parse(expiryDate);
        var creditCard = await _context.CreditCards.Where(c => c.CardNumber == cardNumber && c.CardCvv == cardCvv && expiryDateType == c.CardExpiryDate.Value && c.UserId == userId).SingleOrDefaultAsync();

        if (creditCard == null)
        {
            TempData["Alert"] = "Incorrect credit card data";
            return RedirectToAction("RoomPage", new { roomId = rId });
        }
        var isCreditAmountEnough = creditCard.CardAmount >= totalAmount;
        if (!isCreditAmountEnough)
        {
            TempData["Alert"] = "Credit card amount not enough";
            return RedirectToAction("RoomPage", new { roomId = rId });

        }
        creditCard.CardAmount -= totalAmount;
         _context.Update(creditCard);
        await _context.SaveChangesAsync();
       
        RoomReservation roomReservation = new RoomReservation();
        roomReservation.RoomId = rId;
        roomReservation.UserId = userId;
        roomReservation.CheckInDate = checkInDate;
        roomReservation.CheckOutDate = checkOutDate;
        roomReservation.TotalPrice = totalAmount;
        await _context.AddAsync(roomReservation);
        await _context.SaveChangesAsync();

        string fromEmail = _context.ContactUs.FirstOrDefault().ContactUsEmail;
        string mailTitle = "Funduq Booking Invoice";
        var userCredntials = _context.UserCredentials.Where(u => u.UserId == userId).SingleOrDefault();

        var hotelName = _context.Rooms.Where(r => r.RoomId == rId).Include(r => r.Hotel).SingleOrDefault().Hotel.HotelName;
        var roomName = _context.Rooms.Where(r => r.RoomId == rId).SingleOrDefault().RoomName;
        var userProfile = _context.UserProfiles.Where(up => up.UserId == userId).SingleOrDefault();
        _emailService.SendEmail(_pdfService, fromEmail, mailTitle, userCredntials.Email, userProfile.FirstName, userProfile.LastName, hotelName, (decimal)totalAmount, checkInDate.Value, checkOutDate.Value,roomName);


        return RedirectToAction("SuccessfullyPurchased");

    }
    public IActionResult SuccessfullyPurchased() {
        var userId = HttpContext.Session.GetInt32("user_id");
        if (userId == null)
        {
            ViewBag.user = null;
        }
        else {
            ViewBag.user = _context.UserProfiles.Where(u => u.UserId == userId).SingleOrDefault();

        }

        GetHomePageData();
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
