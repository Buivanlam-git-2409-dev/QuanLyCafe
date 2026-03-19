using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCafe.Web.Data;
using MyCafe.Web.Models;

namespace MyCafe.Web.Controllers
{
    public class NCCController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NCCController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: NCC
        public async Task<IActionResult> Index()
        {
            return View(await _context.NCCs.ToListAsync());
        }

        // GET: NCC/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ncc = await _context.NCCs
                .FirstOrDefaultAsync(m => m.MaNCC == id);
            if (ncc == null)
            {
                return NotFound();
            }

            return View(ncc);
        }

        // GET: NCC/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NCC/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaNCC,TenNCC,DiaChi,SDT")] NCC ncc)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ncc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ncc);
        }

        // GET: NCC/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ncc = await _context.NCCs.FindAsync(id);
            if (ncc == null)
            {
                return NotFound();
            }
            return View(ncc);
        }

        // POST: NCC/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaNCC,TenNCC,DiaChi,SDT")] NCC ncc)
        {
            if (id != ncc.MaNCC)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ncc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NCCExists(ncc.MaNCC))
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
            return View(ncc);
        }

        // GET: NCC/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ncc = await _context.NCCs
                .FirstOrDefaultAsync(m => m.MaNCC == id);
            if (ncc == null)
            {
                return NotFound();
            }

            return View(ncc);
        }

        // POST: NCC/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var ncc = await _context.NCCs.FindAsync(id);
            if (ncc != null)
            {
                _context.NCCs.Remove(ncc);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NCCExists(string id)
        {
            return _context.NCCs.Any(e => e.MaNCC == id);
        }
    }
}
