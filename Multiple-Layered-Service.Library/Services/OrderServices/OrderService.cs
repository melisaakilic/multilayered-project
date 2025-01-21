namespace Multiple_Layered_Service.Library.Services.OrderServices;
public class OrderService : IOrderService
{
    readonly IUnitOfWork _unitOfWork;
    readonly ILogger<OrderService> _logger;

    public OrderService(IUnitOfWork unitOfWork, ILogger<OrderService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PagedResult<ListAllOrderDto>> GetAllAsync(Pagination pagination)
    {
        try
        {
            var orders = await _unitOfWork.Orders.GetAllAsync();
            if (!orders.Any())
                throw new KeyNotFoundException();
            var users = await _unitOfWork.Users.GetAllAsync();

            var orderDtoList = (from o in orders
                                join u in users on o.UserId equals u.Id
                                select new ListAllOrderDto(
                                    o.Id,
                                    o.OrderDate,
                                    o.TotalAmount,
                                    o.UserId,
                                    $"{u.FirstName} {u.LastName}"
                                )).ToList();

            var totalCount = orderDtoList.Count;

            var paginatedOrders = orderDtoList
                .Skip((pagination.Page - 1) * pagination.Size)
                .Take(pagination.Size);

            var result = new PagedResult<ListAllOrderDto>(
                paginatedOrders,
                totalCount,
                pagination.Page,
                pagination.Size
            );

          

            _logger.LogInformation("Siparişler başarıyla getirildi. Sayfa: {Page}, Boyut: {Size}, Toplam: {Total}",
                pagination.Page, pagination.Size, totalCount);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Siparişleri getirirken hata oluştu. Sayfa: {Page}, Boyut: {Size}",
                pagination.Page, pagination.Size);
            throw;
        }
    }

    public async Task<ListAllOrderDto> GetByIdAsync(Guid id)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(id);
        if (order is null)
            throw new KeyNotFoundException();

        var user = await _unitOfWork.Users.GetByIdAsync(order.UserId);
        if (user is null)
            throw new KeyNotFoundException();

        return new ListAllOrderDto(
            order.Id,
            order.OrderDate,
            order.TotalAmount,
            order.UserId,
            $"{user.FirstName} {user.LastName}"
        );
    }

    public async Task<CreateOrderDto> AddAsync(CreateOrderDto createOrderDto)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var order = new Order
            {
                UserId = createOrderDto.UserId,
                TotalAmount = createOrderDto.TotalAmount,
                OrderDate = DateTime.Now
            };

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();
           
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Yeni sipariş eklendi: {OrderId}", order.Id);

            return createOrderDto;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Sipariş eklenirken hata oluştu");
            throw;
        }
    }

    public async Task<UpdateOrderDto> UpdateAsync(UpdateOrderDto updateOrderDto)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var order = await _unitOfWork.Orders.GetByIdAsync(updateOrderDto.Id);
            if (order is null)
                throw new KeyNotFoundException();

            order.TotalAmount = updateOrderDto.TotalAmount;

            await _unitOfWork.Orders.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Sipariş güncellendi: {OrderId}", order.Id);

            return updateOrderDto;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Sipariş güncellenirken hata oluştu");
            throw;
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order is null)
                throw new KeyNotFoundException();

            await _unitOfWork.Orders.DeleteAsync(order.Id);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Sipariş silindi: {OrderId}", id);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Sipariş silinirken hata oluştu");
            throw;
        }
    }
}