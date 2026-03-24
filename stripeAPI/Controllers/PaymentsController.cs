using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Collections.Generic;

namespace stripeAPI.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        
        [HttpPost("create-checkout-session")]
        public ActionResult CreateCheckoutSession()
        {
            
            var options = new SessionCreateOptions
            {
               
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },

              
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = 2000, 
                            Currency = "dkk",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Test Produkt",
                            }
                        },
                        Quantity = 1
                    }
                },

               
                Mode = "payment",

                SuccessUrl = "https://localhost:7114/success.html",
                CancelUrl = "https://localhost:7114/cancel.html"
            };

          
            var service = new SessionService();
            var session = service.Create(options);

           
            return Ok(new
            {
                id = session.Id
            });
        }
    }
}
