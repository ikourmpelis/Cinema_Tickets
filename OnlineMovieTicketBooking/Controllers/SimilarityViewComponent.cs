using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMovieTicketBooking.Data;
using OnlineMovieTicketBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineMovieTicketBooking.ViewComponents
{
    public class SimilarityViewComponent : ViewComponent
    {

        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        public SimilarityViewComponent(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //other users preference list
            var prefs = await _context.MovieML.Where(b => b.UserId != _userManager.GetUserId(HttpContext.User)).ToListAsync();
            var userprefs = await _context.MovieML.Where(b=>b.UserId == _userManager.GetUserId(HttpContext.User)).Select(a => new { a.Age, a.Gen, a.M1, a.M2, a.M3, a.M4, a.M5 }).ToListAsync();
            //Loged in user preference list
            List<double> uspreflist = new List<double>();
            List<double> preflist = new List<double>();
            List<double> cosscores = new List<double>();
            List<string> profileId = new List<string>();
            
            if (userprefs.Count != 0)
            {
                uspreflist.Add(userprefs[0].Age);
                uspreflist.Add(userprefs[0].Gen);
                uspreflist.Add(userprefs[0].M1);
                uspreflist.Add(userprefs[0].M2);
                uspreflist.Add(userprefs[0].M3);
                uspreflist.Add(userprefs[0].M4);
                uspreflist.Add(userprefs[0].M5);
                //calculate cosine similarity between loged in user and each other user 
                for (int i = 0; i <= prefs.Count - 1; i++)
                {
                    preflist.Clear();
                    preflist.Add(prefs[i].Age);
                    preflist.Add(prefs[i].Gen);
                    preflist.Add(prefs[i].M1);
                    preflist.Add(prefs[i].M2);
                    preflist.Add(prefs[i].M3);
                    preflist.Add(prefs[i].M4);
                    preflist.Add(prefs[i].M5);

                    int N = 0;
                    N = ((preflist.Count < uspreflist.Count) ? preflist.Count : uspreflist.Count);
                    double dot = 0.0d;
                    double mag1 = 0.0d;
                    double mag2 = 0.0d;
                    for (int n = 0; n < N; n++)
                    {
                        dot += uspreflist[n] * preflist[n];
                        mag1 += Math.Pow(uspreflist[n], 2);
                        mag2 += Math.Pow(preflist[n], 2);
                    }
                 //Add similarity scores to double list
                    cosscores.Add(dot / (Math.Sqrt(mag1) * Math.Sqrt(mag2)));



                }
       //find bigest similarity score
                int indexMax
        = !cosscores.Any() ? -1 :
        cosscores
        .Select((value, index) => new { Value = value, Index = index })
        .Aggregate((a, b) => (a.Value > b.Value) ? a : b)
        .Index;
                //find user id of user with bigest similarity score
                profileId.Add(prefs[indexMax].UserId);
                var midlist = _context.BookingTable.Where(a => a.UserId == prefs[indexMax].UserId).Select(c => c.MovieDetailsId).ToList();

                Random rnd = new Random();
                int r = rnd.Next(midlist.Count);

               //if user with bigest similarity score has booked a movie playing now
                if (midlist.Count != 0)
                {
                    //return a random movie
                    var movie1 = _context.MovieDetails.Where(b=>b.Status == 1).Where(a => a.Id == midlist[r]).FirstOrDefault();
                    TempData["Test"] = "Cosine Recomendation";
                    return View(movie1);

            }
                else
                {
                    //Else find the genre loged in user has booked most
                    var scores = _context.UserProfile.Where(a => a.UserId == _userManager.GetUserId(HttpContext.User)).Select(b => b.Movie_Score).ToList();
                    //If user has booked movies
                    if (scores.Count != 0)
                    {
                        
                        int most = most = scores.GroupBy(s => s)
                                     .OrderByDescending(s => s.Count())
                                     .First().Key;
                        //return a random movie of this genre playing now
                        Random rnd2 = new Random();
                        var movielist = await _context.MovieDetails.Where(b => b.Status == 1).Where(a => a.GenScore == most).ToListAsync();
                        int r2 = rnd2.Next(movielist.Count);
                        var movie2 = movielist[r2];
                        TempData["Test"] = "Simple Recomendation";
                        return View(movie2);
                    }
                    //Else return a random movie
                    else {

                        Random rnd3 = new Random();
                        var movielist3 = await _context.MovieDetails.Where(a => a.Status == 1).ToListAsync();
                        int r3 = rnd3.Next(movielist3.Count);
                        var movie3 = movielist3[r3];
                        TempData["Test"] = "Default Recomendation";
                        return View(movie3);
                    }

                }




            }
            else {

                var scores = _context.UserProfile.Where(a => a.UserId == _userManager.GetUserId(HttpContext.User)).Select(b => b.Movie_Score).ToList();
                if (scores.Count != 0)
                {
                    int most = most = scores.GroupBy(s => s)
                                 .OrderByDescending(s => s.Count())
                                 .First().Key;
                    Random rnd2 = new Random();
                    var movielist = await _context.MovieDetails.Where(b=>b.Status==1).Where(a => a.GenScore == most).ToListAsync();
                    int r2 = rnd2.Next(movielist.Count);
                    var movie2 = movielist[r2];
                    TempData["Test"] = "Simple Recomendation";
                    return View(movie2);
                }
                else
                {

                    Random rnd3 = new Random();
                    var movielist3 = await _context.MovieDetails.Where(a => a.Status == 1).ToListAsync();
                    int r3 = rnd3.Next(movielist3.Count);
                    var movie3 = movielist3[r3];
                    TempData["Test"] = "Default Recomendation";
                    return View(movie3);
                }
            }


        }

    
    }
}