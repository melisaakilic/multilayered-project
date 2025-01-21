namespace Multiple_Layered_DataAccess.Library.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
