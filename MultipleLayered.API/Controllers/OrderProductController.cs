using Microsoft.AspNetCore.Authorization;

namespace Multiple_Layered.API.Controllers;

[ApiController]
[Route("api/order-product")]
public class OrderProductController : ControllerBase
{
    private readonly IOrderProductService _orderProductService;
    private readonly ILogger<OrderProductController> _logger;

    public OrderProductController(IOrderProductService orderProductService, ILogger<OrderProductController> logger)
    {
        _orderProductService = orderProductService;
        _logger = logger;
    }

    /// <summary>
    /// Tüm Sipariş Detaylarını Getirir
    /// </summary>
    [HttpGet("all")]
    [TimeRestrict("09:00", "23:59")] // Özel saatler ile
    [ProducesResponseType(typeof(IEnumerable<ListAllOrderProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllOrderProducts([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        try
        {
            var pagination = new Pagination(page, size);
            var result = await _orderProductService.GetAllAsync(pagination);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sipariş detayları listelenirken bir hata meydana geldi");
            throw;
        }
    }

    /// <summary>
    /// ID'ye Göre Sipariş Detayı Getirir
    /// </summary>
    [HttpGet("{orderId}/{productId}")]
    [ProducesResponseType(typeof(ListAllOrderProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOrderProductById(Guid orderId, Guid productId)
    {
        try
        {
            var result = await _orderProductService.GetByIdAsync(orderId, productId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sipariş detayı getirilirken bir hata meydana geldi");
            throw;
        }
    }

    /// <summary>
    /// Yeni Sipariş Detayı Oluşturur
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(CreateOrderProductDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateOrderProduct([FromBody] CreateOrderProductDto createOrderProductDto)
    {
        try
        {
            var result = await _orderProductService.AddAsync(createOrderProductDto);
            return Created($"/api/order-product/{result.OrderId}/{result.ProductId}", result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sipariş detayı eklenirken bir hata meydana geldi");
            throw;
        }
    }

    /// <summary>
    /// Sipariş Detayı Günceller
    /// </summary>
    [HttpPut]
    [Authorize]
    [ProducesResponseType(typeof(UpdateOrderProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateOrderProduct([FromBody] UpdateOrderProductDto updateOrderProductDto)
    {
        try
        {
            var result = await _orderProductService.UpdateAsync(updateOrderProductDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sipariş detayı güncellenirken bir hata meydana geldi");
            throw;
        }
    }

    /// <summary>
    /// Sipariş Detayı Siler
    /// </summary>
    [HttpDelete("{orderId}/{productId}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteOrderProduct(Guid orderId, Guid productId)
    {
        try
        {
            await _orderProductService.DeleteAsync(orderId, productId);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sipariş detayı silinirken bir hata meydana geldi");
            throw;
        }
    }
}