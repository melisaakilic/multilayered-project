namespace Multiple_Layered_Service.Library.Dtos.OrderDtos
{
    public record ListAllOrderDto(Guid id, DateTime OrderDate, decimal TotalAmount, Guid UserId, string UserFullName);
}
