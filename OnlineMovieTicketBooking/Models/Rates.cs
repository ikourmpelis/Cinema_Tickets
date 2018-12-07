using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineMovieTicketBooking.Models
{
    public class Rates
    {
        public int Id { get; set; }
        public string MovieName { get; set; }
        public string UserId { get; set; }
        public double Rating { get; set; }
    }
}
