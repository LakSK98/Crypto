using Crypto.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crypto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Processing(Payment payment)
        {
            try
            {
                //var optionToken = new TokenCreateOptions
                //{
                //    Card = new TokenCardOptions
                //    {
                //        Number = 'CreditCardNumber',
                //        ExpMonth = 'Expire Month',
                //        ExpYear = 'Expire Year',
                //        Cvc = 'CVC',
                //        Name = 'Name on card'
                //    },
                //};
                //var tokenService = new TokenService();
                //Token paymentToken = await tokenService.CreateAsync(optionToken);
                Dictionary<string, string> Metadata = new Dictionary<string, string>();
                Metadata.Add("Product", "Name of Product");
                Metadata.Add("Quantity", "1");
                var options = new ChargeCreateOptions
                {
                    Amount = payment.Amount,
                    Currency = "USD",
                    Description = payment.Description,
                    Source = payment.Token,
                    ReceiptEmail = payment.Email,
                    Metadata = Metadata
                };
                var service = new ChargeService();
                Charge charge = await service.CreateAsync(options);
                if (charge.Status.ToLower().Equals("succeeded")) {
                    return Ok("Payment success.");
                }
                else
                {
                    return BadRequest("Payment failed.");
                }
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
