using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileUploadControl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMovieTicketBooking.Data;
using OnlineMovieTicketBooking.Models;
using OnlineMovieTicketBooking.Models.ViewModels;
using OnlineMovieTicketBooking.Services;


namespace OnlineMovieTicketBooking.Controllers
{
    
   
    public class AdminController : Controller
    {
        private readonly IEmailSender _emailSender ;
        private ApplicationDbContext _context;
        private UploadInterface _upload;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,UploadInterface upload, IEmailSender emailSender)
        {
            _upload = upload;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailSender = emailSender;

        }
        [Authorize(Roles = "Admin,cashier")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        //Create Method
        [Authorize(Roles = "Admin")]
        //Return View Model to View
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(IList<IFormFile> files, MovieDetailViewModel vmodel, MovieDetails movie)
        {
            //Set MovieDetails from View Model

            movie.Movie_Name = vmodel.Name;
            movie.Movie_Description = vmodel.Description;
            movie.Movie_Genre = vmodel.Genre;
            movie.DateAndTime = vmodel.DateOfMovie;
            movie.GenScore = vmodel.GenScore;
            movie.PlayingUntill = vmodel.DateUntil;
            foreach (var item in files)
            {
                movie.MoviePicture = "~/uploads/" + item.FileName.Trim();

            }

            _upload.uploadfilemultiple(files);
            //Save to Database
            _context.MovieDetails.Add(movie);
            _context.SaveChanges();
            TempData["Sucess"] = "Save your Movie";
            return RedirectToAction("Create", "Admin");
        }

        //View Halls Method
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult ViewHalls()
        {
            //Return Halls to List View
            var halls = _context.Halls.ToList();
            return View(halls);
        }
        //Asign new hall and date to movie method
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult NewTime_Hall(int id)
        {
            //Get Movie Details of Chosen Movie
            var movie = _context.MovieDetails.Where(a => a.Id == id).FirstOrDefault();
            //Set Movie Details to View Model
            AssignViewModel vm = new AssignViewModel();
            vm.Description = movie.Movie_Description;
            vm.Genre = movie.Movie_Genre;
            vm.MoviePicture = movie.MoviePicture;
            vm.Name = movie.Movie_Name;
            vm.GenScore = movie.GenScore;
            //Return View Model
            return View(vm);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult NewTime_Hall(AssignViewModel vmodel)
        {
            //Set MovieDetails to View Model Values
            MovieDetails movie = new MovieDetails();
            movie.Movie_Name = vmodel.Name;
            movie.Movie_Description = vmodel.Description;
            movie.Movie_Genre = vmodel.Genre;
            movie.DateAndTime = vmodel.DateOfMovie;
            movie.GenScore = vmodel.GenScore;
            movie.PlayingUntill = vmodel.DateUntil;
            movie.MoviePicture = vmodel.MoviePicture;
            movie.Hall = vmodel.hall;
            //Add Movie to DB
            _context.MovieDetails.Add(movie);
            _context.SaveChanges();
         
            return RedirectToAction("CRUD", "Admin");
        }
        //Manage Movies Action
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CRUD()
        {
            // Return Movie Details For all Movies to List View
            var GetMovieDetails = _context.MovieDetails.ToList();
            return View(GetMovieDetails);
        }
        //Delete Movie action
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Delete(int Id)
        {
            //Get Selected Movie Details
            var item = _context.MovieDetails.Where(a => a.Id == Id).FirstOrDefault();
            //Remove From DB
            _context.Remove(item);
            _context.SaveChanges();
            return RedirectToAction("CRUD", "Admin");
        }
        //Delete Hall
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteHall(int Id)
        {
            //Get Selected Hall
            var item = _context.Halls.Where(a => a.Id == Id).FirstOrDefault();
            //Get All Movies Played on selected hall
            var moviestoDelete = _context.MovieDetails.Where(a => a.Hall == Id).ToList();

            //Delete Hall and movies from DB
            foreach (var movie in moviestoDelete) {
                _context.Remove(movie);
                _context.SaveChanges();
            }
            _context.Remove(item);
            _context.SaveChanges();
            return RedirectToAction("ViewHalls", "Admin");
        }
        //Edit Movie Action
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            //Get Selected Movie details and return to View
            var item = _context.MovieDetails.Where(a => a.Id == Id).FirstOrDefault();
            return View(item);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(IList<IFormFile> files,MovieDetails movie)
        {
            //Set New Movie Details
            movie.Movie_Name = movie.Movie_Name;
            movie.Movie_Description = movie.Movie_Description;
            movie.Movie_Genre = movie.Movie_Genre;
            movie.DateAndTime = movie.DateAndTime;
            movie.Status = movie.Status;
            movie.Hall = movie.Hall;
            movie.GenScore = movie.GenScore;
            if (files.Count != 0)
            {

                foreach (var item in files)
                {
                    movie.MoviePicture = "~/uploads/" + item.FileName.Trim();

                }
            }
            else {
                movie.MoviePicture = movie.MoviePicture;
            }

            _upload.uploadfilemultiple(files);
            //Update Database
            _context.MovieDetails.Update(movie);
            _context.SaveChanges();
            TempData["Sucess"] = "Save your Movie";
            return RedirectToAction("Create", "Admin");
        }
        [Authorize(Roles = "Admin,cashier")]
        //Cancel Booking Method
        [HttpGet]
        public IActionResult Edit_Booking(int Id)
        {
            //Get Selected Booking
            var item = _context.BookingTable.Where(a => a.Id == Id).FirstOrDefault();
            //Set isCancelled true and update database
            item.IsCancelled = true;
            _context.Update(item);
            _context.SaveChanges();
            return RedirectToAction("CheckBookSeatAsync", "Admin");
        }
        [Authorize(Roles = "Admin,cashier")]
        //View Bookings

        [HttpGet]
        public async Task<IActionResult> CheckBookSeatAsync()
        {
            //Get all bookings and return to view
            var bookings =await _context.BookingTable.ToListAsync();
            return View(bookings);
        }
        //View User Details
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetUserDetails()
        {
            //Get all User Details and return to list View
            var getUserTable = _context.Users.ToList();
            return View(getUserTable);
        }


        //Create Hall Action
        //Return View Model
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CreateHall()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult CreateHall( HallCreateViewModel vmodel, Halls hall)
        {

        //Set View Model input Valus to Hall Details

            hall.HallName = vmodel.HallName;
            hall.Large = vmodel.Large;

            //Add to Database
            _context.Halls.Add(hall);
            _context.SaveChanges();
            TempData["Sucess"] = "Save your Hall";
            return RedirectToAction("CreateHall", "Admin");
        }
        //Asign new role Action
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> AsignRoleAsync(string uid, string role)
        {
            //Get selected UserId User and role to asign from View
            var user = _context.Users.Where(a => a.Id == uid).FirstOrDefault();
            //Check if new role exists
            bool x = await _roleManager.RoleExistsAsync(role);
            //If No create the new role and asign it to user
            if (!x)
            {


                var Role = new IdentityRole();
                Role.Name = role;
                await _roleManager.CreateAsync(Role);
                var roles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, roles.ToArray());
                await _userManager.AddToRoleAsync(user, role);
               


            }
            //if Yes asign role to user
            else {
                var roles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, roles.ToArray());
                await _userManager.AddToRoleAsync(user, role);



            }
          
            return RedirectToAction("GetUserDetails", "Admin");
        }
        //Get Booking of selected date method a
        [Authorize(Roles = "Admin")]
        [HttpPost]

