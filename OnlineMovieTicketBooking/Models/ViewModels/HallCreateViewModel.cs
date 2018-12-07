using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineMovieTicketBooking.Models.ViewModels
{
    public class HallCreateViewModel
    {
        public String HallName { get; set; }
        public bool Large { get; set; } = false;
    }
}
