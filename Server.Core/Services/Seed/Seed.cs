using Microsoft.AspNetCore.Identity;
using Server.Core.Model;
using BlazorAuth.Shared;
using Server.Entities.Entities;
using BlazorAuth.Shared.Enums;
using Server.Data.Repositories;
using BlazorAuth.Shared.Utils;


namespace Server.Core.Services.Seed
{
    public interface ISeed
    {
        Task CreateRolesAndAdmin();
        Task CreateUser();
    }

    public sealed class Seed : ISeed
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IBaseRepository _baseRepository;

        public Seed(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IBaseRepository baseRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _baseRepository = baseRepository;
        }


        public async Task CreateRolesAndAdmin()
        {
            var email = "sendysauth@gmail.com";
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                if (!await _roleManager.RoleExistsAsync(Role.Sendys))
                {
                    await _roleManager.CreateAsync(new IdentityRole(Role.Sendys));
                    await _roleManager.CreateAsync(new IdentityRole(Role.Admin));
                    await _roleManager.CreateAsync(new IdentityRole(Role.User));
                }
                _ = await CreateUser(email, "Admin@1234", "Admin", "Sendys", false, new[] { Role.Sendys, Role.Admin });
            }
        }

        public async Task CreateUser()
        {
            var email = $"user1sendys@email.com";
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                for (int i = 1; i < 11; i++)
                {
                    _ = await CreateUser($"user{i}sendys@email.com", $"User{i}@1234", $"User{i}", $"Sendys{i}", i % 2 == 0, new[] { Role.Sendys, Role.User });
                }
            }
        }

        private async Task<ApplicationUser> CreateUser(string email, string password, string firstName, string surname, bool isMale, string[] roles)
        {
            var user = ApplicationUserModel.CreateUser();
            user.UserName = email;
            user.Email = email;
            user.EmailConfirmed = true;
            user.Detail = new UserDetail
            {
                FirstName = firstName,
                Surname = surname,
                BirthDate = DateTime.Now,
                Gender = isMale ? Gender.Male : Gender.Female,
            };

            await _userManager.CreateAsync(user, password);
            await _userManager.AddToRolesAsync(user, roles);

            await AddStringValues(user);

            return user;
        }

        private async Task AddStringValues(ApplicationUser user)
        {
            Random rd = new Random();
            var stringValuesArray = new StringValue[100];
            for (int i = 0; i < 100; i++)
            {
                string str = Utils.GetStrings(rd);
                stringValuesArray[i] = new StringValue()
                {
                    UserId = user.Id,
                    Value = str
                };
            }
            await _baseRepository.AddRange<StringValue>(stringValuesArray);
        }
    }
}
