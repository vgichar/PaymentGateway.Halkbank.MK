# Disclaimer

This is not an official library and is not produced by Halkbank or and affiliated developers with Halkbank.  
This software is provided "as is" without any guarantee that it will work as intended.

# Requirements

In order to use this library you must have:
- Web application using .NET Framework 4.6.1+ or .NET Core 1.0+
- Domain certificate
- Merchant contract with [Halkbank.mk](https://www.halkbank.mk/)


# Getting started

Checkout the [sample web project](https://github.com/vgichar/PaymentGateway.Halkbank.MK/tree/master/Halkbank.MK.ExampleWeb) as a full integration example for online payments with Halkbank.

---

When the customers are ready to pay we should redirect them to checkout page.  
On checkout prepare the [`HalkbankPaymentRequest`](https://github.com/vgichar/PaymentGateway.Halkbank.MK/blob/master/Halkbank.MK/HalkbankPaymentRequest.cs) model using the [`HalkbankSettings`](https://github.com/vgichar/PaymentGateway.Halkbank.MK/blob/master/Halkbank.MK/HalkbankSettings.cs) model with data provided by the bank.  

Make sure to get the order amount and id from the customer checkout.

```
[HttpGet]
public ActionResult Checkout(int orderId)
{
    // halkbank provided credentials
    string storeKey = "halkbank provided key";
    string clientId = "halkbank provided client id";
    string portalUrl = "https://halkbank.provided.portal.com/";

    var settings = new HalkbankSettings(clientId, storeKey, isSandbox);
    var request = new HalkbankPaymentRequest(settings);

    request.Amount = 100;
    request.Oid = orderId.ToString();
    request.OkUrl = $"https://localhost:44357/Order/Success?orderId={request.Oid}";
    request.FailUrl = $"https://localhost:44357/Order/Fail?orderId={request.Oid}";
    request.CallbackUrl = $"https://localhost:44357/Order/Callback?orderId={request.Oid}";

    return View(request);
}
```

---

In the checkout view `Checkout.cshtml` call [`Model.ToHtmlForm`](https://github.com/vgichar/PaymentGateway.Halkbank.MK/blob/master/Halkbank.MK/HalkbankPaymentRequest.cs) to render the required form with all required data that Halkbank needs to complete the payment.  

```
@model Halkbank.MK.HalkbankPaymentRequest
@Html.Raw(Model.ToHtmlForm(@"<button type='submit'>Pay now</button>"))
```

This form when submitted will redirect the customer to the Halkbank payment portal in order to complete the order.

---

To confirm the payment order you should expect a callback on the URL as configured in the [`HalkbankPaymentRequest.CallbackUrl`](https://github.com/vgichar/PaymentGateway.Halkbank.MK/blob/master/Halkbank.MK/HalkbankPaymentRequest.cs).  

This callback request should be processed using the [`HalkbankPaymentResponse`](https://github.com/vgichar/PaymentGateway.Halkbank.MK/blob/master/Halkbank.MK/HalkbankPaymentResponse.cs) model as shown below.


```
[HttpGet]
public ActionResult Callback(int orderId)
{
    // halkbank provided credentials
    string storeKey = "halkbank provided key";
    string clientId = "halkbank provided client id";
    string portalUrl = "https://halkbank.provided.portal.com/";

    var body = new StreamReader(Request.Body).ReadToEnd();

    var settings = new HalkbankSettings(clientId, storeKey, isSandbox);
    var response = new HalkbankPaymentResponse(settings, body);

    if (response.Success)
    {
    	OrderService.MarkOrderAsPaid(orderId);
    }

    return Content(response.Response);
}
```

### Migration to production environment
When the solution is ready to be used in production environment make sure to change the setting [`PortalUrl`](https://github.com/vgichar/PaymentGateway.Halkbank.MK/blob/master/Halkbank.MK/HalkbankSettings.cs).

# Questions, bug reports or feature requests?
Do you have feature request or would you like to report a bug? Please post them on the [issue list](https://github.com/vgichar/PaymentGateway.Halkbank.MK/issues).

# License
Halkbank.MK is open source software, licensed under the terms of MIN license. See [LICENSE](https://github.com/vgichar/PaymentGateway.Halkbank.MK/blob/master/LICENSE) for details.

# How to build
Use Visual Studio 2017 and open the solution 'Halkbank.MK.sln'
