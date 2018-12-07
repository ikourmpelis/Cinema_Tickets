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
    public class WeeklyStatsViewComponent : ViewComponent
    {

        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        public WeeklyStatsViewComponent(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var bookingsByDay =await _context.BookingTable.Where(a=>a.IsCancelled==false).ToListAsync();
            var lstModel = new List<SimpleReportViewModel>();
     
            lstModel.Add(new SimpleReportViewModel
            {
                DimensionOne = "Monday",
                Quantity = bookingsByDay.Where(a=>a.Datetopresent.DayOfWeek.ToString() == "Monday").ToList().Count
            });
            lstModel.Add(new SimpleReportViewModel
            {
                DimensionOne = "Tuesday",
                Quantity = bookingsByDay.Where(a => a.Datetopresent.DayOfWeek.ToString() == "Tuesday").ToList().Count
            });
            lstModel.Add(new SimpleReportViewModel
            {
                DimensionOne = "Wednesday",
                Quantity = bookingsByDay.Where(a => a.Datetopresent.DayOfWeek.ToString() == "Wednesday").ToList().Count
            });
            lstModel.Add(new SimpleReportViewModel
            {
                DimensionOne = "Thursday",
                Quantity = bookingsByDay.Where(a => a.Datetopresent.DayOfWeek.ToString() == "Thursday").ToList().Count
            });
            lstModel.Add(new SimpleReportViewModel
            {
                DimensionOne = "Friday",
                Quantity = bookingsByDay.Where(a => a.Datetopresent.DayOfWeek.ToString() == "Friday").ToList().Count
            });
            lstModel.Add(new SimpleReportViewModel
            {
                DimensionOne = "Saturday",
                Quantity = bookingsByDay.Where(a => a.Datetopresent.DayOfWeek.ToString() == "Saturday").ToList().Count
            });
            lstModel.Add(new SimpleReportViewModel
            {
                DimensionOne = "Sunday",
                Quantity = bookingsByDay.Where(a => a.Datetopresent.DayOfWeek.ToString() == "Sunday").ToList().Count
            });
      
         
            return View(lstModel);
        }
    }
}

