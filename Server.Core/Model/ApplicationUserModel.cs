using BlazorAuth.Shared.Dtos;
using Server.Entities.Entities;

namespace Server.Core.Model;

public sealed class ApplicationUserModel
{
    public static ApplicationUser CreateUser()
    {
        try
        {
            return Activator.CreateInstance<ApplicationUser>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        }
    }

    public static UserDetail GetUserDetails(UserDetailDto userDetailDto)
    {
        return new UserDetail
        {
            FirstName = userDetailDto.FirstName,
            Surname = userDetailDto.Surname,
            BirthDate = userDetailDto.BirthDate,
            Gender = userDetailDto.Gender,
        };
    }
}


