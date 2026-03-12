using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace stripeAPI.Controllers
{
    [Route("api/webhook")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Handle()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            Event stripeEvent = EventUtility.ParseEvent(json);

            if (stripeEvent.Type == "checkout.session.completed")
            {
                var session = stripeEvent.Data.Object as Stripe.Checkout.Session;

                Console.WriteLine("Betaling gennemført!");
                Console.WriteLine($"Session ID: {session.Id}");
            }

            return Ok();
        }
    }
}