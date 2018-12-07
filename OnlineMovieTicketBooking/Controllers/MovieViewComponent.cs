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
    public class MovieViewComponent : ViewComponent
    {

        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        public MovieViewComponent(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(int scr)
        {
            // Choose a random movie from list where Genre score equals to selected
            Random rnd = new Random();
            var movielist = await _context.MovieDetails.Where(a=>a.GenScore == scr).ToListAsync();
            int r = rnd.Next(movielist.Count);
            //return random movie
            return View(movielist[r]);
        }
    }
}
