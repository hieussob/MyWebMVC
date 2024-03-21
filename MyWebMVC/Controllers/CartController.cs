using Microsoft.AspNetCore.Mvc;
using MyWebMVC.Data;
using MyWebMVC.ViewModels;
using MyWebMVC.Helpers;
using Microsoft.AspNetCore.Authorization;
using MyWebMVC.Services;

namespace MyWebMVC.Controllers
{
    public class CartController : Controller
    {
        private readonly TMHShopContext db;
        private readonly IVnPayService _vnPayservice;

        public CartController(TMHShopContext context, IVnPayService vnPayservice) {
            db = context;
            _vnPayservice = vnPayservice;
        }

        public List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>(MyConstant.CART_KEY) ?? new List<CartItem>();
        public  List<ChiTietDonHangVM> result = new List<ChiTietDonHangVM>();
    
        public IActionResult Index()
        {
            return View(Cart);
        }
        public IActionResult AddToCart(int id, int quantity = 1)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p=>p.MaHh == id);
            if (item == null)
            {
                var hangHoa= db.HangHoas.SingleOrDefault(p=>p.MaHh==id);
                if(hangHoa == null)
                {
                
                    return RedirectToAction("Home/Index");
                }
                item = new CartItem
                {
                    MaHh = hangHoa.MaHh,
                    Hinh = hangHoa.Hinh,
                    TenHh = hangHoa.TenHh,
                    DonGia = hangHoa.DonGia??0,
                    SoLuong = quantity,
                };
                gioHang.Add(item);
            }
            else
            {
                item.SoLuong += quantity;
            }
            HttpContext.Session.Set(MyConstant.CART_KEY, gioHang);

