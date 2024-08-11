using System.ComponentModel.DataAnnotations;

namespace BillingAPI.Models
{
    public class Log
    {
        public int Id { get; set; }
        public string Endpoint { get; set; }
        public string Method { get; set; }
        public int StatusCode { get; set; }
        public string RequestBody { get; set; }
        public string ResponseBody { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
