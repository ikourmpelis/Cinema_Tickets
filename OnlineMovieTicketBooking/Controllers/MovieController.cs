using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnlineMovieTicketBooking.Models;
using OnlineMovieTicketBooking.Services;

namespace OnlineMovieTicketBooking.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovie _movieService;

        public MovieController(IMovie movieService)
        {
            _movieService = movieService;
        }


        public async Task<IActionResult> Index(string query)
   
        {
            //get json of selcted movie from imdb
            string json = await _movieService.DownlaodApiDataAsync(query);
            //set json values to movie view model and return to view
            var movie = JsonConvert.DeserializeObject<ImdbDetails>(json);
         
            
            return View(movie);
        }
    }
}