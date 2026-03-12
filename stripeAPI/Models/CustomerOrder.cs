namespace stripeAPI.Models
{
    public class CustomerOrder
    {
        public int Id { get; set; }  // Primær nøgle
        public string Email { get; set; }  // Kundens email
        public string ProductName { get; set; }  // Produkt
        public int Quantity { get; set; }  // Hvor mange
        public decimal Amount { get; set; }  // Beløb
        public string Status { get; set; }  // fx "paid"
        public string StripeSessionId { get; set; }  // Session ID
        public DateTime CreatedAt { get; set; }  // Tidspunkt
    }
}
