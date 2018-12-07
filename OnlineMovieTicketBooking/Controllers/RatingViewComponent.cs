using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMovieTicketBooking.Data;
using OnlineMovieTicketBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineMovieTicketBooking.ViewComponents
{
    public class RatingViewComponent : ViewComponent
    {

        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        public RatingViewComponent(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string moviename)
        {
            //Get rating of Movie name selected and reurn TempDatas
            var ratings = await _context.Rates.Where(a => a.MovieName == moviename).Select(b => b.Rating).ToListAsync();
            if (ratings.Count > 0)
            {
                double final = ratings.Sum() / ratings.Count;
                TempData["Rating"] = "Overall Rating : "+ final+"/5"+" ("+ratings.Count+" Votes"+")";
            }
            else {
                TempData["Rating"] = "Be the first to rate this movie";
            }
            return View();

            
        }
    }
}
