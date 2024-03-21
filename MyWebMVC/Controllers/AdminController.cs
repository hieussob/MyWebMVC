using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyWebMVC.Data;
using MyWebMVC.Helpers;
using MyWebMVC.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace MyWebMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly TMHShopContext db;

        public AdminController(TMHShopContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
   
        public IActionResult Login(LoginVM model)
        {
            return View();
        }

        #region Quản lý tài khoản
       
        public IActionResult DanhSachTaiKhoan()
        {
            var result = db.KhachHangs.ToList();
            return View(result);
        }
        [HttpGet]
      
        public IActionResult ThemMoi()
        {
            return View();
        }
        [HttpPost]
  
        public IActionResult ThemMoi(KhachHang model, IFormFile Hinh)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Hinh != null)
                    {
                        model.Hinh = MyUtil.UpLoadHinh(Hinh, "KhachHang");
                    }
                    var khachHang = new KhachHang
                    {
                        MaKh = model.MaKh,
                        MatKhau = model.MatKhau,
                        HoTen = model.HoTen,
                        GioiTinh = model.GioiTinh,
                        DiaChi = model.DiaChi,
                        DienThoai = model.DienThoai,
                        Email = model.Email,
                        Hinh = model.Hinh,
                        HieuLuc = true,
                        VaiTro = model.VaiTro,
                        RandomKey = model.RandomKey,
                        NgaySinh = model.NgaySinh
                    };
                    db.Add(khachHang);
                    db.SaveChanges();
                    return RedirectToAction("DanhSachTaiKhoan", "Admin");
                }
                catch (Exception ex)
                {
                }
            }
            return View();
        }

        public IActionResult SuaTaiKhoan(string MaKh)
        {
            var khachHangChon = db.KhachHangs.SingleOrDefault(p => p.MaKh == MaKh);
            return View(khachHangChon);
        }
        [HttpPost]

        public IActionResult SuaTaiKhoan(KhachHang model, IFormFile? Hinh)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if (Hinh != null)
                    {
                        model.Hinh = MyUtil.UpLoadHinh(Hinh, "KhachHang");
                    }
                    var khachHangChon = db.KhachHangs.SingleOrDefault(p => p.MaKh == model.MaKh);
                    khachHangChon.MatKhau = model.MatKhau ?? khachHangChon.MatKhau;
                    khachHangChon.HoTen = model.HoTen ?? khachHangChon.HoTen;
                    khachHangChon.GioiTinh = model.GioiTinh;
                    khachHangChon.DiaChi = model.DiaChi ?? khachHangChon.DiaChi;
                    khachHangChon.DienThoai = model.DienThoai ?? khachHangChon.DienThoai;
                    khachHangChon.Email = model.Email ?? khachHangChon.Email;
                    khachHangChon.Hinh = model.Hinh ?? khachHangChon.Hinh;
                    khachHangChon.HieuLuc = true;
                    khachHangChon.VaiTro = model.VaiTro;
                    khachHangChon.RandomKey = model.RandomKey;
                    khachHangChon.NgaySinh = model.NgaySinh;
                    db.SaveChanges();
                    return RedirectToAction("DanhSachTaiKhoan", "Admin");
                }
                catch (Exception ex)
                {
                }
            }
            return View();
        }


        public IActionResult XoaTaiKhoan(string MaKH)
        {
            var khachHangXoa = db.KhachHangs.SingleOrDefault(p => p.MaKh == MaKH);
            if (khachHangXoa != null)
            {
                db.Remove(khachHangXoa);
                db.SaveChanges();
            }
            return RedirectToAction("DanhSachTaiKhoan");
        }
        #endregion

        #region Quản lý danh mục
        [Route("danhsachdanhmuc")]
        public IActionResult DanhSachDanhMuc()
        {
            var danhMuc = db.Loais.ToList();
            return View(danhMuc);
        }
        [HttpGet]
        [Route("themdanhmuc")]
        public IActionResult ThemDanhMuc()
        {
            return View();
        }

        [HttpPost]
        [Route("themdanhmuc")]
        public IActionResult ThemDanhMuc(Loai model, IFormFile? Hinh)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Hinh != null)
                    {
                        model.Hinh = MyUtil.UpLoadHinh(Hinh, "Loai");
                    }
                    var loai = new Loai
                    {
                        TenLoai = model.TenLoai,
                        TenLoaiAlias = model.TenLoaiAlias,
                        Hinh = model.Hinh,
                        MoTa = model.MoTa
                    };
                    db.Add(loai);
                    db.SaveChanges();
                    return RedirectToAction("DanhSachDanhMuc");
                }
            }
            catch (Exception ex)
            {
            }
            return View();
        }

        [HttpGet]
        [Route("capnhatdanhmuc")]
        public IActionResult CapNhatDanhMuc(int MaLoai)
        {
            var loaiChon = db.Loais.SingleOrDefault(p => p.MaLoai == MaLoai);
            return View(loaiChon);
        }

        [HttpPost]
        [Route("capnhatdanhmuc")]
        public IActionResult CapNhatDanhMuc(Loai model, IFormFile? Hinh)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Hinh != null)
                    {
                        model.Hinh = MyUtil.UpLoadHinh(Hinh, "Loai");
                    }
                    var loaiChon = db.Loais.SingleOrDefault(p => p.MaLoai == model.MaLoai);
                    loaiChon.TenLoai = model.TenLoai;
                    loaiChon.TenLoaiAlias = model.TenLoaiAlias;
                    loaiChon.MoTa = model.MoTa;
                    loaiChon.Hinh = model.Hinh ?? loaiChon.Hinh;
                    db.SaveChanges();
                    return RedirectToAction("DanhSachDanhMuc");
                }
            }
            catch
            {

            }
            return View();
        }
        [Route("xoadanhmuc")]
        public IActionResult XoaDanhMuc(int MaLoai)
        {
            var loaiChon = db.Loais.SingleOrDefault(p => p.MaLoai == MaLoai);
            var sanPhamThuocLoai = db.HangHoas.Where(p => p.MaLoai == MaLoai);
            var chitietHD = db.ChiTietHds.Where(p => p.MaHhNavigation.MaLoai == MaLoai);
            db.RemoveRange(chitietHD);
            db.RemoveRange(sanPhamThuocLoai);
            db.Remove(loaiChon);
            db.SaveChanges();
            return RedirectToAction("DanhSachDanhMuc");
        }
        #endregion

        #region Quản lý sản phẩm
        [Route("danhsachsanpham")]
        public IActionResult DanhSachSanPham()
        {
            var danhSachSanPham = db.HangHoas.Include(p => p.MaLoaiNavigation).Include(p => p.MaNccNavigation).ToList();
            return View(danhSachSanPham);
        }

        [HttpGet]
        [Route("themmoisanpham")]
        public IActionResult ThemMoiSanPham()
        {
            var maNccList = new SelectList(db.NhaCungCaps, "MaNcc", "MaNcc"); // Giả sử MaNccs là tên DbSet của bạn
            var maLoaiList = new SelectList(db.Loais, "MaLoai", "MaLoai"); // Giả sử MaNccs là tên DbSet của bạn

            // Đặt dữ liệu vào ViewBag
            ViewBag.MaNcc = maNccList;
            ViewBag.MaLoai = maLoaiList;
            return View();
        }

        [HttpPost]
        [Route("themmoisanpham")]
        public IActionResult ThemMoiSanPham(HangHoa model, IFormFile? Hinh)
        {
            var maNccList = new SelectList(db.NhaCungCaps, "MaNcc", "MaNcc"); // Giả sử MaNccs là tên DbSet của bạn
            var maLoaiList = new SelectList(db.Loais, "MaLoai", "MaLoai"); // Giả sử MaNccs là tên DbSet của bạn

            // Đặt dữ liệu vào ViewBag
            ViewBag.MaNcc = maNccList;
            ViewBag.MaLoai = maLoaiList;
            try
            {
                if (Hinh != null)
                {
                    model.Hinh = MyUtil.UpLoadHinh(Hinh, "HangHoa");
                }
                var hangHoa = new HangHoa
                {
                    MaHh = model.MaHh,
                    TenHh = model.TenHh,
                    TenAlias = model.TenAlias,
                    MaLoai = model.MaLoai,
                    MoTaDonVi = model.MoTaDonVi,
                    MoTa = model.MoTa,
                    DonGia = model.DonGia,
                    Hinh = model.Hinh,
                    NgaySx = model.NgaySx,
                    GiamGia = model.GiamGia,
                    MaNcc = model.MaNcc,
                    SoLanXem = model.SoLanXem
                };
                db.Add(hangHoa);
                db.SaveChanges();
                return RedirectToAction("DanhSachSanPham");
            }
            catch (Exception ex) { }
            return View();
        }

        [HttpGet]
        [Route("capnhatsanpham")]
        public IActionResult CapNhatSanPham(int id)
        {
            var maNccList = new SelectList(db.NhaCungCaps, "MaNcc", "MaNcc"); // Giả sử MaNccs là tên DbSet của bạn
            var maLoaiList = new SelectList(db.Loais, "MaLoai", "MaLoai"); // Giả sử MaNccs là tên DbSet của bạn

            // Đặt dữ liệu vào ViewBag
            ViewBag.MaNcc = maNccList;
            ViewBag.MaLoai = maLoaiList;
            var sanPhamChon = db.HangHoas.SingleOrDefault(p => p.MaHh == id);

            return View(sanPhamChon);
        }
        [HttpPost]
        [Route("capnhatsanpham")]
        public IActionResult CapNhatSanPham(HangHoa model, IFormFile? Hinh)
        {
            var maNccList = new SelectList(db.NhaCungCaps, "MaNcc", "MaNcc"); // Giả sử MaNccs là tên DbSet của bạn
            var maLoaiList = new SelectList(db.Loais, "MaLoai", "MaLoai"); // Giả sử MaNccs là tên DbSet của bạn

            // Đặt dữ liệu vào ViewBag
            ViewBag.MaNcc = maNccList;
            ViewBag.MaLoai = maLoaiList;
            try
            {
                if (Hinh != null)
                {
                    model.Hinh = MyUtil.UpLoadHinh(Hinh, "HangHoa");
                }
                var sanPhamChon = db.HangHoas.SingleOrDefault(p => p.MaHh == model.MaHh);
                sanPhamChon.TenHh = model.TenHh;
                sanPhamChon.TenAlias = model.TenAlias;
                sanPhamChon.MaLoai = model.MaLoai;
                sanPhamChon.MoTaDonVi = model.MoTaDonVi;
                sanPhamChon.DonGia = model.DonGia;
                sanPhamChon.Hinh = model.Hinh ?? sanPhamChon.Hinh;
                sanPhamChon.NgaySx = model.NgaySx;
                sanPhamChon.GiamGia = model.GiamGia;
                sanPhamChon.SoLanXem = model.SoLanXem;
                sanPhamChon.MoTa = model.MoTa;
                sanPhamChon.MaNcc = model.MaNcc;
                db.SaveChanges();
                return RedirectToAction("DanhSachSanPham");

            }
            catch (Exception ex) { }
            return View();
        }

        [Route("xoasanpham")]
        public IActionResult XoaSanPham(int id)
        {
            var sanPhamXoa = db.HangHoas.SingleOrDefault(p => p.MaHh == id);
            db.Remove(sanPhamXoa);
            db.SaveChanges();
            return RedirectToAction("DanhSachSanPham");
        }
        #endregion

        #region Quản lý nhà cung cấp
        [Route("danhsachnhacungcap")]
        public IActionResult DanhSachNhaCungCap()
        {
            var dsNhaCC = db.NhaCungCaps.ToList();
            return View(dsNhaCC);
        }
        [Route("themncc")]
        public IActionResult ThemNCC()
        {
            return View();
        }

        [HttpPost]
        [Route("themncc")]
        public IActionResult ThemNCC(NhaCungCap model, IFormFile? Logo)
        {
            try
            {

                if (Logo != null)
                {
                    model.Logo = MyUtil.UpLoadHinh(Logo, "NhaCC");
                }
                var nhaCC = new NhaCungCap()
                {
                    MaNcc = model.MaNcc,
                    TenCongTy = model.TenCongTy,
                    DiaChi = model.DiaChi,
                    DienThoai = model.DienThoai,
                    Email = model.Email,
                    MoTa = model.MoTa,
                    NguoiLienLac = model.NguoiLienLac,
                    Logo = model.Logo

                };
                db.Add(nhaCC);
                db.SaveChanges();
                return RedirectToAction("DanhSachNhaCungCap");

            }
            catch { }
            return View();
        }

        [HttpGet]
        [Route("capnhatncc")]
        public IActionResult CapNhatNCC(string id)
        {
            var nhaCCChon = db.NhaCungCaps.SingleOrDefault(p => p.MaNcc == id);
            return View(nhaCCChon);
        }

        [HttpPost]
        [Route("capnhatncc")]
        public IActionResult CapNhatNCC(NhaCungCap model, IFormFile? Logo)
        {
            try
            {

                if (Logo != null)
                {
                    model.Logo = MyUtil.UpLoadHinh(Logo, "NhaCC");
                }
                var nhaCCChon = db.NhaCungCaps.SingleOrDefault(p => p.MaNcc == model.MaNcc);
                nhaCCChon.NguoiLienLac = model.NguoiLienLac;
                nhaCCChon.TenCongTy = model.TenCongTy;
                nhaCCChon.DiaChi = model.DiaChi;
                nhaCCChon.DienThoai = model.DienThoai;
                nhaCCChon.Email = model.Email;
                nhaCCChon.Logo = model.Logo ?? nhaCCChon.Logo;
                nhaCCChon.MoTa = model.MoTa;
                db.SaveChanges();
                return RedirectToAction("DanhSachNhaCungCap");

            }
            catch { }
            return View();
        }
        [Route("xoancc")]
        public IActionResult XoaNCC(string id)
        {
            var nhaCCXoa = db.NhaCungCaps.SingleOrDefault(p => p.MaNcc == id);
            db.Remove(nhaCCXoa);
            db.SaveChanges();
            return RedirectToAction("DanhSachNhaCungCap");
        }
        #endregion

        #region Quản lý đơn hàng và hóa đơn
        [Route("danhsachdonhang")]
        public IActionResult DanhSachDonHang()
        {
            var query = db.HoaDons.Include(p => p.MaKhNavigation).Include(p => p.MaNvNavigation).Include(p => p.MaTrangThaiNavigation);

            var dsDonHang = query.Select(ds => new HoaDon
            {
                MaHd = ds.MaHd,
                MaKh = ds.MaKh,
                NgayDat = ds.NgayDat,
                NgayCan = ds.NgayCan,
                NgayGiao = ds.NgayGiao,
                HoTen = ds.MaKhNavigation.HoTen ?? null,
                DiaChi = ds.DiaChi,
                CachThanhToan = ds.CachThanhToan,
                CachVanChuyen = ds.CachVanChuyen,
                PhiVanChuyen = ds.PhiVanChuyen,
                MaTrangThai = ds.MaTrangThai,
                MaNv = ds.MaNv ?? "không có",
                GhiChu = ds.GhiChu

            }).ToList();
            return View(dsDonHang);
        }
        [HttpGet]
        [Route("themdonhang")]
        public IActionResult ThemDonHang()
        {
            var maKh = new SelectList(db.KhachHangs, "MaKh", "MaKh");
            ViewBag.MaKh = maKh;
            var maNv = new SelectList(db.NhanViens, "MaNv", "MaNv");
            ViewBag.MaNV = maNv;
            var maTT = new SelectList(db.TrangThais, "MaTrangThai", "MaTrangThai");
            ViewBag.MaTrangThai = maTT;
            return View();
        }

        [HttpPost]
        [Route("themdonhang")]
        public IActionResult ThemDonHang(HoaDon model)
        {
            var maKh = new SelectList(db.KhachHangs, "MaKh", "MaKh");
            ViewBag.MaKh = maKh;
            var maNv = new SelectList(db.NhanViens, "MaNv", "MaNv");
            ViewBag.MaNV = maNv;
            var maTT = new SelectList(db.TrangThais, "MaTrangThai", "MaTrangThai");
            ViewBag.MaTrangThai = maTT;

            try
            {
                var donHang = new HoaDon
                {
                    MaKh = model.MaKh,
                    MaNv = model.MaNv,
                    NgayCan = model.NgayCan,
                    NgayDat = model.NgayDat,
                    NgayGiao = model.NgayGiao,
                    HoTen = model.HoTen,
                    DiaChi = model.DiaChi,
                    CachThanhToan = model.CachThanhToan,
                    CachVanChuyen = model.CachVanChuyen,
                    PhiVanChuyen = model.PhiVanChuyen,
                    MaTrangThai = model.MaTrangThai,
                    GhiChu = model.GhiChu,
                };
                db.Add(donHang);
                db.SaveChanges();
                return RedirectToAction("DanhSachDonHang");
            }
            catch { }

            return View();
        }
        #endregion

        public IActionResult Test()
        {
            return View();
        }
    }
}
