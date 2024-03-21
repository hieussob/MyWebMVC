using MyWebMVC.Data;

namespace MyWebMVC.ViewModels
{
    public class ChiTietDonHangVM
    {
        public int MaHd { get; set; }
        public string MaKh { get; set; } = null!;
        public DateTime NgayDat { get; set; }
        public string? HoTen { get; set; }
        public string DiaChi { get; set; } = null!;
        public string CachThanhToan { get; set; } = null!;
        public string CachVanChuyen { get; set; } = null!;
        public double PhiVanChuyen { get; set; }
        public int MaTrangThai { get; set; }
        public string? MaNv { get; set; }
        public string? GhiChu { get; set; }
        public int MaHh { get; set; }
        public string TenHh { get; set; }
        public string Hinh { get; set; }
        public double DonGia { get; set; }
        public int SoLuong { get; set; }
        public double GiamGia { get; set; }
        public double TongTien { get; set; }
    }
}
