using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMovieTicketBooking.Data;
using OnlineMovieTicketBooking.Models;
using OnlineMovieTicketBooking.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineMovieTicketBooking.ViewComponents
{
    public class YearSelectorViewComponent : ViewComponent
    {

        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        public YearSelectorViewComponent(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<YearViewModel> vm = new List<YearViewModel>();
            List<int> solo = new List<int>();
            var years = await _context.BookingTable.Select(a => a.Datetopresent.Year).ToListAsync();

            foreach (var year in years) {
                if (!solo.Contains(year)) { solo.Add(year); }
               
               
            }
            foreach (var year in solo)
            {

                vm.Add(new YearViewModel { Year = year });
            }
            return View(vm);
        }
    }
}
