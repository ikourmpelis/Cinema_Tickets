using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineMovieTicketBooking.Services
{
    
        public interface IMovie
        {
            Task<string> DownlaodApiDataAsync(string searchQuery);
        }
    
}
