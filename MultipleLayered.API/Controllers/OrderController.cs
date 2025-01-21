using Microsoft.AspNetCore.Authorization;

namespace Multiple_Layered.API.Controllers;

[ApiController]
[Route("api/order")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly ILogger<OrderController> _logger;

    public OrderController(IOrderService orderService, ILogger<OrderController> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    /// <summary>
    /// Tüm Siparişleri Getirir
    /// </summary>
    [HttpGet("all")]
    [TimeRestrict("09:00", "23:59")] 
    [ProducesResponseType(typeof(IEnumerable<ListAllOrderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllOrders([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        try
        {
            var pagination = new Pagination(page, size);
            var result = await _orderService.GetAllAsync(pagination);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Siparişler listelenirken hata meydana geldi");
            throw;
        }
    }

    /// <summary>
    /// ID'ye Göre Sipariş Getirir
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ListAllOrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetOrderById(Guid id)
    {
        try
        {
            var result = await _orderService.GetByIdAsync(id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ID: {Id} olan sipariş getirilirken hata meydana geldi", id);
            throw;
        }
    }

    /// <summary>
    /// Yeni Sipariş Oluşturur
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(CreateOrderDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
    {
        try
        {
            var result = await _orderService.AddAsync(createOrderDto);
            return Created($"/api/order/{result.UserId}", result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sipariş oluşturulurken hata meydana geldi");
            throw;
        }
    }

    /// <summary>
    /// Sipariş Günceller
    /// </summary>
    [HttpPut]
    [Authorize]
    [ProducesResponseType(typeof(UpdateOrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderDto updateOrderDto)
    {
        try
        {
            var result = await _orderService.UpdateAsync(updateOrderDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ID: {Id} olan sipariş güncellenirken hata meydana geldi", updateOrderDto.Id);
            throw;
        }
    }

    /// <summary>
    /// Sipariş Siler
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteOrder(Guid id)
    {
        try
        {
            await _orderService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ID: {Id} olan sipariş silinirken hata meydana geldi", id);
            throw;
        }
    }
}