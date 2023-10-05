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
    public class AspNetUserClaimsController : Controller
    {
        private readonly GameReviewDbContext _context;

        public AspNetUserClaimsController(GameReviewDbContext context)
        {
            _context = context;
        }

        // GET: AspNetUserClaims
        public async Task<IActionResult> Index()
        {
            var gameReviewDbContext = _context.AspNetUserClaims.Include(a => a.User);
            return View(await gameReviewDbContext.ToListAsync());
        }

        // GET: AspNetUserClaims/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AspNetUserClaims == null)
            {
                return NotFound();
            }

            var aspNetUserClaim = await _context.AspNetUserClaims
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aspNetUserClaim == null)
            {
                return NotFound();
            }

            return View(aspNetUserClaim);
        }

        // GET: AspNetUserClaims/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: AspNetUserClaims/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,ClaimType,ClaimValue")] AspNetUserClaim aspNetUserClaim)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aspNetUserClaim);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", aspNetUserClaim.UserId);
            return View(aspNetUserClaim);
        }

        // GET: AspNetUserClaims/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AspNetUserClaims == null)
            {
                return NotFound();
            }

            var aspNetUserClaim = await _context.AspNetUserClaims.FindAsync(id);
            if (aspNetUserClaim == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", aspNetUserClaim.UserId);
            return View(aspNetUserClaim);
        }

        // POST: AspNetUserClaims/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,ClaimType,ClaimValue")] AspNetUserClaim aspNetUserClaim)
        {
            if (id != aspNetUserClaim.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aspNetUserClaim);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AspNetUserClaimExists(aspNetUserClaim.Id))
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
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", aspNetUserClaim.UserId);
            return View(aspNetUserClaim);
        }

        // GET: AspNetUserClaims/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AspNetUserClaims == null)
            {
                return NotFound();
            }

            var aspNetUserClaim = await _context.AspNetUserClaims
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aspNetUserClaim == null)
            {
                return NotFound();
            }

            return View(aspNetUserClaim);
        }

        // POST: AspNetUserClaims/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AspNetUserClaims == null)
            {
                return Problem("Entity set 'GameReviewDbContext.AspNetUserClaims'  is null.");
            }
            var aspNetUserClaim = await _context.AspNetUserClaims.FindAsync(id);
            if (aspNetUserClaim != null)
            {
                _context.AspNetUserClaims.Remove(aspNetUserClaim);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AspNetUserClaimExists(int id)
        {
          return (_context.AspNetUserClaims?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