            return RedirectToAction("Index") ;
        }

        public IActionResult RemoveCart(int id)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaHh == id);
            if (item != null)
            {
                gioHang.Remove(item);
                HttpContext.Session.Set(MyConstant.CART_KEY,gioHang);
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public IActionResult ThanhToanDonHang() {
            
            if(Cart.Count == 0)
            {
                return Redirect("/");
            }
            return View(Cart);
        }
        
        [Authorize]
        [HttpPost]
        public IActionResult ThanhToanDonHang(CheckoutVM model,string payment) {

            if (ModelState.IsValid)
            {
                
                
                var customerId = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MyConstant.CLAIM_CUSTOMERID).Value;
                var khachHang = new KhachHang();
                if (model.GiongKhachHang)
                {
                    khachHang= db.KhachHangs.SingleOrDefault(p=>p.MaKh==customerId);
                }

                var hoaDon = new HoaDon
                {
                    MaKh = customerId,
                    HoTen = model.HoTen ?? khachHang.HoTen ,
                    DiaChi =model.DiaChi ?? khachHang.DiaChi,
                    GhiChu = model.GhiChu ?? "",
                    NgayDat = DateTime.Now,
                    CachThanhToan = payment,
                    CachVanChuyen = "EXPRESS",
                    MaTrangThai = 0
                };
                db.Database.BeginTransaction();
                try
                {
                    db.Database.CommitTransaction();
                    db.Add(hoaDon);
                    db.SaveChanges();
                    var cthd = new List<ChiTietHd>();
                    foreach(var item in Cart)
                    {
                        cthd.Add(new ChiTietHd
                        {
                            MaHd = hoaDon.MaHd,
                            SoLuong= item.SoLuong,
                            DonGia= item.DonGia,
                            MaHh=item.MaHh,
                            GiamGia=0
                        }) ;
                    }
                    db.AddRange(cthd);
                    db.SaveChanges();
                    
                    if (payment == "Thanh Toán VnPay")
                    {
                        var vnPayModel = new VnPaymentRequestModel
                        {
                            Amount = Cart.Sum(p => p.ThanhTien),
                            CreatedDate = DateTime.Now,
                            Description = $"{model.HoTen}{model.DiaChi}",
                            FullName = model.HoTen,
                            OrderId = new Random().Next(1000, 10000)
                        };
                        
                        HttpContext.Session.Set<List<CartItem>>(MyConstant.CART_KEY, new List<CartItem>());
                        return Redirect(_vnPayservice.CreatePaymentUrl(HttpContext, vnPayModel));
                    }
                    HttpContext.Session.Set<List<CartItem>>(MyConstant.CART_KEY, new List<CartItem>());
                    return RedirectToAction("Success");
                }
                catch
                {
                    return View("ThanhToanDonHang");
                }
                
            }
            return View(Cart);
        }

        [Authorize]
        public IActionResult Success()
        {
            var customerId = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MyConstant.CLAIM_CUSTOMERID).Value;
            var hoaDon = from hd in db.HoaDons
                         join cthd in db.ChiTietHds on hd.MaHd equals cthd.MaHd
                         join hh in db.HangHoas on cthd.MaHh equals hh.MaHh
                         where hd.MaKh.Equals(customerId)
                         select new ChiTietDonHangVM
                         {
                             MaHd = hd.MaHd,
                             MaKh = customerId,
                             HoTen = hd.HoTen,
                             DiaChi = hd.DiaChi,
                             GhiChu = hd.GhiChu ?? "",
                             NgayDat = hd.NgayDat,
                             CachThanhToan = hd.CachThanhToan,
                             CachVanChuyen = hd.CachVanChuyen,
                             MaTrangThai = hd.MaTrangThai,
                             MaHh = cthd.MaHh,
                             TenHh = hh.TenHh ?? "",
                             Hinh = hh.Hinh ?? "",
                             DonGia = cthd.DonGia,
                             SoLuong = cthd.SoLuong,
                             GiamGia = 0,
                             TongTien = cthd.DonGia * cthd.SoLuong
                         };
           
            return View(hoaDon);
        }
        public IActionResult RemoveCartDetail(int MaHd)
        {
            var item = db.HoaDons.SingleOrDefault(p => p.MaHd == MaHd);
            if (item.MaTrangThai == 0)
            {
                db.Remove(item);
                db.SaveChanges();
            }
            return RedirectToAction("Success");
        }
        [Authorize]
        public IActionResult PaymentFail()
        {
            return View();
        }

        [Authorize]
        public IActionResult PaymentCallBack()
        {
            var response = _vnPayservice.PaymentExecute(Request.Query);
            
            if(response == null || response.VnPayResponseCode != "00")
            {
                TempData["Message"] = $"Lỗi thanh toán VNPay : {response.VnPayResponseCode}";
                RedirectToAction("PaymentFail");
            }

            ////Lưu đơn hàng vào database

            //TempData["Message"] = $"Thanh toán VNPay thành công";
            //return RedirectToAction("Success");
            try
            {
                // Lấy mã đơn hàng từ response hoặc từ Session nếu bạn đã lưu nó trong quá trình tạo payment
                var orderId = response.OrderId; // Lưu ý: Đây chỉ là ví dụ, bạn cần điều chỉnh phù hợp với cách bạn lưu OrderId

                // Cập nhật trạng thái của đơn hàng có mã orderId thành 1 (hoàn thành) trong cơ sở dữ liệu
                var hoaDon = db.HoaDons.SingleOrDefault(hd => hd.MaHd.ToString() == orderId);
                if (hoaDon != null)
                {
                    hoaDon.MaTrangThai = 1; // 1 là trạng thái hoàn thành, bạn có thể sử dụng enum để quản lý trạng thái
                    db.SaveChanges();
                    TempData["Message"] = $"Thanh toán VNPay thành công";
                    return RedirectToAction("Success");
                }
                else
                {
                    TempData["Message"] = $"Không tìm thấy đơn hàng có mã {orderId}";
                    return RedirectToAction("PaymentFail");
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Lỗi khi cập nhật trạng thái đơn hàng: {ex.Message}";
                return RedirectToAction("PaymentFail");
            }

        }




    }
}
