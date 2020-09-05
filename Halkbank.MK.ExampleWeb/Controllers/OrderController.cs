using Halkbank.MK.ExampleWeb.Domain;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Halkbank.MK.ExampleWeb.Controllers
{
    public class OrderController : Controller
    {
        [HttpGet]
        public ActionResult Checkout(int orderId)
        {
            // halkbank provided credentials
            string storeKey = "halkbank provided key";
            string clientId = "halkbank provided client id";
            string portalUrl = "https://halkbank.provided.portal.com/";

            var settings = new HalkbankSettings(clientId, storeKey, portalUrl);
            var request = new HalkbankPaymentRequest(settings);

            request.Amount = 100;
            request.Oid = orderId.ToString();
            request.OkUrl = $"https://localhost:44357/Order/Success?orderId={request.Oid}";
            request.FailUrl = $"https://localhost:44357/Order/Fail?orderId={request.Oid}";
            request.CallbackUrl = $"https://localhost:44357/Order/Callback?orderId={request.Oid}";

            return View(request);
        }

        [HttpGet]
        public ActionResult Success(int orderId)
        {
            return View();
        }

        [HttpGet]
        public ActionResult Fail(int orderId)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Callback(int orderId)
        {
            // halkbank provided credentials
            string storeKey = "halkbank provided key";
            string clientId = "halkbank provided client id";
            string portalUrl = "https://halkbank.provided.portal.com/";

            var body = new StreamReader(Request.Body).ReadToEnd();

            var settings = new HalkbankSettings(clientId, storeKey, portalUrl);
            var response = new HalkbankPaymentResponse(settings, body);

            if (response.Success)
            {
                OrderService.MarkOrderAsPaid(orderId);
            }

            return Content(response.Response);
        }
    }
}
