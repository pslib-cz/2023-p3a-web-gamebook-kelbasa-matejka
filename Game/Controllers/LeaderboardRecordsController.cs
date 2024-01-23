using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Game.Helpers;
using Game.Models;

namespace Game.Controllers
{
    public class LeaderboardRecordsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeaderboardRecordsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LeaderboardRecords
        public async Task<IActionResult> Index()
        {
            return View(await _context.Records.ToListAsync());
        }

        // GET: LeaderboardRecords/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaderboardRecord = await _context.Records
                .FirstOrDefaultAsync(m => m.PlayerId == id);
            if (leaderboardRecord == null)
            {
                return NotFound();
            }

            return View(leaderboardRecord);
        }

        // GET: LeaderboardRecords/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LeaderboardRecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlayerId,Name,PlayTime,SavedAt")] LeaderboardRecord leaderboardRecord)
        {
            if (ModelState.IsValid)
            {
                _context.Add(leaderboardRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(leaderboardRecord);
        }

        // GET: LeaderboardRecords/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaderboardRecord = await _context.Records.FindAsync(id);
            if (leaderboardRecord == null)
            {
                return NotFound();
            }
            return View(leaderboardRecord);
        }

        // POST: LeaderboardRecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("PlayerId,Name,PlayTime,SavedAt")] LeaderboardRecord leaderboardRecord)
        {
            if (id != leaderboardRecord.PlayerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leaderboardRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaderboardRecordExists(leaderboardRecord.PlayerId))
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
            return View(leaderboardRecord);
        }

        // GET: LeaderboardRecords/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaderboardRecord = await _context.Records
                .FirstOrDefaultAsync(m => m.PlayerId == id);
            if (leaderboardRecord == null)
            {
                return NotFound();
            }

            return View(leaderboardRecord);
        }

        // POST: LeaderboardRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var leaderboardRecord = await _context.Records.FindAsync(id);
            if (leaderboardRecord != null)
            {
                _context.Records.Remove(leaderboardRecord);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaderboardRecordExists(string id)
        {
            return _context.Records.Any(e => e.PlayerId == id);
        }
    }
}
