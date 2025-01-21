namespace Multiple_Layered_Service.Library.Services.OrderServices
{
    public interface IOrderService
    {
        Task<PagedResult<ListAllOrderDto>> GetAllAsync(Pagination pagination);
        Task<ListAllOrderDto> GetByIdAsync(Guid id);
        Task<UpdateOrderDto> UpdateAsync(UpdateOrderDto updateOrderDto);
        Task<CreateOrderDto> AddAsync(CreateOrderDto createOrderDto);
        Task DeleteAsync(Guid id);
    }
}
