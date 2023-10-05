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
    public class StudioRolesController : Controller
    {
        private readonly GameReviewDbContext _context;

        public StudioRolesController(GameReviewDbContext context)
        {
            _context = context;
        }

        // GET: StudioRoles
        public async Task<IActionResult> Index()
        {
              return _context.StudioRoles != null ? 
                          View(await _context.StudioRoles.ToListAsync()) :
                          Problem("Entity set 'GameReviewDbContext.StudioRoles'  is null.");
        }

        // GET: StudioRoles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.StudioRoles == null)
            {
                return NotFound();
            }

            var studioRole = await _context.StudioRoles
                .FirstOrDefaultAsync(m => m.StudioRoleId == id);
            if (studioRole == null)
            {
                return NotFound();
            }

            return View(studioRole);
        }

        // GET: StudioRoles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StudioRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudioRoleId,StudioRoleName")] StudioRole studioRole)
        {
            if (ModelState.IsValid)
            {
                _context.Add(studioRole);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(studioRole);
        }

        // GET: StudioRoles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.StudioRoles == null)
            {
                return NotFound();
            }

            var studioRole = await _context.StudioRoles.FindAsync(id);
            if (studioRole == null)
            {
                return NotFound();
            }
            return View(studioRole);
        }

        // POST: StudioRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudioRoleId,StudioRoleName")] StudioRole studioRole)
        {
            if (id != studioRole.StudioRoleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studioRole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudioRoleExists(studioRole.StudioRoleId))
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
            return View(studioRole);
        }

        // GET: StudioRoles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.StudioRoles == null)
            {
                return NotFound();
            }

            var studioRole = await _context.StudioRoles
                .FirstOrDefaultAsync(m => m.StudioRoleId == id);
            if (studioRole == null)
            {
                return NotFound();
            }

            return View(studioRole);
        }

        // POST: StudioRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.StudioRoles == null)
            {
                return Problem("Entity set 'GameReviewDbContext.StudioRoles'  is null.");
            }
            var studioRole = await _context.StudioRoles.FindAsync(id);
            if (studioRole != null)
            {
                _context.StudioRoles.Remove(studioRole);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudioRoleExists(int id)
        {
          return (_context.StudioRoles?.Any(e => e.StudioRoleId == id)).GetValueOrDefault();
        }
    }
}
