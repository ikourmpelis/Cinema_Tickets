using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineMovieTicketBooking.Models.ViewModels
{
    public class BookingsViewModel
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Movie_Time { get; set; }
        public string Contact_Name { get; set; }
        public string Seat_No { get; set; }
        public string Email { get; set; }
        public string Contact_No { get; set; }
    }
}
