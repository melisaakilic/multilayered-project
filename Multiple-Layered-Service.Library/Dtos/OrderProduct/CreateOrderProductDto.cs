namespace Multiple_Layered_Service.Library.Dtos.OrderProduct
{
    public record CreateOrderProductDto(Guid OrderId, Guid ProductId, int Quantity);
}
