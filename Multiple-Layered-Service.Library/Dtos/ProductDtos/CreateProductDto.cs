namespace Multiple_Layered_Service.Library.Dtos.ProductDtos
{
    public record CreateProductDto(Guid Id, string Name, decimal Price, int Stock);
}
