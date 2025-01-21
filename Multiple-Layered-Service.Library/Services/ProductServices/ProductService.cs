namespace Multiple_Layered_Service.Library.Services.ProductServices;
public class ProductService : IProductService
{
    readonly IUnitOfWork _unitOfWork;
    readonly ILogger<ProductService> _logger;

    public ProductService(IUnitOfWork unitOfWork, ILogger<ProductService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PagedResult<ListAllProductDto>> GetAllAsync(Pagination pagination)
    {
        try
        {
            var products = await _unitOfWork.Products.GetAllAsync();
            if (!products.Any())
                throw new KeyNotFoundException();
            var totalCount = products.Count();

            var paginatedProducts = products
                .Skip((pagination.Page - 1) * pagination.Size)
                .Take(pagination.Size);

            var productDtos = paginatedProducts.Select(p => new ListAllProductDto(
                p.Id,
                p.Name,
                p.Price,
                p.Stock
            ));

            var result = new PagedResult<ListAllProductDto>(
                productDtos,
                totalCount,
                pagination.Page,
                pagination.Size
            );

           

            _logger.LogInformation("Ürünler başarıyla getirildi. Sayfa: {Page}, Boyut: {Size}, Toplam: {Total}",
                pagination.Page, pagination.Size, totalCount);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürünleri getirirken hata oluştu. Sayfa: {Page}, Boyut: {Size}",
                pagination.Page, pagination.Size);
            throw;
        }
    }

    public async Task<ListAllProductDto> GetByIdAsync(Guid id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product is null)
            throw new KeyNotFoundException();

        return new ListAllProductDto(
            product.Id,
            product.Name,
            product.Price,
            product.Stock
        );
    }

    public async Task<CreateProductDto> AddAsync(CreateProductDto createProductDto)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var product = new Product
            {
                Name = createProductDto.Name,
                Price = createProductDto.Price,
                Stock = createProductDto.Stock
            };

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();       
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Yeni ürün eklendi: {ProductName}", product.Name);

            return createProductDto;
        }

        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Ürün eklenirken hata: {ProductName}", createProductDto.Name);
            throw;
        }
    }

    public async Task<UpdateProductDto> UpdateAsync(UpdateProductDto updateProductDto)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var product = await _unitOfWork.Products.GetByIdAsync(updateProductDto.Id);
            if (product is null)
                throw new KeyNotFoundException();

            product.Name = updateProductDto.Name;
            product.Price = updateProductDto.Price;
            product.Stock = updateProductDto.Stock;

            await _unitOfWork.Products.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Ürün güncellendi: {ProductName}", product.Name);

            return updateProductDto;
        }

        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Ürün güncellenirken hata: {ProductId}", updateProductDto.Id);
            throw;
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product is null)
                throw new KeyNotFoundException();

            await _unitOfWork.Products.DeleteAsync(product.Id);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Ürün silindi: {ProductId}", id);
        }

        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Ürün silinirken hata: {ProductId}", id);
            throw;
        }
    }
}