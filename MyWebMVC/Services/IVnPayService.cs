using MyWebMVC.ViewModels;

namespace MyWebMVC.Services
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model);
        public VnPaymentResponseModel PaymentExecute(IQueryCollection collection);
    }
}
