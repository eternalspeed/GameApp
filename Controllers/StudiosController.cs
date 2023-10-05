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
    public class StudiosController : Controller
    {
        private readonly GameReviewDbContext _context;

        public StudiosController(GameReviewDbContext context)
        {
            _context = context;
        }

        // GET: Studios
        public async Task<IActionResult> Index()
        {
            var gameReviewDbContext = _context.Studios.Include(s => s.StudioRole);
            return View(await gameReviewDbContext.ToListAsync());
        }

        // GET: Studios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Studios == null)
            {
                return NotFound();
            }

            var studio = await _context.Studios
                .Include(s => s.StudioRole)
                .FirstOrDefaultAsync(m => m.StudioId == id);
            if (studio == null)
            {
                return NotFound();
            }

            return View(studio);
        }

        // GET: Studios/Create
        public IActionResult Create()
        {
            ViewData["StudioRoleId"] = new SelectList(_context.StudioRoles, "StudioRoleId", "StudioRoleId");
            return View();
        }

        // POST: Studios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudioId,StudioRoleId,StudioName,Website")] Studio studio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(studio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudioRoleId"] = new SelectList(_context.StudioRoles, "StudioRoleId", "StudioRoleId", studio.StudioRoleId);
            return View(studio);
        }

        // GET: Studios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Studios == null)
            {
                return NotFound();
            }

            var studio = await _context.Studios.FindAsync(id);
            if (studio == null)
            {
                return NotFound();
            }
            ViewData["StudioRoleId"] = new SelectList(_context.StudioRoles, "StudioRoleId", "StudioRoleId", studio.StudioRoleId);
            return View(studio);
        }

        // POST: Studios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudioId,StudioRoleId,StudioName,Website")] Studio studio)
        {
            if (id != studio.StudioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudioExists(studio.StudioId))
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
            ViewData["StudioRoleId"] = new SelectList(_context.StudioRoles, "StudioRoleId", "StudioRoleId", studio.StudioRoleId);
            return View(studio);
        }

        // GET: Studios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Studios == null)
            {
                return NotFound();
            }

            var studio = await _context.Studios
                .Include(s => s.StudioRole)
                .FirstOrDefaultAsync(m => m.StudioId == id);
            if (studio == null)
            {
                return NotFound();
            }

            return View(studio);
        }

        // POST: Studios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Studios == null)
            {
                return Problem("Entity set 'GameReviewDbContext.Studios'  is null.");
            }
            var studio = await _context.Studios.FindAsync(id);
            if (studio != null)
            {
                _context.Studios.Remove(studio);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudioExists(int id)
        {
          return (_context.Studios?.Any(e => e.StudioId == id)).GetValueOrDefault();
        }
    }
}
