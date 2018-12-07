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
    public class HallTableViewComponent : ViewComponent
    {

        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        public HallTableViewComponent(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(int hid)
        {
            //Get hall of selected movie and return to view
            var movieHall = await _context.MovieDetails.Where(a => a.Id == hid).Select(b => b.Hall).FirstOrDefaultAsync();
            var hall = await _context.Halls.Where(a => a.Id == movieHall).FirstOrDefaultAsync();

            return View(hall);
        }
    }
}
