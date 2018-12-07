using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineMovieTicketBooking.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int Movie_Score { get; set; }
        public int Age { get; set; }
        public int Gender { get; set; }
        public int Booking_times { get; set; }
    }
}
