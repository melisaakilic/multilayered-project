namespace Multiple_Layered_Service.Library.Services.RoleServices
{
    public interface IRoleService
    {
        Task<Response> AssignRoleAsync(AssignRoleDto assignRoleDto);
    }
}
