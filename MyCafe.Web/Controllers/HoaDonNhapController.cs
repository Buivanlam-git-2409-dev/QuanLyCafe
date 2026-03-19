using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyCafe.Web.Data;
using MyCafe.Web.Models;

namespace MyCafe.Web.Controllers
{
    public class HoaDonNhapController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HoaDonNhapController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HoaDonNhap
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.HoaDonNhaps
                .Include(h => h.NCC)
                .Include(h => h.NhanVien);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: HoaDonNhap/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDonNhap = await _context.HoaDonNhaps
                .Include(h => h.NCC)
                .Include(h => h.NhanVien)
                .Include(h => h.ChiTietHoaDonNhaps)
                    .ThenInclude(d => d.Sanpham)
                .FirstOrDefaultAsync(m => m.MaHDN == id);
            
            if (hoaDonNhap == null)
            {
                return NotFound();
            }

            return View(hoaDonNhap);
        }

        // GET: HoaDonNhap/Create
        public IActionResult Create()
        {
            ViewData["MaNCC"] = new SelectList(_context.NCCs, "MaNCC", "TenNCC");
            ViewData["MaNV"] = new SelectList(_context.NhanViens, "MaNV", "TenNV");
            return View();
        }

        // POST: HoaDonNhap/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaHDN,NgayNhap,MaNV,MaNCC,TongTien")] HoaDonNhap hoaDonNhap)
        {
            if (ModelState.IsValid)
            {
                hoaDonNhap.TongTien = 0;
                _context.Add(hoaDonNhap);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Edit), new { id = hoaDonNhap.MaHDN });
            }
            ViewData["MaNCC"] = new SelectList(_context.NCCs, "MaNCC", "TenNCC", hoaDonNhap.MaNCC);
            ViewData["MaNV"] = new SelectList(_context.NhanViens, "MaNV", "TenNV", hoaDonNhap.MaNV);
            return View(hoaDonNhap);
        }

        // GET: HoaDonNhap/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDonNhap = await _context.HoaDonNhaps
                .Include(h => h.ChiTietHoaDonNhaps)
                    .ThenInclude(d => d.Sanpham)
                .FirstOrDefaultAsync(m => m.MaHDN == id);

            if (hoaDonNhap == null)
            {
                return NotFound();
            }

            ViewData["MaNCC"] = new SelectList(_context.NCCs, "MaNCC", "TenNCC", hoaDonNhap.MaNCC);
            ViewData["MaNV"] = new SelectList(_context.NhanViens, "MaNV", "TenNV", hoaDonNhap.MaNV);
            ViewData["MaSP"] = new SelectList(_context.Sanphams, "MaSP", "TenSP");

            return View(hoaDonNhap);
        }

        // POST: HoaDonNhap/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaHDN,NgayNhap,MaNV,MaNCC,TongTien")] HoaDonNhap hoaDonNhap)
        {
            if (id != hoaDonNhap.MaHDN)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hoaDonNhap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HoaDonNhapExists(hoaDonNhap.MaHDN))
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
            ViewData["MaNCC"] = new SelectList(_context.NCCs, "MaNCC", "TenNCC", hoaDonNhap.MaNCC);
            ViewData["MaNV"] = new SelectList(_context.NhanViens, "MaNV", "TenNV", hoaDonNhap.MaNV);
            return View(hoaDonNhap);
        }

        // POST: HoaDonNhap/AddDetail
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDetail(string MaHDN, string MaSP, int SoLuong, decimal DonGia, decimal KhuyenMai)
        {
            var hdn = await _context.HoaDonNhaps.FindAsync(MaHDN);
            var sp = await _context.Sanphams.FindAsync(MaSP);

            if (hdn == null || sp == null) return NotFound();

            var detail = new ChiTietHoaDonNhap
            {
                MaHDN = MaHDN,
                MaSP = MaSP,
                SoLuong = SoLuong,
                DonGia = DonGia,
                KhuyenMai = KhuyenMai,
                ThanhTien = (DonGia * SoLuong) - KhuyenMai
            };

            // Update stock (increment for import)
            sp.SoLuong = (sp.SoLuong ?? 0) + SoLuong;
            
            // Update product's cost price (optional, but good for management)
            sp.GiaNhap = DonGia;

            _context.ChiTietHoaDonNhaps.Add(detail);
            
            // Update total amount
            hdn.TongTien = (hdn.TongTien ?? 0) + detail.ThanhTien;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Edit), new { id = MaHDN });
        }

        // POST: HoaDonNhap/RemoveDetail
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveDetail(string MaHDN, string MaSP)
        {
            var detail = await _context.ChiTietHoaDonNhaps
                .FirstOrDefaultAsync(d => d.MaHDN == MaHDN && d.MaSP == MaSP);

            if (detail != null)
            {
                var hdn = await _context.HoaDonNhaps.FindAsync(MaHDN);
                var sp = await _context.Sanphams.FindAsync(MaSP);

                if (hdn != null && sp != null)
                {
                    // Restore stock (decrement if removing import detail)
                    sp.SoLuong -= detail.SoLuong;
                    
                    // Update total amount
                    hdn.TongTien -= detail.ThanhTien;

                    _context.ChiTietHoaDonNhaps.Remove(detail);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction(nameof(Edit), new { id = MaHDN });
        }

        // GET: HoaDonNhap/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDonNhap = await _context.HoaDonNhaps
                .Include(h => h.NCC)
                .Include(h => h.NhanVien)
                .FirstOrDefaultAsync(m => m.MaHDN == id);
            if (hoaDonNhap == null)
            {
                return NotFound();
            }

            return View(hoaDonNhap);
        }

        // POST: HoaDonNhap/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var hoaDonNhap = await _context.HoaDonNhaps
                .Include(h => h.ChiTietHoaDonNhaps)
                .FirstOrDefaultAsync(m => m.MaHDN == id);

            if (hoaDonNhap != null)
            {
                // Restore stock for all products in the invoice (decrement)
                foreach (var detail in hoaDonNhap.ChiTietHoaDonNhaps)
                {
                    var sp = await _context.Sanphams.FindAsync(detail.MaSP);
                    if (sp != null)
                    {
                        sp.SoLuong -= detail.SoLuong;
                    }
                }

                _context.HoaDonNhaps.Remove(hoaDonNhap);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool HoaDonNhapExists(string id)
        {
            return _context.HoaDonNhaps.Any(e => e.MaHDN == id);
        }
    }
}
