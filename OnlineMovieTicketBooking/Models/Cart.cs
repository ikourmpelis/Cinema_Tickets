using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineMovieTicketBooking.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SeatNo { get; set; }
        public string UserId { get; set; }
        public DateTime date { get; set; }
        public int Amount { get; set; }
        public int MovieId { get; set; }
        public bool IsChecked { get; set; }





    }
}
