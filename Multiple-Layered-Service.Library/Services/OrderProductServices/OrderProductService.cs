namespace Multiple_Layered_Service.Library.Services.OrderProductServices
{
    public class OrderProductService : IOrderProductService
    {
        readonly IUnitOfWork _unitOfWork;
        readonly ILogger<OrderProductService> _logger;

        public OrderProductService(IUnitOfWork unitOfWork, ILogger<OrderProductService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<CreateOrderProductDto> AddAsync(CreateOrderProductDto createOrderProductDto)
        {
            try
            {
                var order = await _unitOfWork.Orders.GetByIdAsync(createOrderProductDto.OrderId);
                if (order is null)
                    throw new KeyNotFoundException();

                var product = await _unitOfWork.Products.GetByIdAsync(createOrderProductDto.ProductId);
                if (product is null)
                    throw new KeyNotFoundException();



                await _unitOfWork.BeginTransactionAsync();

                var orderProduct = new OrderProduct
                {
                    OrderId = createOrderProductDto.OrderId,
                    ProductId = createOrderProductDto.ProductId,
                    Quantity = createOrderProductDto.Quantity
                };

                await _unitOfWork.OrderProducts.AddAsync(orderProduct);
                await _unitOfWork.SaveChangesAsync();
               
                await _unitOfWork.CommitAsync();

                _logger.LogInformation("Yeni sipariş detayı eklendi");
                return createOrderProductDto;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError(ex, "Sipariş detayı eklenirken hata oluştu");
                throw;
            }
        }

        public async Task DeleteAsync(Guid orderId, Guid productId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var orderProduct = await _unitOfWork.OrderProducts.GetByCompositeKeyAsync(orderId, productId);
                if (orderProduct is null)
                    throw new KeyNotFoundException();

                await _unitOfWork.OrderProducts.DeleteAsync(orderProduct.OrderId);
                await _unitOfWork.SaveChangesAsync();
                
                await _unitOfWork.CommitAsync();

                _logger.LogInformation("Sipariş detayı silindi");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError(ex, "Sipariş detayı silinirken hata oluştu");
                throw;
            }
        }

        public async Task<PagedResult<ListAllOrderProductDto>> GetAllAsync(Pagination pagination)
        {
            try
            {
                var orderProducts = await _unitOfWork.OrderProducts.GetAllAsync();
                var products = await _unitOfWork.Products.GetAllAsync();
                var orders = await _unitOfWork.Orders.GetAllAsync();

                var orderProductList = (from op in orderProducts
                                        join p in products on op.ProductId equals p.Id
                                        join o in orders on op.OrderId equals o.Id
                                        select new ListAllOrderProductDto
                                        (
                                            op.OrderId,
                                            op.ProductId,
                                            p.Name,
                                            p.Price,
                                            op.Quantity,
                                            p.Price * op.Quantity,
                                            o.OrderDate
                                        )).ToList();

                var totalCount = orderProductList.Count;

                var paginatedOrderProducts = orderProductList
                    .Skip((pagination.Page - 1) * pagination.Size)
                    .Take(pagination.Size);

                var result = new PagedResult<ListAllOrderProductDto>(
                    paginatedOrderProducts,
                    totalCount,
                    pagination.Page,
                    pagination.Size
                );

                _logger.LogInformation("Sipariş detayları başarıyla getirildi. Sayfa: {Page}, Boyut: {Size}, Toplam: {Total}",
                    pagination.Page, pagination.Size, totalCount);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sipariş detayları getirilirken hata oluştu. Sayfa: {Page}, Boyut: {Size}",
                    pagination.Page, pagination.Size);
                throw;
            }
        }
        
        public async Task<ListAllOrderProductDto> GetByIdAsync(Guid orderId, Guid productId)
        {
            var orderProduct = await _unitOfWork.OrderProducts.GetByCompositeKeyAsync(orderId, productId);
            if (orderProduct is null)
                throw new KeyNotFoundException();

            var product = await _unitOfWork.Products.GetByIdAsync(productId);
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);

            return new ListAllOrderProductDto(
                orderProduct.OrderId,
                orderProduct.ProductId,
                product.Name,
                product.Price,
                orderProduct.Quantity,
                product.Price * orderProduct.Quantity,
                order.OrderDate
            );
        }

        public async Task<UpdateOrderProductDto> UpdateAsync(UpdateOrderProductDto updateOrderProductDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var orderProduct = await _unitOfWork.OrderProducts.GetByCompositeKeyAsync(updateOrderProductDto.OrderId, updateOrderProductDto.ProductId);

                if (orderProduct is null)
                    throw new KeyNotFoundException();

                orderProduct.Quantity = updateOrderProductDto.Quantity;

                await _unitOfWork.OrderProducts.UpdateAsync(orderProduct);
                await _unitOfWork.SaveChangesAsync();
            
                await _unitOfWork.CommitAsync();

                _logger.LogInformation("Sipariş detayı güncellendi");
                return updateOrderProductDto;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError(ex, "Sipariş detayı güncellenirken hata oluştu");
                throw;
            }
        }
    }
}