        public IActionResult ShowByDate(DateTime date)
        {
            //get bookings of selected date and return to view
            var bookings = _context.BookingTable.Where(a => a.Datetopresent.Date == date).ToList();
            return View(bookings);
        }
        //Monthly statistics by year method

        [Authorize(Roles = "Admin")]
        public IActionResult Bar(string Year)
        {
            //Get all dates of bookings of selected year that are not cancelled 
            var bookingsByMonth = _context.BookingTable.Where(a => a.IsCancelled == false).Where(c=>c.Datetopresent.Year.ToString() == Year).Select(b => b.Datetopresent).ToList();

            // Select Bookings quantities for each month and create SimpleReportViewModels
            var lstModel = new List<SimpleReportViewModel>();
          
            lstModel.Add(new SimpleReportViewModel
            {
                DimensionOne = "January",
                Quantity = bookingsByMonth.Where(a => a.Month.ToString() == "1").ToList().Count 
            });
            lstModel.Add(new SimpleReportViewModel
            {
                DimensionOne = "February",
                Quantity = bookingsByMonth.Where(a => a.Month.ToString() == "2").ToList().Count
            });
            lstModel.Add(new SimpleReportViewModel
            {
                DimensionOne = "March",
                Quantity = bookingsByMonth.Where(a => a.Month.ToString() == "3").ToList().Count
            });
            lstModel.Add(new SimpleReportViewModel
            {
                DimensionOne = "April",
                Quantity = bookingsByMonth.Where(a => a.Month.ToString() == "4").ToList().Count
            });
            lstModel.Add(new SimpleReportViewModel
            {
                DimensionOne = "May",
                Quantity = bookingsByMonth.Where(a => a.Month.ToString() == "5").ToList().Count
            });
            lstModel.Add(new SimpleReportViewModel
            {
                DimensionOne = "June",
                Quantity = bookingsByMonth.Where(a => a.Month.ToString() == "6").ToList().Count
            });
            lstModel.Add(new SimpleReportViewModel
            {
                DimensionOne = "July",
                Quantity = bookingsByMonth.Where(a => a.Month.ToString() == "7").ToList().Count
            });
            lstModel.Add(new SimpleReportViewModel
            {
                DimensionOne = "August",
                Quantity = bookingsByMonth.Where(a => a.Month.ToString() == "8").ToList().Count
            });
            lstModel.Add(new SimpleReportViewModel
            {
                DimensionOne = "September",
                Quantity = bookingsByMonth.Where(a => a.Month.ToString() == "9").ToList().Count
            });
            lstModel.Add(new SimpleReportViewModel
            {
                DimensionOne = "Octomber",
                Quantity = bookingsByMonth.Where(a => a.Month.ToString() == "10").ToList().Count
            });
            lstModel.Add(new SimpleReportViewModel
            {
                DimensionOne = "November",
                Quantity = bookingsByMonth.Where(a => a.Month.ToString() == "11").ToList().Count
            });
            lstModel.Add(new SimpleReportViewModel
            {
                DimensionOne = "December",
                Quantity = bookingsByMonth.Where(a => a.Month.ToString() == "12").ToList().Count
            });
            //Retutn to View
            return View(lstModel);
        }


