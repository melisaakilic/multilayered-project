namespace Multiple_Layered_Service.Library.Dtos.OrderProduct
{
    public record ListAllOrderProductDto
    (
        Guid OrderId,
        Guid ProductId,
        string ProductName,      
        decimal UnitPrice,       
        int Quantity,
        decimal TotalPrice,     
        DateTime OrderDate
    );
}
