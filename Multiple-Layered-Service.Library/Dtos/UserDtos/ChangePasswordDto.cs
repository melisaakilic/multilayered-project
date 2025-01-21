namespace Multiple_Layered_Service.Library.Dtos.UserDtos
{
    public record ChangePasswordDto
    (
     string Email,
     string CurrentPassword,
     string NewPassword,
     string ConfirmNewPassword
    )
    {
        public bool PasswordsMatch => NewPassword == ConfirmNewPassword;
    }
}
