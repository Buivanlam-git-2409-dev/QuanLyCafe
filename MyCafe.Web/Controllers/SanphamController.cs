using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCafe.Web.Data;
using MyCafe.Web.Models;

namespace MyCafe.Web.Controllers
{
    public class SanphamController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SanphamController> _logger;

        public SanphamController(ApplicationDbContext context, ILogger<SanphamController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Sanpham
        public async Task<IActionResult> Index(string searchString)
        {
            var sanphams = _context.Sanphams
                .Include(s => s.Loai)
                .Include(s => s.CongDung)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                sanphams = sanphams.Where(s => 
                    s.MaSP.Contains(searchString) || 
                    s.TenSP.Contains(searchString));
            }

            return View(await sanphams.ToListAsync());
        }

        // GET: Sanpham/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanpham = await _context.Sanphams
                .Include(s => s.Loai)
                .Include(s => s.CongDung)
                .FirstOrDefaultAsync(m => m.MaSP == id);

            if (sanpham == null)
            {
                return NotFound();
            }

            return View(sanpham);
        }

        // GET: Sanpham/Create
        public IActionResult Create()
        {
            ViewData["MaLoai"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                _context.Loais, "MaLoai", "TenLoai");
            ViewData["MaCongDung"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                _context.CongDungs, "MaCongDung", "TenCongDung");
            return View();
        }

        // POST: Sanpham/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaSP,TenSP,MaLoai,GiaNhap,GiaBan,SoLuong,MaCongDung")] Sanpham sanpham, IFormFile hinhanh)
        {
            try
            {
                // Check if product code already exists
                if (_context.Sanphams.Any(s => s.MaSP == sanpham.MaSP))
                {
                    ModelState.AddModelError("MaSP", "Mã sản phẩm này đã tồn tại");
                }

                if (ModelState.IsValid)
                {
                    // Handle image upload
                    if (hinhanh != null && hinhanh.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await hinhanh.CopyToAsync(memoryStream);
                            sanpham.HinhAnh = memoryStream.ToArray();
                        }
                    }
                    else
                    {
                        sanpham.HinhAnh = Array.Empty<byte>();
                    }

                    sanpham.CreatedAt = DateTime.Now;
                    sanpham.UpdatedAt = DateTime.Now;

                    _context.Add(sanpham);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Sản phẩm {sanpham.MaSP} đã được thêm mới");
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Lỗi database: {ex.Message}");
                ModelState.AddModelError("", "Lỗi khi lưu dữ liệu vào cơ sở dữ liệu");
            }

            ViewData["MaLoai"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                _context.Loais, "MaLoai", "TenLoai", sanpham.MaLoai);
            ViewData["MaCongDung"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                _context.CongDungs, "MaCongDung", "TenCongDung", sanpham.MaCongDung);
            return View(sanpham);
        }

        // GET: Sanpham/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanpham = await _context.Sanphams.FindAsync(id);
            if (sanpham == null)
            {
                return NotFound();
            }

            ViewData["MaLoai"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                _context.Loais, "MaLoai", "TenLoai", sanpham.MaLoai);
            ViewData["MaCongDung"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                _context.CongDungs, "MaCongDung", "TenCongDung", sanpham.MaCongDung);
            return View(sanpham);
        }

        // POST: Sanpham/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaSP,TenSP,MaLoai,GiaNhap,GiaBan,SoLuong,MaCongDung")] Sanpham sanpham, IFormFile hinhanh)
        {
            if (id != sanpham.MaSP)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingSanpham = await _context.Sanphams.FindAsync(id);
                    if (existingSanpham == null)
                    {
                        return NotFound();
                    }

                    // Update properties
                    existingSanpham.TenSP = sanpham.TenSP;
                    existingSanpham.MaLoai = sanpham.MaLoai;
                    existingSanpham.GiaNhap = sanpham.GiaNhap;
                    existingSanpham.GiaBan = sanpham.GiaBan;
                    existingSanpham.SoLuong = sanpham.SoLuong;
                    existingSanpham.MaCongDung = sanpham.MaCongDung;
                    existingSanpham.UpdatedAt = DateTime.Now;

                    // Handle image upload
                    if (hinhanh != null && hinhanh.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await hinhanh.CopyToAsync(memoryStream);
                            existingSanpham.HinhAnh = memoryStream.ToArray();
                        }
                    }

                    _context.Update(existingSanpham);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Sản phẩm {sanpham.MaSP} đã được cập nhật");
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SanphamExists(sanpham.MaSP))
                    {
                        return NotFound();
                    }
                    throw;
                }
            }

            ViewData["MaLoai"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                _context.Loais, "MaLoai", "TenLoai", sanpham.MaLoai);
            ViewData["MaCongDung"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                _context.CongDungs, "MaCongDung", "TenCongDung", sanpham.MaCongDung);
            return View(sanpham);
        }

        // GET: Sanpham/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanpham = await _context.Sanphams
                .Include(s => s.Loai)
                .Include(s => s.CongDung)
                .FirstOrDefaultAsync(m => m.MaSP == id);

            if (sanpham == null)
            {
                return NotFound();
            }

            return View(sanpham);
        }

        // POST: Sanpham/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var sanpham = await _context.Sanphams.FindAsync(id);
            if (sanpham != null)
            {
                _context.Sanphams.Remove(sanpham);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Sản phẩm {sanpham.MaSP} đã bị xóa");
            }

            return RedirectToAction(nameof(Index));
        }

        private bool SanphamExists(string id)
        {
            return _context.Sanphams.Any(e => e.MaSP == id);
        }
    }
}
