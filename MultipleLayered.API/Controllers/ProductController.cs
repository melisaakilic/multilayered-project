using Microsoft.AspNetCore.Authorization;


namespace Multiple_Layered.API.Controllers;

[ApiController]
[Route("api/product")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductController> _logger;

    public ProductController(IProductService productService, ILogger<ProductController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    /// <summary>
    /// Tüm Ürünleri Getirir
    /// </summary>
    [HttpGet("all")]
    [TimeRestrict("09:00", "23:59")] // Özel saatler ile
    [ProducesResponseType(typeof(IEnumerable<ListAllProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllProducts([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        try
        {
            var pagination = new Pagination(page, size);
            var result = await _productService.GetAllAsync(pagination);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürünler listelenirken hata meydana geldi");
            throw;
        }
    }

    /// <summary>
    /// ID'ye Göre Ürün Getirir
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ListAllProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        try
        {
            var result = await _productService.GetByIdAsync(id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ID: {Id} olan ürün getirilirken hata meydana geldi", id);
            throw;
        }
    }

    /// <summary>
    /// Yeni Ürün Oluşturur
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(CreateProductDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto createProductDto)
    {
        try
        {
            var result = await _productService.AddAsync(createProductDto);
            return Created($"/api/product/{result.Id}", result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürün eklenirken hata meydana geldi");
            throw;
        }
    }

    /// <summary>
    /// Ürün Günceller
    /// </summary>
    [HttpPut]
    [Authorize]
    [ProducesResponseType(typeof(UpdateProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductDto updateProductDto)
    {
        try
        {
            var result = await _productService.UpdateAsync(updateProductDto);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ID: {Id} olan ürün güncellenirken hata meydana geldi", updateProductDto.Id);
            throw;
        }
    }

    /// <summary>
    /// Ürün Siler
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        try
        {
            await _productService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ID: {Id} olan ürün silinirken hata meydana geldi", id);
            throw;
        }
    }
}