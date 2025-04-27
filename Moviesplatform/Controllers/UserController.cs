//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace Moviesplatform.Controllers
//{
//    [Authorize(Roles = "User")]

//    public class UserController : Controller
//    {
//        public IActionResult Index()
//        {
//            return View();
//        }
//    }
//}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moviesplatform.Areas.Identity.Data;

using Moviesplatform.Data;
using Moviesplatform.Models;


namespace Moviesplatform.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private readonly MoviesplatformDBContext _context;
        private readonly UserManager<MoviesplatformUser> _userManager;


       

        public UserController(MoviesplatformDBContext context, UserManager<MoviesplatformUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> ListAll()
        {
            var userId = _userManager.GetUserId(User);

            var userWatchlist = await _context.WatchLists
                .Where(w => w.UserId == userId)
                .Include(w => w.WatchlistItems)
                .ToListAsync();

            var allFilms = await _context.Films.ToListAsync();
            var allSeries = await _context.Series.ToListAsync();

            var model = new List<FilmSerieViewModel>();

            foreach (var film in allFilms)
            {
                bool isInWatchlist = userWatchlist.SelectMany(w => w.WatchlistItems).Any(wi => wi.FilmId == film.Id);

                model.Add(new FilmSerieViewModel
                {
                    Id = film.Id,
                    Title = film.Titre,
                    IsMovie = true,
                    IsInWatchlist = isInWatchlist
                });
            }

            foreach (var serie in allSeries)
            {
                bool isInWatchlist = userWatchlist.SelectMany(w => w.WatchlistItems).Any(wi => wi.SeriesId == serie.Id);

                model.Add(new FilmSerieViewModel
                {
                    Id = serie.Id,
                    Title = serie.Titre,
                    IsMovie = false,
                    IsInWatchlist = isInWatchlist
                });
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddToWatchlist(int id, bool isMovie)
        {
            var userId = _userManager.GetUserId(User);

            var watchlist = await _context.WatchLists
                .Include(w => w.WatchlistItems)
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (watchlist == null)
            {
                watchlist = new Watchlist()
                {
                    UserId = userId,
                    WatchlistItems = new List<WachListItem>()
                };
                _context.WatchLists.Add(watchlist);
            }

            bool alreadyExists = isMovie
                ? watchlist.WatchlistItems.Any(wi => wi.FilmId == id)
                : watchlist.WatchlistItems.Any(wi => wi.SeriesId == id);

            if (!alreadyExists)
            {
                var item = new WachListItem
                {
                    Watchlist = watchlist,
                    FilmId = isMovie ? id : (int?)null,
                    SeriesId = isMovie ? (int?)null : id
                };
                watchlist.WatchlistItems.Add(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(ListAll));
        }
        [HttpPost]
        public async Task<IActionResult> RemoveFromWatchlist(int id, bool isMovie)
        {
            var userId = _userManager.GetUserId(User);

            var watchlist = await _context.WatchLists
                .Include(w => w.WatchlistItems)
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (watchlist != null)
            {
                var item = isMovie
                    ? watchlist.WatchlistItems.FirstOrDefault(wi => wi.FilmId == id)
                    : watchlist.WatchlistItems.FirstOrDefault(wi => wi.SeriesId == id);

                if (item != null)
                {
                    _context.Remove(item);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction(nameof(ListAll));
        }

    }
}
