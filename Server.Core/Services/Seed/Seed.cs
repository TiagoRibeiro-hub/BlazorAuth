using Microsoft.AspNetCore.Identity;
using Server.Core.Model;
using Server.Core.Services.Email;
using Server.Entities.Constants;
using Server.Entities.Entities;


namespace Server.Core.Services.Seed
{
    public interface ISeed
    {
        Task CreateRolesAndAdmin();
    }

    public sealed class Seed : ISeed
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;

        public Seed(
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager,
            IUserStore<ApplicationUser> userStore)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
        }


        public async Task CreateRolesAndAdmin()
        {
            var email = "sendysauth@gmail.com"; //
            var user = await _userManager.FindByEmailAsync(email);


            if (!await _roleManager.RoleExistsAsync(Role.Sendys))
            {
                await _roleManager.CreateAsync(new IdentityRole(Role.Sendys));
                await _roleManager.CreateAsync(new IdentityRole(Role.Admin));
                await _roleManager.CreateAsync(new IdentityRole(Role.User));
            }

            if (user == null)
            {
                user = ApplicationUserModel.CreateUser();


                var userStore = UserStore(user, email);

                user.Detail = new UserDetail
                {
                    FirstName = "Admin Sendys",
                    Surname = "Sendys",
                    BirthDate = DateTime.Now,
                    Gender = BlazorAuth.Shared.Enums.Gender.Male,
                };
                await userStore;
                await _userManager.CreateAsync(user, "Admin@1234");

                await _userManager.AddToRolesAsync(user, new[] { Role.Sendys, Role.Admin });

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                await _userManager.ConfirmEmailAsync(user, token);
            }

        }

        private async Task UserStore(ApplicationUser user, string email)
        {
            await _userStore.SetUserNameAsync(user, email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, email, CancellationToken.None);
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
