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
    public class HallsSelectViewComponent : ViewComponent
    {

        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        public HallsSelectViewComponent (ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //Return to view all halls
            var halls= await _context.Halls.ToListAsync();

            return View(halls);
        }
    }
}
