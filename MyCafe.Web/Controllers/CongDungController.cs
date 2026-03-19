using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCafe.Web.Data;
using MyCafe.Web.Models;

namespace MyCafe.Web.Controllers
{
    public class CongDungController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CongDungController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CongDung
        public async Task<IActionResult> Index()
        {
            return View(await _context.CongDungs.ToListAsync());
        }

        // GET: CongDung/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var congDung = await _context.CongDungs
                .FirstOrDefaultAsync(m => m.MaCongDung == id);
            if (congDung == null)
            {
                return NotFound();
            }

            return View(congDung);
        }

        // GET: CongDung/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CongDung/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaCongDung,TenCongDung")] CongDung congDung)
        {
            if (ModelState.IsValid)
            {
                _context.Add(congDung);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(congDung);
        }

        // GET: CongDung/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var congDung = await _context.CongDungs.FindAsync(id);
            if (congDung == null)
            {
                return NotFound();
            }
            return View(congDung);
        }

        // POST: CongDung/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaCongDung,TenCongDung")] CongDung congDung)
        {
            if (id != congDung.MaCongDung)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(congDung);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CongDungExists(congDung.MaCongDung))
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
            return View(congDung);
        }

        // GET: CongDung/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var congDung = await _context.CongDungs
                .FirstOrDefaultAsync(m => m.MaCongDung == id);
            if (congDung == null)
            {
                return NotFound();
            }

            return View(congDung);
        }

        // POST: CongDung/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var congDung = await _context.CongDungs.FindAsync(id);
            if (congDung != null)
            {
                _context.CongDungs.Remove(congDung);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CongDungExists(string id)
        {
            return _context.CongDungs.Any(e => e.MaCongDung == id);
        }
    }
}
