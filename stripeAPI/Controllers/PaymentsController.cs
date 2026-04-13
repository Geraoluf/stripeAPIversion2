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

                SuccessUrl = "https://apiversion4-fthsdbbcc0ggdydb.denmarkeast-01.azurewebsites.net/Success/Success",
                CancelUrl = "https://apiversion4-fthsdbbcc0ggdydb.denmarkeast-01.azurewebsites.net/Success/Cancel"
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
