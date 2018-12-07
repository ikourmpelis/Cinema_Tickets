using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineMovieTicketBooking.Models
{
    public class Halls
    {
        public int Id { get; set; }
        public String HallName { get; set; }
        public bool Large { get; set; } = false;
    }
}
