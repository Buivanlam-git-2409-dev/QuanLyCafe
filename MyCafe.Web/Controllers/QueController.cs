using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCafe.Web.Data;
using MyCafe.Web.Models;

namespace MyCafe.Web.Controllers
{
    public class QueController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QueController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Que
        public async Task<IActionResult> Index()
        {
            return View(await _context.Ques.ToListAsync());
        }

        // GET: Que/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var que = await _context.Ques
                .FirstOrDefaultAsync(m => m.MaQue == id);
            if (que == null)
            {
                return NotFound();
            }

            return View(que);
        }

        // GET: Que/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Que/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaQue,TenQue")] Que que)
        {
            if (ModelState.IsValid)
            {
                _context.Add(que);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(que);
        }

        // GET: Que/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var que = await _context.Ques.FindAsync(id);
            if (que == null)
            {
                return NotFound();
            }
            return View(que);
        }

        // POST: Que/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaQue,TenQue")] Que que)
        {
            if (id != que.MaQue)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(que);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QueExists(que.MaQue))
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
            return View(que);
        }

        // GET: Que/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var que = await _context.Ques
                .FirstOrDefaultAsync(m => m.MaQue == id);
            if (que == null)
            {
                return NotFound();
            }

            return View(que);
        }

        // POST: Que/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var que = await _context.Ques.FindAsync(id);
            if (que != null)
            {
                _context.Ques.Remove(que);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QueExists(string id)
        {
            return _context.Ques.Any(e => e.MaQue == id);
        }
    }
}
