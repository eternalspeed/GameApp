using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace GameApp.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class CastsController : Controller
    {
        private readonly GameReviewDbContext _context;

        public CastsController(GameReviewDbContext context)
        {
            _context = context;
        }

        // GET: Casts
        public async Task<IActionResult> Index()
        {
            var gameReviewDbContext = _context.Casts.Include(c => c.CastRole).Include(c => c.Game);
            return View(await gameReviewDbContext.ToListAsync());
        }

        // GET: Casts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Casts == null)
            {
                return NotFound();
            }

            var cast = await _context.Casts
                .Include(c => c.CastRole)
                .Include(c => c.Game)
                .FirstOrDefaultAsync(m => m.CastId == id);
            if (cast == null)
            {
                return NotFound();
            }

            return View(cast);
        }

        // GET: Casts/Create
        public IActionResult Create()
        {
            ViewData["CastRoleId"] = new SelectList(_context.CastRoles, "CastRoleId", "CastRoleId");
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameId");
            return View();
        }

        // POST: Casts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CastId,CastRoleId,GameId,Name,Surname,Sex,Age")] Cast cast)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cast);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CastRoleId"] = new SelectList(_context.CastRoles, "CastRoleId", "CastRoleId", cast.CastRoleId);
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameId", cast.GameId);
            return View(cast);
        }

        // GET: Casts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Casts == null)
            {
                return NotFound();
            }

            var cast = await _context.Casts.FindAsync(id);
            if (cast == null)
            {
                return NotFound();
            }
            ViewData["CastRoleId"] = new SelectList(_context.CastRoles, "CastRoleId", "CastRoleId", cast.CastRoleId);
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameId", cast.GameId);
            return View(cast);
        }

        // POST: Casts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CastId,CastRoleId,GameId,Name,Surname,Sex,Age")] Cast cast)
        {
            if (id != cast.CastId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cast);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CastExists(cast.CastId))
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
            ViewData["CastRoleId"] = new SelectList(_context.CastRoles, "CastRoleId", "CastRoleId", cast.CastRoleId);
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameId", cast.GameId);
            return View(cast);
        }

        // GET: Casts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Casts == null)
            {
                return NotFound();
            }

            var cast = await _context.Casts
                .Include(c => c.CastRole)
                .Include(c => c.Game)
                .FirstOrDefaultAsync(m => m.CastId == id);
            if (cast == null)
            {
                return NotFound();
            }

            return View(cast);
        }

        // POST: Casts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Casts == null)
            {
                return Problem("Entity set 'GameReviewDbContext.Casts'  is null.");
            }
            var cast = await _context.Casts.FindAsync(id);
            if (cast != null)
            {
                _context.Casts.Remove(cast);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CastExists(int id)
        {
          return (_context.Casts?.Any(e => e.CastId == id)).GetValueOrDefault();
        }
    }
}
