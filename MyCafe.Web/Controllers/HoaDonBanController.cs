using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyCafe.Web.Data;
using MyCafe.Web.Models;

namespace MyCafe.Web.Controllers
{
    public class HoaDonBanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HoaDonBanController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HoaDonBan
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.HoaDonBans
                .Include(h => h.KhachHang)
                .Include(h => h.NhanVien);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: HoaDonBan/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDonBan = await _context.HoaDonBans
                .Include(h => h.KhachHang)
                .Include(h => h.NhanVien)
                .Include(h => h.ChiTietHoaDonBans)
                .FirstOrDefaultAsync(m => m.MaHDB == id);
            
            if (hoaDonBan == null)
            {
                return NotFound();
            }

            return View(hoaDonBan);
        }

        // GET: HoaDonBan/Create
        public IActionResult Create()
        {
            ViewData["MaKH"] = new SelectList(_context.KhachHangs, "MaKH", "TenKH");
            ViewData["MaNV"] = new SelectList(_context.NhanViens, "MaNV", "TenNV");
            return View();
        }

        // POST: HoaDonBan/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaHDB,NgayBan,MaNV,MaKH,TongTien")] HoaDonBan hoaDonBan)
        {
            if (ModelState.IsValid)
            {
                hoaDonBan.TongTien = 0;
                _context.Add(hoaDonBan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Edit), new { id = hoaDonBan.MaHDB });
            }
            ViewData["MaKH"] = new SelectList(_context.KhachHangs, "MaKH", "TenKH", hoaDonBan.MaKH);
            ViewData["MaNV"] = new SelectList(_context.NhanViens, "MaNV", "TenNV", hoaDonBan.MaNV);
            return View(hoaDonBan);
        }

        // GET: HoaDonBan/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDonBan = await _context.HoaDonBans
                .Include(h => h.ChiTietHoaDonBans)
                .FirstOrDefaultAsync(m => m.MaHDB == id);

            if (hoaDonBan == null)
            {
                return NotFound();
            }

            ViewData["MaKH"] = new SelectList(_context.KhachHangs, "MaKH", "TenKH", hoaDonBan.MaKH);
            ViewData["MaNV"] = new SelectList(_context.NhanViens, "MaNV", "TenNV", hoaDonBan.MaNV);
            ViewData["MaSP"] = new SelectList(_context.Sanphams, "MaSP", "TenSP");

            return View(hoaDonBan);
        }

        // POST: HoaDonBan/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaHDB,NgayBan,MaNV,MaKH,TongTien")] HoaDonBan hoaDonBan)
        {
            if (id != hoaDonBan.MaHDB)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hoaDonBan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HoaDonBanExists(hoaDonBan.MaHDB))
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
            ViewData["MaKH"] = new SelectList(_context.KhachHangs, "MaKH", "TenKH", hoaDonBan.MaKH);
            ViewData["MaNV"] = new SelectList(_context.NhanViens, "MaNV", "TenNV", hoaDonBan.MaNV);
            return View(hoaDonBan);
        }

        // POST: HoaDonBan/AddDetail
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDetail(string MaHDB, string MaSP, int SoLuong, decimal KhuyenMai)
        {
            var hdb = await _context.HoaDonBans.FindAsync(MaHDB);
            var sp = await _context.Sanphams.FindAsync(MaSP);

            if (hdb == null || sp == null) return NotFound();

            if (sp.SoLuong < SoLuong)
            {
                TempData["Error"] = "Số lượng trong kho không đủ!";
                return RedirectToAction(nameof(Edit), new { id = MaHDB });
            }

            var detail = new ChiTietHoaDonBan
            {
                MaHDB = MaHDB,
                MaSP = MaSP,
                TenSP = sp.TenSP,
                SoLuong = SoLuong,
                KhuyenMai = KhuyenMai,
                ThanhTien = (sp.GiaBan ?? 0) * SoLuong - KhuyenMai
            };

            // Update stock
            sp.SoLuong -= SoLuong;

            _context.ChiTietHoaDonBans.Add(detail);
            
            // Update total amount
            hdb.TongTien = (hdb.TongTien ?? 0) + detail.ThanhTien;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Edit), new { id = MaHDB });
        }

        // POST: HoaDonBan/RemoveDetail
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveDetail(string MaHDB, string MaSP)
        {
            var detail = await _context.ChiTietHoaDonBans
                .FirstOrDefaultAsync(d => d.MaHDB == MaHDB && d.MaSP == MaSP);

            if (detail != null)
            {
                var hdb = await _context.HoaDonBans.FindAsync(MaHDB);
                var sp = await _context.Sanphams.FindAsync(MaSP);

                if (hdb != null && sp != null)
                {
                    // Restore stock
                    sp.SoLuong += detail.SoLuong;
                    
                    // Update total amount
                    hdb.TongTien -= detail.ThanhTien;

                    _context.ChiTietHoaDonBans.Remove(detail);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction(nameof(Edit), new { id = MaHDB });
        }

        // GET: HoaDonBan/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDonBan = await _context.HoaDonBans
                .Include(h => h.KhachHang)
                .Include(h => h.NhanVien)
                .FirstOrDefaultAsync(m => m.MaHDB == id);
            if (hoaDonBan == null)
            {
                return NotFound();
            }

            return View(hoaDonBan);
        }

        // POST: HoaDonBan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var hoaDonBan = await _context.HoaDonBans
                .Include(h => h.ChiTietHoaDonBans)
                .FirstOrDefaultAsync(m => m.MaHDB == id);

            if (hoaDonBan != null)
            {
                // Restore stock for all products in the invoice
                foreach (var detail in hoaDonBan.ChiTietHoaDonBans)
                {
                    var sp = await _context.Sanphams.FindAsync(detail.MaSP);
                    if (sp != null)
                    {
                        sp.SoLuong += detail.SoLuong;
                    }
                }

                _context.HoaDonBans.Remove(hoaDonBan);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool HoaDonBanExists(string id)
        {
            return _context.HoaDonBans.Any(e => e.MaHDB == id);
        }
    }
}
