using Microsoft.AspNetCore.Authorization;


namespace Multiple_Layered.API.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// Tüm Kullanıcıları Getirir
    /// </summary>
    [HttpGet("all")]
    [AllowAnonymous]
    [TimeRestrict("09:00", "23:59")] // Özel saatler ile
    [ProducesResponseType(typeof(IEnumerable<ListAllUserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllUsers([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        try
        {
            var pagination = new Pagination(page, size);
            var result = await _userService.GetAllAsync(pagination);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kullanıcılar Getirilirken Bir Hata Meydana Geldi");
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Id'ye Göre Kullanıcı Getirir
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ListAllUserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        try
        {
            var result = await _userService.GetByIdAsync(id);
            if (result is null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kullanıcı Getirilirken Bir Hata Meydana Geldi");
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Kullanıcı Bilgilerini Günceller
    /// </summary>
    [HttpPut("update")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<IdentityError>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto updateUserDto)
    {
        try
        {
            var result = await _userService.UpdateAsync(updateUserDto);

            if (result.Succeeded)
                return Ok();

            _logger.LogWarning("Kullanıcı güncellenirken validasyon hataları: {@Errors}", result.Errors);
            return BadRequest(result.Errors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kullanıcı güncellenirken bir hata meydana geldi");
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Kullanıcıyı Siler
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        try
        {
            await _userService.DeleteAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kullanıcı silinirken bir hata meydana geldi");
            return StatusCode(500);
        }
    }
}