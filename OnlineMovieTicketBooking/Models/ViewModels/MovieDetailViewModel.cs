using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineMovieTicketBooking.Models.ViewModels
{
    public class MovieDetailViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public DateTime DateOfMovie { get; set; }
        public DateTime DateUntil { get; set; }
        public string MoviePicture { get; set; }
        public int GenScore { get; set; }
        public int hall { get; set; }
    }
}
