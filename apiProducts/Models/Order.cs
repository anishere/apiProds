namespace apiProducts.Models
{
    public class Order
    {
        public int ID { get; set; }
        public string? PhoneNumber { get; set; } 
        public string? Name { get; set; }
        public string? Address { get; set; }

        public string? Note { get; set; }
        public string? Email { get; set; }

        public string? CodePayment { get; set; }
        public string? ListCart { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? Status { get; set; }
    }
}

