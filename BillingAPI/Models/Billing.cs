using System.ComponentModel.DataAnnotations;

namespace BillingAPI.Models
{
    public class Billing
    {
        public int Id { get; set; }
        public string CustomerAccountName { get; set; }
        public int CustomerID { get; set; }
        public int OrderID { get; set; }
        public int BillingNumber { get; set; }
        public decimal PaymentAmount { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
