using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameApp.Models;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using static System.Formats.Asn1.AsnWriter;

namespace GameApp.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class GamesController : Controller
    {
        private readonly GameReviewDbContext _context;

        public GamesController(GameReviewDbContext context)
        {
            _context = context;
        }

        // GET: Games
        public async Task<IActionResult> Index()
        {
            var gameReviewDbContext = _context.Games.Include(g => g.Series).Include(g => g.Studio);
            return View(await gameReviewDbContext.ToListAsync());
        }

        // GET: Games/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Games == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .Include(g => g.Series)
                .Include(g => g.Studio)
                .FirstOrDefaultAsync(m => m.GameId == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // GET: Games/Create
        public IActionResult Create(int seriesID, int studioID)
        {
            ViewData["SeriesId"] = new SelectList(_context.Series, "SeriesId", "SeriesId");
            ViewData["StudioId"] = new SelectList(_context.Studios, "StudioId", "StudioId");

            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GameId,SeriesId,StudioId,ImageUrl,GameTitle,Genere,Platform,Engine,Mode,ReleaseDate,Score,Description")] Game game)
        {
            if (ModelState.IsValid)
            {
                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SeriesId"] = new SelectList(_context.Series, "SeriesId", "SeriesId", game.SeriesId);
            ViewData["StudioId"] = new SelectList(_context.Studios, "StudioId", "StudioId", game.StudioId);
            return View(game);
        }

        // GET: Games/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Games == null)
            {
                return NotFound();
            }

            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            ViewData["SeriesId"] = new SelectList(_context.Series, "SeriesId", "SeriesId", game.SeriesId);
            ViewData["StudioId"] = new SelectList(_context.Studios, "StudioId", "StudioId", game.StudioId);
            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GameId,SeriesId,StudioId,ImageUrl,GameTitle,Genere,Platform,Engine,Mode,ReleaseDate,Score,Description")] Game game)
        {
            if (id != game.GameId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.GameId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SeriesId"] = new SelectList(_context.Series, "SeriesId", "SeriesId", game.SeriesId);
            ViewData["StudioId"] = new SelectList(_context.Studios, "StudioId", "StudioId", game.StudioId);
            return View(game);
        }

        // GET: Games/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Games == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .Include(g => g.Series)
                .Include(g => g.Studio)
                .FirstOrDefaultAsync(m => m.GameId == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Games == null)
            {
                return Problem("Entity set 'GameReviewDbContext.Games'  is null.");
            }
            var game = await _context.Games.FindAsync(id);
            if (game != null)
            {
                _context.Games.Remove(game);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameExists(int id)
        {
          return (_context.Games?.Any(e => e.GameId == id)).GetValueOrDefault();
        }
    }
}
