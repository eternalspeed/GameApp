using GameApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using static System.Formats.Asn1.AsnWriter;

namespace GameApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GameReviewDbContext _context;

        public HomeController(ILogger<HomeController> logger, GameReviewDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Average Score
        public static decimal? AvgScore(int id)
        {
            GameReviewDbContext db = new GameReviewDbContext();
            var reviews = (from x in db.Reviews where x.GameId.Equals(id) select x).ToList();
            decimal? avg = (from x in reviews select x.Score).Average();
            return avg;
        }
        public async Task<IActionResult> Index()
        {
            var allGames = _context.Games.Include(n => n.Studio).Include(g => g.Casts).Include(g => g.Series).Include(g => g.Awards);
            return View(await allGames.ToListAsync());
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Management()
        {
            return View();
        } 
        
        [Authorize(Roles = "User, Administrator")]
        public async Task<IActionResult> Artworks()
        {
            var gameReviewDbContext = _context.Artworks.Include(a => a.Game).Include(a => a.User);
            return View(await gameReviewDbContext.ToListAsync());
        } 
        
        public async Task<IActionResult> Awards(int id)
        {
            var allAwards = _context.Awards.Include(a => a.Game).Include(a => a.User);
            string game = id.ToString();

            if (!string.IsNullOrEmpty(game))
            {
                var filteredResult = allAwards.Where(n => n.GameId.ToString().Equals(game.ToLower())).ToList();

                return View("Awards", filteredResult);
            }

            return View("Awards", allAwards);
        }
        
        public async Task<IActionResult> Casts(int id)
        {
            var allCasts = _context.Casts.Include(a => a.Game).Include(a => a.CastRole);
            string game = id.ToString();

            if (!string.IsNullOrEmpty(game))
            {
                var filteredResult = allCasts.Where(n => n.GameId.ToString().Contains(game.ToLower())).ToList();

                return View("Casts", filteredResult);
            }

            return View("Casts", allCasts);
        }

        public async Task<IActionResult> Reviews(int id)
        {
            var allReviews = _context.Reviews.Include(a => a.Game).Include(a => a.User);
            string game = id.ToString();

            if (!string.IsNullOrEmpty(game))
            {
                var filteredResult = allReviews.Where(n => n.GameId.ToString().Equals(game.ToLower())).ToList();

                return View("Reviews", filteredResult);
            }

            return View("Reviews", allReviews);
        }

        // Search
        [AllowAnonymous]
        public async Task<IActionResult> Filter(string searchString)
        {
            var allGames = _context.Games.Include(n => n.Studio).Include(g => g.Series);

            if (!string.IsNullOrEmpty(searchString))
            {
                var filteredResult = allGames.Where(n => n.GameTitle.ToLower().Contains(searchString.ToLower()) || n.Description.ToLower().Contains(searchString.ToLower())).ToList();

                return View("Index", filteredResult);
            }

            return View("Index", allGames);
        }

        //GET: Game/Details/1
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            if (id == null || _context.Games == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .Include(g => g.Casts)
                .Include(g => g.Series)
                .Include(g => g.Studio)
                .Include(g => g.Awards)
                .FirstOrDefaultAsync(m => m.GameId == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // GET: Reviews/Create
        [Authorize]
        public IActionResult CreateReview(int id)
        {
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameId", id);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", User.Identity.Name);
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReview([Bind("ReviewId,UserId,GameId,ReviewTitle,Score,Body")] Review review)
        {
            if (ModelState.IsValid)
            {
                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameId", review.GameId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", review.UserId);
            return View(review);
        }

        // GET: Awards/Create
        [Authorize]
        public IActionResult CreateAward(int id)
        {
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameId", id);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: Awards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAward([Bind("AwardId,UserId,GameId,AwardName")] Award award)
        {
            if (ModelState.IsValid)
            {
                _context.Add(award);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameId", award.GameId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", award.UserId);
            return View(award);
        }

        // GET: Artworks/Create
        [Authorize]
        public IActionResult CreateArtwork()
        {
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameId");
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: Artworks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateArtwork([Bind("ArtworkId,UserId,GameId,ArtworkUrl,ArtworkTitle,Type,Description")] Artwork artwork)
        {
            if (ModelState.IsValid)
            {
                _context.Add(artwork);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameId", artwork.GameId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", artwork.UserId);
            return View(artwork);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}