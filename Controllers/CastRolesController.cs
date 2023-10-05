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
    public class CastRolesController : Controller
    {
        private readonly GameReviewDbContext _context;

        public CastRolesController(GameReviewDbContext context)
        {
            _context = context;
        }

        // GET: CastRoles
        public async Task<IActionResult> Index()
        {
              return _context.CastRoles != null ? 
                          View(await _context.CastRoles.ToListAsync()) :
                          Problem("Entity set 'GameReviewDbContext.CastRoles'  is null.");
        }

        // GET: CastRoles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CastRoles == null)
            {
                return NotFound();
            }

            var castRole = await _context.CastRoles
                .FirstOrDefaultAsync(m => m.CastRoleId == id);
            if (castRole == null)
            {
                return NotFound();
            }

            return View(castRole);
        }

        // GET: CastRoles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CastRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CastRoleId,CastRoleName")] CastRole castRole)
        {
            if (ModelState.IsValid)
            {
                _context.Add(castRole);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(castRole);
        }

        // GET: CastRoles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CastRoles == null)
            {
                return NotFound();
            }

            var castRole = await _context.CastRoles.FindAsync(id);
            if (castRole == null)
            {
                return NotFound();
            }
            return View(castRole);
        }

        // POST: CastRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CastRoleId,CastRoleName")] CastRole castRole)
        {
            if (id != castRole.CastRoleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(castRole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CastRoleExists(castRole.CastRoleId))
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
            return View(castRole);
        }

        // GET: CastRoles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CastRoles == null)
            {
                return NotFound();
            }

            var castRole = await _context.CastRoles
                .FirstOrDefaultAsync(m => m.CastRoleId == id);
            if (castRole == null)
            {
                return NotFound();
            }

            return View(castRole);
        }

        // POST: CastRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CastRoles == null)
            {
                return Problem("Entity set 'GameReviewDbContext.CastRoles'  is null.");
            }
            var castRole = await _context.CastRoles.FindAsync(id);
            if (castRole != null)
            {
                _context.CastRoles.Remove(castRole);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CastRoleExists(int id)
        {
          return (_context.CastRoles?.Any(e => e.CastRoleId == id)).GetValueOrDefault();
        }
    }
}
