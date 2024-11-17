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
    public class InvoiceController : Controller
    {
        private readonly string clientId = "2ad3bccd-e6f1-44e8-bb70-32b0ca279e28";
        private readonly string secretKey = "Gj5nS9wyYHRadaVffz5VKB4v4wlVWyPhcJvrTD4NHtNgA+wV+OZQzRauuUXaDzPpXTJ9gW11hYUfuDLJtkXKiphFxXwk/LAIV+aGL9TYQHwiYvb1afLxD4SsjV8eoMwMFI5bpnuvwcwRPVBgVSJiqJqhQuDGw7hfUeOEhD2rupxpRpq32gHqBOnPU/98GQQRv/BYPQsQtlIIA9VoAa4Z7FfGW9j13YgrUlxnGM6+QKWWLyu28L8cnLOVFeqjw1+2Z5m5y43KYyacfgp8cN7zUA==";
        private readonly string apiUrl = "https://backend.jofotara.gov.jo/core/invoices/";

        [HttpGet]
        public ActionResult GetSendInvoiceForm()
        {
            return View("SendInvoice");
        }

        [HttpPost]
        public async Task<ActionResult> SendInvoice()
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;


                // إنشاء نص XML للفاتورة


                string xmlInvoice = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<Invoice xmlns=\"urn:oasis:names:specification:ubl:schema:xsd:Invoice-2\" xmlns:cac=\"urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2\" xmlns:cbc=\"urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2\" xmlns:ext=\"urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2\">\r\n\t<cbc:ProfileID>reporting:1.0</cbc:ProfileID>\r\n\t<cbc:ID>EIN998835</cbc:ID>\r\n\t<cbc:UUID>7a4452eb-0acd-49983-b630-b2b79d0acert</cbc:UUID>\r\n\t<cbc:IssueDate>2022-09-27</cbc:IssueDate>\r\n\t<cbc:InvoiceTypeCode name=\"011\">388</cbc:InvoiceTypeCode>\r\n\t<cbc:Note>ملاحظات 2</cbc:Note>\r\n\t<cbc:DocumentCurrencyCode>JOD</cbc:DocumentCurrencyCode>\r\n\t<cbc:TaxCurrencyCode>JOD</cbc:TaxCurrencyCode>\r\n\t<cac:AdditionalDocumentReference>\r\n\t\t<cbc:ID>ICV</cbc:ID>\r\n\t\t<cbc:UUID>11</cbc:UUID>\r\n\t</cac:AdditionalDocumentReference>\r\n\t<cac:AccountingSupplierParty>\r\n\t\t<cac:Party>\r\n\t\t\t<cac:PostalAddress>\r\n\t\t\t\t<cac:Country>\r\n\t\t\t\t\t<cbc:IdentificationCode>JO</cbc:IdentificationCode>\r\n\t\t\t\t</cac:Country>\r\n\t\t\t</cac:PostalAddress>\r\n\t\t\t<cac:PartyTaxScheme>\r\n\t\t\t\t<cbc:CompanyID>6810330</cbc:CompanyID>\r\n\t\t\t\t<cac:TaxScheme>\r\n\t\t\t\t\t<cbc:ID>VAT</cbc:ID>\r\n\t\t\t\t</cac:TaxScheme>\r\n\t\t\t</cac:PartyTaxScheme>\r\n\t\t\t<cac:PartyLegalEntity>\r\n\t\t\t\t<cbc:RegistrationName>AAAAAA</cbc:RegistrationName>\r\n\t\t\t</cac:PartyLegalEntity>\r\n\t\t</cac:Party>\r\n\t</cac:AccountingSupplierParty>\r\n\t<cac:AccountingCustomerParty>\r\n\t\t<cac:Party>\r\n\t\t\t<cac:PartyIdentification>\r\n\t\t\t\t<cbc:ID schemeID=\"NIN\">امجد سليمان</cbc:ID>\r\n\t\t\t</cac:PartyIdentification>\r\n\t\t\t<cac:PostalAddress>\r\n\t\t\t\t<cbc:PostalZone>23432</cbc:PostalZone>\r\n\t\t\t\t<cac:Country>\r\n\t\t\t\t\t<cbc:IdentificationCode>JO</cbc:IdentificationCode>\r\n\t\t\t\t</cac:Country>\r\n\t\t\t</cac:PostalAddress>\r\n\t\t\t<cac:PartyTaxScheme>\r\n\t\t\t\t<cac:TaxScheme>\r\n\t\t\t\t\t<cbc:ID>VAT</cbc:ID>\r\n\t\t\t\t</cac:TaxScheme>\r\n\t\t\t</cac:PartyTaxScheme>\r\n\t\t\t<cac:PartyLegalEntity>\r\n\t\t\t\t<cbc:RegistrationName></cbc:RegistrationName>\r\n\t\t\t</cac:PartyLegalEntity>\r\n\t\t</cac:Party>\r\n\t\t<cac:AccountingContact>\r\n\t\t\t<cbc:Telephone>324323434</cbc:Telephone>\r\n\t\t</cac:AccountingContact>\r\n\t</cac:AccountingCustomerParty>\r\n\t<cac:SellerSupplierParty>\r\n\t\t<cac:Party>\r\n\t\t\t<cac:PartyIdentification>\r\n\t\t\t\t<cbc:ID>2088797</cbc:ID>\r\n\t\t\t</cac:PartyIdentification>\r\n\t\t</cac:Party>\r\n\t</cac:SellerSupplierParty>\r\n\t<cac:AllowanceCharge>\r\n\t\t<cbc:ChargeIndicator>false</cbc:ChargeIndicator>\r\n\t\t<cbc:AllowanceChargeReason>discount</cbc:AllowanceChargeReason>\r\n\t\t<cbc:Amount currencyID=\"JO\">2.00</cbc:Amount>\r\n\t</cac:AllowanceCharge>\r\n\t<cac:LegalMonetaryTotal>\r\n\t\t<cbc:TaxExclusiveAmount currencyID=\"JO\">132.00</cbc:TaxExclusiveAmount>\r\n\t\t<cbc:TaxInclusiveAmount currencyID=\"JO\">130.00</cbc:TaxInclusiveAmount>\r\n\t\t<cbc:AllowanceTotalAmount currencyID=\"JO\">2.00</cbc:AllowanceTotalAmount>\r\n\t\t<cbc:PayableAmount currencyID=\"JO\">130.00</cbc:PayableAmount>\r\n\t</cac:LegalMonetaryTotal>\r\n\t<cac:InvoiceLine>\r\n\t\t<cbc:ID>1</cbc:ID>\r\n\t\t<cbc:InvoicedQuantity unitCode=\"PCE\">44.00</cbc:InvoicedQuantity>\r\n\t\t<cbc:LineExtensionAmount currencyID=\"JO\">130.00</cbc:LineExtensionAmount>\r\n\t\t<cac:Item>\r\n\t\t\t<cbc:Name>زهره</cbc:Name>\r\n\t\t</cac:Item>\r\n\t\t<cac:Price>\r\n\t\t\t<cbc:PriceAmount currencyID=\"JO\">3.00</cbc:PriceAmount>\r\n\t\t\t<cac:AllowanceCharge>\r\n\t\t\t\t<cbc:ChargeIndicator>false</cbc:ChargeIndicator>\r\n\t\t\t\t<cbc:AllowanceChargeReason>DISCOUNT</cbc:AllowanceChargeReason>\r\n\t\t\t\t<cbc:Amount currencyID=\"JO\">2.00</cbc:Amount>\r\n\t\t\t</cac:AllowanceCharge>\r\n\t\t</cac:Price>\r\n\t</cac:InvoiceLine>\r\n</Invoice>";


                string base64EncodedInvoice = Convert.ToBase64String(Encoding.UTF8.GetBytes(xmlInvoice));

                var handler = new HttpClientHandler { UseProxy = false };

                using (var httpClient = new HttpClient(handler))
                {
                    httpClient.DefaultRequestHeaders.Add("Client-Id", clientId);
                    httpClient.DefaultRequestHeaders.Add("Secret-Key", secretKey);
                    httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var jsonBody = $@"{{ ""invoice"": ""{base64EncodedInvoice}"" }}";
                    var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var jsonResponse = JObject.Parse(responseContent);

                        string qrData = jsonResponse["EINV_QR"]?.ToString();

                        if (!string.IsNullOrEmpty(qrData))
                        {
                            // توليد QR Code
                            string qrCodeImage = GenerateQRCode(qrData);
                            return View("QRCodeView", model: qrCodeImage);
                        }
                        else
                        {
                            return Content("Invoice sent successfully, but QR Code not found.");
                        }
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        return Content($"Error: {response.StatusCode} - {errorContent}");
                    }
                }
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
        public ActionResult QRCodeView(string qrCode)
        {
            ViewBag.QRCode = qrCode;
            return View();
        }
    }
}