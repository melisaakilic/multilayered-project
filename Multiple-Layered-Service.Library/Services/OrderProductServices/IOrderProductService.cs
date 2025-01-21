namespace Multiple_Layered_Service.Library.Services.OrderProductServices
{
    public interface IOrderProductService
    {
        Task<PagedResult<ListAllOrderProductDto>> GetAllAsync(Pagination pagination);
        Task<ListAllOrderProductDto> GetByIdAsync(Guid orderId, Guid productId);
        Task<CreateOrderProductDto> AddAsync(CreateOrderProductDto createOrderProductDto);
        Task<UpdateOrderProductDto> UpdateAsync(UpdateOrderProductDto updateOrderProductDto);
        Task DeleteAsync(Guid orderId, Guid productId);
    }
}
