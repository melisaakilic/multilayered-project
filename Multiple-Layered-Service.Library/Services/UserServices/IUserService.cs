

namespace Multiple_Layered_Service.Library.Services.UserServices
{
    public interface IUserService
    {
        Task<PagedResult<ListAllUserDto>> GetAllAsync(Pagination pagination);
        Task<ListAllUserDto> GetByIdAsync(Guid id);
        Task<IdentityResult> UpdateAsync(UpdateUserDto updateUserDto);
        Task<IdentityResult> ChangePasswordAsync(ChangePasswordDto changePasswordDto);
        Task DeleteAsync(Guid id);
    }
}
