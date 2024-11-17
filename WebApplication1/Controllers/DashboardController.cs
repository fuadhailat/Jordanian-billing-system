using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;

namespace InvoiceProject2.Controllers
{
    public class DashboardController : Controller
    {
        // الأكشن الذي يعرض صفحة الإدخال
        public ActionResult ViewDashboard()
        {
            return View("ViewDashboard");
        }

        // أكشن لمعالجة البيانات المدخلة
        [HttpPost]
        public async Task<ActionResult> SubmitData(string taxNumber, string incomeSequence, string userId, string secretKey)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Client-Id", userId);
                    httpClient.DefaultRequestHeaders.Add("Secret-Key", secretKey);
                    httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    // استخدام كود XML المقدم منك
                    string xmlInvoice = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<Invoice xmlns=\"urn:oasis:names:specification:ubl:schema:xsd:Invoice-2\" xmlns:cac=\"urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2\" xmlns:cbc=\"urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2\" xmlns:ext=\"urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2\">\r\n\t<cbc:ProfileID>reporting:1.0</cbc:ProfileID>\r\n\t<cbc:ID>EIN998833</cbc:ID>\r\n\t<cbc:UUID>7a4452eb-0acd-49983-b630-b2b79d0acert</cbc:UUID>\r\n\t<cbc:IssueDate>2022-09-27</cbc:IssueDate>\r\n\t<cbc:InvoiceTypeCode name=\"011\">388</cbc:InvoiceTypeCode>\r\n\t<cbc:Note>ملاحظات 2</cbc:Note>\r\n\t<cbc:DocumentCurrencyCode>JOD</cbc:DocumentCurrencyCode>\r\n\t<cbc:TaxCurrencyCode>JOD</cbc:TaxCurrencyCode>\r\n\t<cac:AdditionalDocumentReference>\r\n\t\t<cbc:ID>ICV</cbc:ID>\r\n\t\t<cbc:UUID>11</cbc:UUID>\r\n\t</cac:AdditionalDocumentReference>\r\n\t<cac:AccountingSupplierParty>\r\n\t\t<cac:Party>\r\n\t\t\t<cac:PostalAddress>\r\n\t\t\t\t<cac:Country>\r\n\t\t\t\t\t<cbc:IdentificationCode>JO</cbc:IdentificationCode>\r\n\t\t\t\t</cac:Country>\r\n\t\t\t</cac:PostalAddress>\r\n\t\t\t<cac:PartyTaxScheme>\r\n\t\t\t\t<cbc:CompanyID>6810330</cbc:CompanyID>\r\n\t\t\t\t<cac:TaxScheme>\r\n\t\t\t\t\t<cbc:ID>VAT</cbc:ID>\r\n\t\t\t\t</cac:TaxScheme>\r\n\t\t\t</cac:PartyTaxScheme>\r\n\t\t\t<cac:PartyLegalEntity>\r\n\t\t\t\t<cbc:RegistrationName>AAAAAA</cbc:RegistrationName>\r\n\t\t\t</cac:PartyLegalEntity>\r\n\t\t</cac:Party>\r\n\t</cac:AccountingSupplierParty>\r\n\t<cac:AccountingCustomerParty>\r\n\t\t<cac:Party>\r\n\t\t\t<cac:PartyIdentification>\r\n\t\t\t\t<cbc:ID schemeID=\"NIN\">امجد سليمان</cbc:ID>\r\n\t\t\t</cac:PartyIdentification>\r\n\t\t\t<cac:PostalAddress>\r\n\t\t\t\t<cbc:PostalZone>23432</cbc:PostalZone>\r\n\t\t\t\t<cac:Country>\r\n\t\t\t\t\t<cbc:IdentificationCode>JO</cbc:IdentificationCode>\r\n\t\t\t\t</cac:Country>\r\n\t\t\t</cac:PostalAddress>\r\n\t\t\t<cac:PartyTaxScheme>\r\n\t\t\t\t<cac:TaxScheme>\r\n\t\t\t\t\t<cbc:ID>VAT</cbc:ID>\r\n\t\t\t\t</cac:TaxScheme>\r\n\t\t\t</cac:PartyTaxScheme>\r\n\t\t\t<cac:PartyLegalEntity>\r\n\t\t\t\t<cbc:RegistrationName></cbc:RegistrationName>\r\n\t\t\t</cac:PartyLegalEntity>\r\n\t\t</cac:Party>\r\n\t\t<cac:AccountingContact>\r\n\t\t\t<cbc:Telephone>324323434</cbc:Telephone>\r\n\t\t</cac:AccountingContact>\r\n\t</cac:AccountingCustomerParty>\r\n\t<cac:SellerSupplierParty>\r\n\t\t<cac:Party>\r\n\t\t\t<cac:PartyIdentification>\r\n\t\t\t\t<cbc:ID>2088797</cbc:ID>\r\n\t\t\t</cac:PartyIdentification>\r\n\t\t</cac:Party>\r\n\t</cac:SellerSupplierParty>\r\n\t<cac:AllowanceCharge>\r\n\t	<cbc:ChargeIndicator>false</cbc:ChargeIndicator>\r\n\t	<cbc:AllowanceChargeReason>discount</cbc:AllowanceChargeReason>\r\n\t	<cbc:Amount currencyID=\"JO\">2.00</cbc:Amount>\r\n\t</cac:AllowanceCharge>\r\n\t<cac:LegalMonetaryTotal>\r\n\t	<cbc:TaxExclusiveAmount currencyID=\"JO\">132.00</cbc:TaxExclusiveAmount>\r\n\t	<cbc:TaxInclusiveAmount currencyID=\"JO\">130.00</cbc:TaxInclusiveAmount>\r\n\t	<cbc:AllowanceTotalAmount currencyID=\"JO\">2.00</cbc:AllowanceTotalAmount>\r\n\t	<cbc:PayableAmount currencyID=\"JO\">130.00</cbc:PayableAmount>\r\n\t</cac:LegalMonetaryTotal>\r\n\t<cac:InvoiceLine>\r\n\t	<cbc:ID>1</cbc:ID>\r\n\t	<cbc:InvoicedQuantity unitCode=\"PCE\">44.00</cbc:InvoicedQuantity>\r\n\t	<cbc:LineExtensionAmount currencyID=\"JO\">130.00</cbc:LineExtensionAmount>\r\n\t	<cac:Item>\r\n\t		<cbc:Name>زهره</cbc:Name>\r\n\t	</cac:Item>\r\n\t	<cac:Price>\r\n\t		<cbc:PriceAmount currencyID=\"JO\">3.00</cbc:PriceAmount>\r\n\t		<cac:AllowanceCharge>\r\n\t			<cbc:ChargeIndicator>false</cbc:ChargeIndicator>\r\n\t			<cbc:AllowanceChargeReason>DISCOUNT</cbc:AllowanceChargeReason>\r\n\t			<cbc:Amount currencyID=\"JO\">2.00</cbc:Amount>\r\n\t		</cac:AllowanceCharge>\r\n\t	</cac:Price>\r\n\t</cac:InvoiceLine>\r\n</Invoice>";

                    string base64EncodedInvoice = Convert.ToBase64String(Encoding.UTF8.GetBytes(xmlInvoice));

                    var jsonBody = $"{{ \"invoice\": \"{base64EncodedInvoice}\" }}";
                    var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync("https://backend.jofotara.gov.jo/core/invoices/", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var jsonResponse = JObject.Parse(responseContent);

                        string qrData = jsonResponse["EINV_QR"]?.ToString();

                        if (!string.IsNullOrEmpty(qrData))
                        {
                            return RedirectToAction("QRCodeView", "Invoice2", new { qrCode = qrData });
                        }
                        else
                        {
                            return Content("Invoice sent successfully, but QR Code not found.");
                        }
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        return Content($"Error: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                return Content("An unexpected error occurred.");
            }
        }
    }
}
