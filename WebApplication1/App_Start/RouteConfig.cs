using System.Web.Mvc;
using System.Web.Routing;

public class RouteConfig
{
    public static void RegisterRoutes(RouteCollection routes)
    {
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

        // مسار مخصص للكنترولر الثاني
        routes.MapRoute(
            name: "Invoice2",
            url: "Invoice2/{action}/{id}",
            defaults: new { controller = "Invoice2", action = "GetSendInvoice2Form", id = UrlParameter.Optional },
                namespaces: new[] { "InvoiceProject2.Controllers" }

        );

        // المسار الافتراضي
        routes.MapRoute(
            name: "Default",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Invoice", action = "GetSendInvoiceForm", id = UrlParameter.Optional },
                namespaces: new[] { "InvoiceProject2.Controllers" }

        );
    }
}
