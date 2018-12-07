using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineMovieTicketBooking.Models
{
    public class BookingTable
    {

        public int Id { get; set; }
        public string SeatNo { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Datetopresent { get; set; }
        public int MovieDetailsId { get; set; }
        public string MovieName { get; set; }
        public int Amount { get; set; }
        public string Email { get; set; }
        public bool IsCancelled { get; set; } = false;
        [ForeignKey("MovieDetailsId")]
        public virtual MovieDetails moviedetails { get; set; }

}
}
