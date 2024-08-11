using Microsoft.AspNetCore.Mvc;
using BillingAPI.Models;
using BillingAPI.Data;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace BillingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BillingController : ControllerBase
    {
        private readonly BillingContext _context;

        public BillingController(BillingContext context)
        {
            _context = context;
        }

        [HttpPost("checkPaymentStatus")]
        public IActionResult CheckPaymentStatus([FromBody] PaymentStatusRequest request)
        {
            var billing = _context.Billings.FirstOrDefault(b => b.OrderID == request.OrderID && b.BillingNumber == request.BillingNumber);
            if (billing == null)
            {
                return NotFound(new { success = "false", message = "Order ID or Billing Number not found" });
            }

            return Ok(new { success = "true", message = "successful", billing });
        }

        [HttpPost("updatePaymentStatus")]
        public IActionResult UpdatePaymentStatus([FromBody] PaymentStatusRequest request)
        {
            var billing = _context.Billings.FirstOrDefault(b => b.OrderID == request.OrderID && b.BillingNumber == request.BillingNumber);
            if (billing == null)
            {
                return NotFound(new { success = "false", message = "Order ID or Billing Number not found" });
            }

            if (billing.PaymentStatus == "paid")
            {
                return BadRequest(new { success = "false", message = "payment is already paid" });
            }

            billing.PaymentStatus = "paid";
            _context.SaveChanges();

            return Ok(new { success = "true", message = "payment is successful", billing });
        }

        [HttpGet("getAllBilling")]
        public IActionResult GetAllBilling()
        {
            var billings = _context.Billings.ToList();
            return Ok(billings);
        }

        [NonAction]
        private void LogRequestResponse(string endpoint, string method, int statusCode, string requestBody, string responseBody)
        {
            var log = new Log
            {
                Endpoint = endpoint,
                Method = method,
                StatusCode = statusCode,
                RequestBody = requestBody,
                ResponseBody = responseBody
            };

            _context.Logs.Add(log);
            _context.SaveChanges();
        }

        public override OkObjectResult Ok(object value)
        {
            LogRequestResponse(Request.Path, Request.Method, 200, JsonSerializer.Serialize(Request.Body), JsonSerializer.Serialize(value));
            return base.Ok(value);
        }

        public override ObjectResult StatusCode(int statusCode, object value)
        {
            LogRequestResponse(Request.Path, Request.Method, statusCode, JsonSerializer.Serialize(Request.Body), JsonSerializer.Serialize(value));
            return base.StatusCode(statusCode, value);
        }
    }

    public class PaymentStatusRequest
    {
        public int OrderID { get; set; }
        public int BillingNumber { get; set; }
    }
}
