namespace MyWebMVC.Helpers
{
    public class MyUtil
    {
        public static string UpLoadHinh(IFormFile Hinh, string folder)
        {
            try
            {
                var pathFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Hinh", folder, Hinh.FileName);
                using (var myFile = new FileStream(pathFile, FileMode.CreateNew))
                {
                    Hinh.CopyTo(myFile);
                }
                return Hinh.FileName;
            }
            catch
            {
                return Hinh.FileName;
            }

        }
    }
}
