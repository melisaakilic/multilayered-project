namespace Multiple_Layered_Service.Library.Dtos.AuthDtos
{
    public record RegisterDto
    (
        string FirstName,
        string LastName,
        string UserName,
        string Email,
        string Password
    );
}
