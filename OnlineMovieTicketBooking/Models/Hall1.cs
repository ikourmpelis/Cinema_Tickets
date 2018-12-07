using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineMovieTicketBooking.Models
{
    public class Hall1
    {

        public int Id { get; set; }
        public DateTime MovieDate { get; set; }
        public string MovieName { get; set; }
        public int MovieID { get; set; }
        [ForeignKey("MovieDetailsId")]
        public virtual MovieDetails moviedetails { get; set; }
    }
}
