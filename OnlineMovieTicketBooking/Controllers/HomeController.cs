using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineMovieTicketBooking.Data;
using OnlineMovieTicketBooking.Models;
using OnlineMovieTicketBooking.Models.ViewModels;
using OnlineMovieTicketBooking.Services;

namespace OnlineMovieTicketBooking.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmailSender _emailSender;
        private ApplicationDbContext _context;
        int count = 1;
        bool flag = true;
        private UserManager<ApplicationUser> _userManager;
        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;

        }



        public IActionResult Index(int id)
        {
            
         
            List<MovieDetails> soloMovies = new List<MovieDetails>();
            List<string> names = new List<string>();
            //Get all movies to list
            var allmovies = _context.MovieDetails.ToList();
            foreach (var movie in allmovies) {
                //if movie date has expired set movie status stopped playing if status is now playing
                if (movie.PlayingUntill < DateTime.Now)
                {

                    if (movie.Status == 1)
                    {
                        movie.Status = 2;
                        _context.Update(movie);
                        _context.SaveChanges();
                    }





                }
                else
                {
                    //Get today date and the time already setted
                    DateTime date = DateTime.Today.Date + movie.DateAndTime.TimeOfDay;
                   

                    //update movie
                    movie.DateAndTime = date;
                    _context.Update(movie);
                    _context.SaveChanges();
                }


            }
            //Select all movies now playing
            var movieNames = _context.MovieDetails.Where(b=>b.Status ==1).Select(a => a.Movie_Name).ToList();
            //Get movie names without duplicates
            foreach (var item in movieNames) {
                if (!names.Contains(item)) { names.Add(item); }

            }
            //Add every first movie with name equals to each name of list to a MovieDetails List
            foreach (var name in names) {
                soloMovies.Add(_context.MovieDetails.Where(a => a.Movie_Name == name).FirstOrDefault());
            }
        //Check if user have completed personalization questions
            var preflist = _context.MovieML.Select(a=>a.UserId).ToList();
            if (!preflist.Contains(_userManager.GetUserId(HttpContext.User)))
            {
                TempData["NO"] = "NO";
            }
            else {
                TempData["NO"] = "YES";
            }
            //return movies to view
            return View(soloMovies);
        }


       
        [Authorize(Roles = "user,Admin,Special")]
        
        [HttpGet]
        public IActionResult BookNow(int Id)
        {

         
            BookNowViewModel vm = new BookNowViewModel();
            //Get selected movies MovieDetails
            var item = _context.MovieDetails.Where(a => a.Id == Id).FirstOrDefault();
            //Get seats of selected movie of specific date that are not cancelled
            var bookedSeats = _context.BookingTable.Where(d => d.MovieDetailsId == Id).Where(b => b.IsCancelled == false).Where(d=>d.Datetopresent == item.DateAndTime).Select(c => c.SeatNo).ToList();
            vm.allSeats = new List<string>();
            // Add movie Details and seats to view model
            vm.Movie_Name = item.Movie_Name;
            vm.Movie_Date = item.DateAndTime;
            vm.MovieId = Id;
        
        
            foreach(var seat in bookedSeats) { vm.allSeats.Add(seat); }
            //return view model
            return View(vm);
        }
        [Authorize(Roles = "user,Admin,Special")]
    
        [HttpPost]
        public IActionResult BookNow(BookNowViewModel vm)
        {
          
            List<MovieDetails> mv = new List<MovieDetails>();
            List<Cart> carts = new List<Cart>();
            List<UserProfile> up = new List<UserProfile>();
          
            string seatno = vm.SeatNo.ToString();
            int movieId = vm.MovieId;
            DateTime now = DateTime.Today;
            //save user details for recommendation purposes
            var dateofBirth = _userManager.Users.Where(a => a.Id == _userManager.GetUserId(HttpContext.User)).Select(b => b.DateOfBirth).FirstOrDefault().Value.Year;
            int Age = now.Year - dateofBirth;
            var MovieScore = _context.MovieDetails.Where(a => a.Id == vm.MovieId).Select(b => b.GenScore).FirstOrDefault();
            var Gender = _userManager.Users.Where(a => a.Id == _userManager.GetUserId(HttpContext.User)).Select(b => b.Sex).FirstOrDefault();

            int GenderInt;
            if (Gender == "Male")
            {
                GenderInt = 1;
            }
            else {
                GenderInt = 0;
            }

           
                up.Add(new UserProfile
                {
                    UserId = _userManager.GetUserId(HttpContext.User),
                    Age = Age,
                    Movie_Score = MovieScore,
                    Gender = GenderInt,
                    Booking_times = 1
                });
                foreach (var profile in up)
                {
                    _context.UserProfile.Add(profile);
                    _context.SaveChanges();

                }
          

            //get all seats chosen
            string[] seatnoArray = seatno.Split(',');
            count = seatnoArray.Length;
            //If seat is not allready booked
            if (checkseat(seatno, movieId) == false )
            {
                foreach (var item in seatnoArray)
                {
                    //Set Vm inputs to Cart table
                    carts.Add(new Cart { Amount = 7, MovieId = vm.MovieId, Title = vm.Movie_Name, UserId = _userManager.GetUserId(HttpContext.User), date = vm.Movie_Date, SeatNo = item ,IsChecked =false});
                 
                }
           
                foreach (var item in carts)
                {
                    _context.Cart.Add(item);
                    _context.SaveChanges();
                }
                TempData["Sucess"] = "<div class='alert alert-success col-md-4'>Your seat added succesfully to cart !!! Please visit Visit your cart to complete your reservation </div>";
            }else
            {
                TempData["Sucess"] = "<div class='alert alert-danger col-md-4'>Please Change your Seat No </div>";

            }
            return RedirectToAction("BookNow");

        }
      //Check if chosen seat is allready booked
        private bool checkseat(string seatno, int movieId)
        {
            //get chosen seats and add to alist by removing commas
            string seats = seatno;

            string[] seatreserve = seats.Split(',');
            //Check if selected seats allready exist to bookings
            var movieToBook = _context.MovieDetails.Where(a => a.Id == movieId).FirstOrDefault();
            var seatnolist = _context.BookingTable.Where(a => a.MovieDetailsId == movieId).Where(b => b.IsCancelled == false).Where(c=>c.Datetopresent == movieToBook.DateAndTime).ToList();
            foreach (var item in seatnolist)
            {
                string alreadybook = item.SeatNo;
                foreach (var item1 in seatreserve)
                {
                    int n;
                    bool isNumeric = int.TryParse(item1, out n);
                    if (item1 == alreadybook )
                    {
                        
                        flag = false;
                       // break;
                    }
                }
            }
            if (flag == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        [HttpPost]
        public IActionResult checkseat(DateTime Movie_Date , BookNowViewModel booknow  )
        {
            string seatno = string.Empty;
            bool cancelled ;
            
            var movielist = _context.BookingTable.Where(a => a.Datetopresent == Movie_Date).ToList();
       
            if (movielist != null)
            {
                var getseatno = movielist.Where(b => b.MovieDetailsId == booknow.MovieId).ToList();
                
                if(getseatno!=null)
                {
                    foreach (var item in getseatno)
                    {
                        cancelled = item.IsCancelled;
                        if (cancelled == false)
                        {
                            seatno = seatno + " " + item.SeatNo.ToString();
                        }

                    }
                    if (seatno != "")
                    {

                        TempData["SNO"] = "Seats "+ seatno + " are already booked !!";
                        
                    }
                    else
                    {
                        TempData["SNO"] = "All seats are available";
                    }
                }

            }
            return View();
        }







        [HttpGet]
        public IActionResult Checkout()
        {
            //Get all bookings of logged in user added to cart and return to List View
            var bookings = _context.Cart.Where(a => a.UserId == _userManager.GetUserId(HttpContext.User)).Where(b => b.IsChecked == false);
            return View(bookings);
        }

        [HttpGet]
        public IActionResult CheckoutFinal()
        {
            List<BookingTable> booking = new List<BookingTable>();
            List<Cart> carts = new List<Cart>();
            //get bookings that have not confirmed
            var bookings = _context.Cart.Where(a => a.UserId == _userManager.GetUserId(HttpContext.User)).Where(b=>b.IsChecked==false).ToList();

            string body = "Below you will find your reservation details : <br />";
            string details = "";
            //Save bookings to BookingTable and create email body for each booking
            foreach (var item in bookings)
            {
                item.IsChecked = true;
                _context.Cart.Update(item);
                _context.SaveChanges();
                var hall = _context.Halls.Where(a => a.Id == _context.MovieDetails.Where(b => b.Id == item.MovieId).Select(c => c.Hall).FirstOrDefault()).Select(d => d.HallName).FirstOrDefault();
                //carts.Add(new Cart { Amount = 150, MovieId = vm.MovieId, UserId = _userManager.GetUserId(HttpContext.User), date = vm.Movie_Date, SeatNo = item });
                booking.Add(new BookingTable { Amount = item.Amount,MovieName =item.Title , Datetopresent = item.date, MovieDetailsId = item.MovieId, SeatNo = item.SeatNo,UserName=_context.Users.Where(a=>a.Id == _userManager.GetUserId(HttpContext.User)).Select(b=>b.Contact_Name).FirstOrDefault(), UserId = _userManager.GetUserId(HttpContext.User) ,PhoneNumber= _context.Users.Where(a => a.Id == _userManager.GetUserId(HttpContext.User)).Select(b => b.PhoneNumber).FirstOrDefault(),Email=_userManager.GetUserName(HttpContext.User) });
                details = details + "<br />" + "<strong>Movie Name: </strong>" + item.Title + ",<strong>Seat Number: </strong>" + item.SeatNo + ",<strong>Date to present: </strong>" + item.date + ",<strong>Hall: </strong>" + hall;
                _context.Cart.Remove(item);
                _context.SaveChanges();
            }
            foreach (var item1 in booking)
                       {
                        _context.BookingTable.Add(item1);
                        _context.SaveChanges();
                    }
            string to = _userManager.GetUserName(HttpContext.User);
            string subject = "Successful Reservation";
            string fullBody = body + details;
            //send email to user
            _emailSender.SendEmailAsync(to,subject,fullBody);
               return View();
        }
        [HttpGet]
        public IActionResult EmptyCart()
        {
  //remove all bookings from cart
            var bookings = _context.Cart.Where(a => a.UserId == _userManager.GetUserId(HttpContext.User)).Where(b => b.IsChecked == false).ToList();
            foreach (var item in bookings)
            {
               
                _context.Cart.Remove(item);
                _context.SaveChanges();
            }
          
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize(Roles = "user,Admin,Special")]
        [HttpGet]
        public IActionResult Rate(int mid,double rating)
        {

            //get movie to rate
            var moviename = _context.MovieDetails.Where(a => a.Id == mid).Select(b => b.Movie_Name).FirstOrDefault();
            //get User ids that allready rated the movie
            var idlist = _context.Rates.Where(b => b.MovieName == moviename).Select(a => a.UserId).ToList();
            //If Loged in used has not rated the movie
            if (!idlist.Contains(_userManager.GetUserId(HttpContext.User)))
            {
               
                List<Rates> ratings = new List<Rates>();
               

                //Add rating to Db
                    ratings.Add(new Rates
                    {
                        UserId = _userManager.GetUserId(HttpContext.User),
                        Rating = rating,
                        MovieName = moviename
                    });

                    foreach (var rates in ratings)
                    {
                        _context.Rates.Add(rates);
                        _context.SaveChanges();
                    }
                    TempData["rateresult"] = "<div class='alert alert-success'>Thank you for your vote</div>";
                
            }
            //Else send message to user 
            else {
                TempData["rateresult"] = "<div class='alert alert-danger'>You have allready rated this movie !!</div>";

            }
            return RedirectToAction("Index");

        }



        [HttpPost]
        public IActionResult SavePrefs(string M1, string M2, string M3, string M4, string M5)
        {
            //Save selected preferences values to Db
            List<MovieML> movieml = new List<MovieML>();
            double m1 = Convert.ToDouble(M1);
            double m2 = Convert.ToDouble(M2);
            double m3 = Convert.ToDouble(M3);
            double m4 = Convert.ToDouble(M4);
            double m5 = Convert.ToDouble(M5);
            string uid = _userManager.GetUserId(HttpContext.User);
            int Age = DateTime.Now.Year -_context.Users.Where(a => a.Id == uid).Select(b => b.DateOfBirth).FirstOrDefault().Value.Year ;
            int G = 0;
            if (_context.Users.Where(a => a.Id == uid).Select(b => b.Sex).FirstOrDefault() == "Male")
            {

                G = 1;
            }
            else { G = 0; }


            movieml.Add(new MovieML { UserId = uid, Age = Age, Gen = G, M1 = m1, M2 = m2, M3 = m3, M4 = m4, M5 = m5 });
            foreach (var item in movieml) {
                _context.MovieML.Add(item);
                _context.SaveChanges();

            }

            return RedirectToAction("Index");

        }
    }
}
