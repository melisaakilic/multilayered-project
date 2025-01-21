namespace Multiple_Layered_Service.Library.Dtos.UserDtos
{
    public record ListAllUserDto
        (
            Guid Id,
            string FirstName,
            string LastName,
            string Username,
            string Email,
            string PhoneNumber,
            bool EmailConfirmed,
            bool PhoneNumberConfirmed,
            bool TwoFactorEnabled
        );
}
