using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using QRCoder;
using System.IO;

namespace InvoiceProject2.Controllers
{
    public class Invoice2Controller : Controller
    {
        private readonly string apiUrl = "https://backend.jofotara.gov.jo/core/invoices/";

        [HttpGet]
        public ActionResult GetSendInvoice2Form()
        {
            return View("SendInvoice2");
        }

        [HttpPost]
        public async Task<ActionResult> SendInvoice2(string taxNumber, string incomeSequence, string userId, string secretKey)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

                // توليد البيانات اللازمة لإنشاء رمز QR
                string qrData = $"TaxNumber: {taxNumber}, IncomeSequence: {incomeSequence}";
                string qrCodeBase64 = GenerateQRCode(qrData);

                // تمرير رمز QR باستخدام TempData
                TempData["QRCode"] = qrCodeBase64;

                // إعادة التوجيه إلى العرض لعرض رمز QR
                return RedirectToAction("QRCodeView");
            }
            catch (HttpRequestException httpEx)
            {
                return Content($"HTTP Request Error: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                return Content($"An unexpected error occurred: {ex.Message}");
            }
        }

        private string GenerateQRCode(string qrData, int pixelSize = 2)
        {
            using (var qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrData, QRCodeGenerator.ECCLevel.Q);
                using (var qrCode = new QRCode(qrCodeData))
                {
                    using (var qrBitmap = qrCode.GetGraphic(pixelSize))
                    {
                        using (var stream = new MemoryStream())
                        {
                            qrBitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                            return Convert.ToBase64String(stream.ToArray());
                        }
                    }
                }
            }
        }

        [HttpGet]
        public ActionResult QRCodeView()
        {
            if (TempData["QRCode"] != null)
            {
                ViewBag.QRCode = TempData["QRCode"];
            }
            else
            {
                ViewBag.QRCode = null;
            }
            return View();
        }
    }
}

// الشرح بالعربي:
// 1. هذا الكود يمثل وحدة تحكم (Controller) في تطبيق ASP.NET MVC تُدعى Invoice2Controller.
// 2. يحتوي على ثلاث دوال رئيسية:
//    - GetSendInvoice2Form: تعرض صفحة إدخال الفاتورة.
//    - SendInvoice2: تُعالج إرسال بيانات الفاتورة وتولد رمز QR بناءً على القيم المدخلة.
//    - GenerateQRCode: تُولد رمز QR باستخدام مكتبة QRCoder وتُعيده كنص Base64.
//    - QRCodeView: تعرض رمز QR في الصفحة إذا كان متاحًا.
// 3. يتم استخدام دالة GenerateQRCode لإنشاء رمز QR من البيانات المدخلة مثل رقم الضريبة (TaxNumber) والتسلسل (IncomeSequence)، ثم يتم عرضها في صفحة QRCodeView باستخدام TempData.
// 4. إذا كان هناك خطأ أثناء العملية، يتم عرض رسالة خطأ مناسبة للمستخدم.
