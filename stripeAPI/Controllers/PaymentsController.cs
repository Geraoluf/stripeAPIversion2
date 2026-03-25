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

                SuccessUrl = "https://portfoliegertrasmussen-a3gpe7fzdncch0ct.denmarkeast-01.azurewebsites.net/success.html",
                CancelUrl = "https://portfoliegertrasmussen-a3gpe7fzdncch0ct.denmarkeast-01.azurewebsites.net/cancel.html"
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
