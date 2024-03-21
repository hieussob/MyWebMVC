using System.ComponentModel.DataAnnotations;

namespace MyWebMVC.ViewModels
{
    public class RegisterVM
    {
        [Display(Name ="Tên đăng nhập")]
        [Required(ErrorMessage ="*")]
        [MaxLength(20,ErrorMessage ="Tối đa 20 kí tự")]
        public string? MaKh { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "*")]
        [MinLength(6, ErrorMessage = "Tối thiểu 6 kí tự")]
        [DataType(DataType.Password)]
        public string? MatKhau { get; set; }

        [Required(ErrorMessage = "*")]
        [MaxLength(50, ErrorMessage = "Tối đa 50 kí tự")]
        [Display(Name = "Họ tên")]
        public string?  HoTen { get; set; }

        [Display(Name = "Giới tính")]
        public bool GioiTinh { get; set; } = true;

        [DataType(DataType.Date)]
        [Display(Name = "Ngày sinh")]
        public DateTime? NgaySinh { get; set; }

        [Required(ErrorMessage = "Vui lòng điền địa chỉ")]
        [MaxLength(60, ErrorMessage = "Tối đa 60 kí tự")]
        [Display(Name = "Địa chỉ")]
        public string? DiaChi { get; set; }

        [Required(ErrorMessage = "Vui lòng điền số điện thoại")]
        [MaxLength(24, ErrorMessage = "Tối đa 24 kí tự")]
        [RegularExpression(@"0\d{9}", ErrorMessage ="Chưa đúng định dạng")]
        [Display(Name = "Số điện thoại")]
        public string? DienThoai { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage ="Chưa đúng định dạng")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [DataType(DataType.ImageUrl)]
        public string? Hinh { get; set; }
        
    }
}
