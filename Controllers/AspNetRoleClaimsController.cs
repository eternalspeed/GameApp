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
    public class AspNetRoleClaimsController : Controller
    {
        private readonly GameReviewDbContext _context;

        public AspNetRoleClaimsController(GameReviewDbContext context)
        {
            _context = context;
        }

        // GET: AspNetRoleClaims
        public async Task<IActionResult> Index()
        {
            var gameReviewDbContext = _context.AspNetRoleClaims.Include(a => a.Role);
            return View(await gameReviewDbContext.ToListAsync());
        }

        // GET: AspNetRoleClaims/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AspNetRoleClaims == null)
            {
                return NotFound();
            }

            var aspNetRoleClaim = await _context.AspNetRoleClaims
                .Include(a => a.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aspNetRoleClaim == null)
            {
                return NotFound();
            }

            return View(aspNetRoleClaim);
        }

        // GET: AspNetRoleClaims/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.AspNetRoles, "Id", "Id");
            return View();
        }

        // POST: AspNetRoleClaims/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RoleId,ClaimType,ClaimValue")] AspNetRoleClaim aspNetRoleClaim)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aspNetRoleClaim);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.AspNetRoles, "Id", "Id", aspNetRoleClaim.RoleId);
            return View(aspNetRoleClaim);
        }

        // GET: AspNetRoleClaims/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AspNetRoleClaims == null)
            {
                return NotFound();
            }

            var aspNetRoleClaim = await _context.AspNetRoleClaims.FindAsync(id);
            if (aspNetRoleClaim == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.AspNetRoles, "Id", "Id", aspNetRoleClaim.RoleId);
            return View(aspNetRoleClaim);
        }

        // POST: AspNetRoleClaims/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RoleId,ClaimType,ClaimValue")] AspNetRoleClaim aspNetRoleClaim)
        {
            if (id != aspNetRoleClaim.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aspNetRoleClaim);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AspNetRoleClaimExists(aspNetRoleClaim.Id))
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
            ViewData["RoleId"] = new SelectList(_context.AspNetRoles, "Id", "Id", aspNetRoleClaim.RoleId);
            return View(aspNetRoleClaim);
        }

        // GET: AspNetRoleClaims/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AspNetRoleClaims == null)
            {
                return NotFound();
            }

            var aspNetRoleClaim = await _context.AspNetRoleClaims
                .Include(a => a.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aspNetRoleClaim == null)
            {
                return NotFound();
            }

            return View(aspNetRoleClaim);
        }

        // POST: AspNetRoleClaims/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AspNetRoleClaims == null)
            {
                return Problem("Entity set 'GameReviewDbContext.AspNetRoleClaims'  is null.");
            }
            var aspNetRoleClaim = await _context.AspNetRoleClaims.FindAsync(id);
            if (aspNetRoleClaim != null)
            {
                _context.AspNetRoleClaims.Remove(aspNetRoleClaim);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AspNetRoleClaimExists(int id)
        {
          return (_context.AspNetRoleClaims?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