        [Authorize(Roles = "Admin")]
        //Cancel movie action
        [HttpGet]
        public IActionResult CancelMovie(int id)
        {
            // Get Movie to be cancelled
            var movieToCancel = _context.MovieDetails.Where(a => a.Id == id).FirstOrDefault();
            //Get all bookings of selected movie
            var bookings = _context.BookingTable.Where(a => a.MovieDetailsId == id).Where(c => c.Datetopresent == movieToCancel.DateAndTime).ToList();
            //Cancel all bookings of selected movie
            foreach (var movie in bookings) {
                movie.IsCancelled = true;
                _context.Update(movie);
                _context.SaveChanges();
            }
            //Set Movie status to canceled
            movieToCancel.Status = 0;
            _context.Update(movieToCancel);
            _context.SaveChanges();
            //Select emails of users booked the canceled movie
            var usersBooked = _context.BookingTable.Where(a => a.MovieDetailsId == id).Select(b => b.UserName).ToList();
            //Notify users via email that movie is cancelled
            string subject = movieToCancel.Movie_Name + " projection cancelled";
            string body = "We are sorry to inform you that the projection of  " + movieToCancel.Movie_Name + " at " + movieToCancel.DateAndTime + " has been cancelled.Please contact us for further details and compensation."; 
            foreach (var item in usersBooked) {

                _emailSender.SendEmailAsync(item, subject, body);

            }
            return RedirectToAction("CRUD", "Admin");
        }
    }
}