using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace OnlineMovieTicketBooking.Services
{
    public class MovieService : IMovie
    {
  

        private string BASE_URL = "http://www.omdbapi.com/";
        private string API_KEY = "&apikey=55c6cc63";

        public async Task<string> DownlaodApiDataAsync(string searchQuery)
        {
            string _MovieJson;
            string url = BASE_URL + "?t=" + searchQuery + API_KEY+ "&plot=full";
            using (HttpClient hc = new HttpClient())
            {
                string justforquery = searchQuery;

                var Moviejson = await hc.GetStringAsync(url);
                _MovieJson = Moviejson;

            }

            return _MovieJson;
        }
    }
}
