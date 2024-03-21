using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using MyWebMVC.Data;
using MyWebMVC.Helpers;
using MyWebMVC.ViewModels;
using System.Data.Common;
using System.Security.Claims;

namespace MyWebMVC.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly TMHShopContext db;

        public KhachHangController(TMHShopContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult DangKy()
        {
            return View();
        }

        #region Đăng ký
        [HttpPost]
        public IActionResult DangKy(RegisterVM model, IFormFile Hinh)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Hinh != null)
                    {
                        model.Hinh = MyUtil.UpLoadHinh(Hinh, "KhachHang");
                    }
                    var kHang = new KhachHang
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
                        VaiTro = 0,
                    };
                    db.Add(kHang);
                    db.SaveChanges();
                    return RedirectToAction("Index", "HangHoa");
                }
                catch (Exception ex)
                {

                }
            }
            return View();
        }
        #endregion
        #region Đăng nhập
        [HttpGet]
        
        public IActionResult DangNhap(string? returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        
        public async Task<IActionResult> DangNhap(LoginVM model, string? returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            if (ModelState.IsValid)
            {
                var khachHang = db.KhachHangs.SingleOrDefault(p => p.MaKh == model.UserName);

                if (khachHang == null)
                {
                    ModelState.AddModelError("Lỗi", "Không tồn tại tài khoản này");
                }
                else
                {
                    if (!khachHang.HieuLuc)
                    {
                        ModelState.AddModelError("Lỗi", "Tài khoản đã bị khóa");
                    }
                    else
                    {
                        if (khachHang.MatKhau.ToLower() != model.Password.ToLower())
                        {
                            ModelState.AddModelError("Lỗi", "Sai mật khẩu");
                        }
                        else
                        {
                            var claims = new List<Claim>
                                {
                                    new Claim(ClaimTypes.Email, khachHang.Email),
                                    new Claim(ClaimTypes.Name, khachHang.HoTen),
                                    new Claim(MyConstant.CLAIM_CUSTOMERID, khachHang.MaKh),
                                    //claim role-động
                                    new Claim(ClaimTypes.Role, "Customer")
                                };
                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                            await HttpContext.SignInAsync(claimsPrincipal);

                            if (Url.IsLocalUrl(returnUrl))
                            {
                                ViewBag.UserName = khachHang.HoTen;
                                return Redirect(returnUrl);
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home");
                            }

                        }
                    }
                }
            }
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Profile()
        {
            var customerId = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MyConstant.CLAIM_CUSTOMERID).Value;
            var khachHang = db.KhachHangs.SingleOrDefault(p => p.MaKh == customerId);
            //if(customerId == "hieu")
            //{
            //    return RedirectToAction("LoginAdmin");
            //}

            return View(khachHang);
        }



        [Authorize]
        public async Task<IActionResult> DangXuat()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        [Authorize]
        [HttpPost]
        public IActionResult Profile(KhachHang model, IFormFile? Hinh)
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
                    khachHangChon.MaKh = model.MaKh;
                    khachHangChon.MatKhau = model.MatKhau ?? khachHangChon.MatKhau;
                    khachHangChon.HoTen = model.HoTen ?? khachHangChon.HoTen;
                    khachHangChon.GioiTinh = model.GioiTinh;
                    khachHangChon.DiaChi = model.DiaChi ?? khachHangChon.DiaChi;
                    khachHangChon.DienThoai = model.DienThoai ?? khachHangChon.DienThoai;
                    khachHangChon.Email = model.Email ?? khachHangChon.Email;
                    khachHangChon.Hinh = model.Hinh ?? khachHangChon.Hinh;
                    khachHangChon.HieuLuc = true;
                    khachHangChon.VaiTro = 0;

                    db.SaveChanges();
                    return View(khachHangChon);
                }
                catch (Exception ex)
                {

                }
            }
            return View();
        }

        #endregion



    }
}
