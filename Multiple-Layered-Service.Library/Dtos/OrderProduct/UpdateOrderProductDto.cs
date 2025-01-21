namespace Multiple_Layered_Service.Library.Dtos.OrderProduct
{
    public record UpdateOrderProductDto(Guid OrderId, Guid ProductId, int Quantity);
}
