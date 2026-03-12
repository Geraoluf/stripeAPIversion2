using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Collections.Generic;

namespace stripeAPI.Controllers
{
    // Routing for API’et: https://localhost:7090/api/payments
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        // POST endpoint: /api/payments/create-checkout-session
        [HttpPost("create-checkout-session")]
        public ActionResult CreateCheckoutSession()
        {
            // 1️⃣ Konfiguration af Stripe Checkout-session
            var options = new SessionCreateOptions
            {
                // Hvilke betalingsmetoder vi accepterer
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },

                // Hvilke produkter/priser der skal købes
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = 2000, // beløb i øre (20,00 DKK)
                            Currency = "dkk",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Test Produkt",
                            }
                        },
                        Quantity = 1
                    }
                },

                // Mode: engangsbetaling
                Mode = "payment",

                // URLs til success og cancel
                SuccessUrl = "https://localhost:7114/success.html",
                CancelUrl = "https://localhost:7114/cancel.html"
            };

            // 2️⃣ Opret Stripe session
            var service = new SessionService();
            var session = service.Create(options);

            // 3️⃣ Returnér session-id til frontend
            return Ok(new
            {
                id = session.Id
            });
        }
    }
}
