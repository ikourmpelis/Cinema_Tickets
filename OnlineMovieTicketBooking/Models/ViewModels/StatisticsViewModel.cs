using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineMovieTicketBooking.Models.ViewModels
{
    public class StatisticsViewModel
    {
        public string BookingsByMonth { get; set; }
        public string BookingsByDays { get; set; }
        public string BookingsByYear { get; set; }
    }
}
