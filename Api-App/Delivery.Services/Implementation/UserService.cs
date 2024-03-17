using Core;
using DeliveryApp.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Delivery.Services.Implementation
{
    public class UserService :
    IUserStore<User>,
    IUserEmailStore<User>,
    IUserPhoneNumberStore<User>,
    IUserTwoFactorStore<User>,
    IUserPasswordStore<User>,
    IUserRoleStore<User>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IOptions<RepositoryOptions> _options;
        public UserService(IRepository<User> userRepository, IRepository<Role> roleRepository, IOptions<RepositoryOptions> options)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _options = options;
        }


        public async Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            var roles = await _roleRepository.GetByAsync(new Dictionary<string, object>
                    {
                        { "Name", roleName }
                    });
            // Find the role by name
            Role role = roles.FirstOrDefault();

            if (role != null)
            {
                // Check if the user is already assigned to the role
                if (!user.UserRoles.Any(ur => ur.RoleId == role.Id))
                {
                    // Create a new UserRole instance
                    UsersRole userRole = new UsersRole
                    {
                        UserId = user.Id,
                        RoleId = role.Id
                    };

                    // Add the UserRole to the User's UserRoles collection
                    user.UserRoles.Add(userRole);

                    // Update the user
                    await _userRepository.UpdateAsync(user);
                }
                // If the user is already assigned to the role, you can choose to handle it differently,
                // such as throwing an exception or returning a specific result.
            }
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            try
            {
                await _userRepository.AddAsync(user);
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                // Log the error
                return IdentityResult.Failed(new IdentityError { Description = ex.Message });
            }
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            await _userRepository.DeleteAsync(user.Id);
            return IdentityResult.Success;
        }

        public void Dispose()
        {
            // Dispose any resources if needed
        }

        public async Task<User?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetByAsync(new Dictionary<string, object>
        {
            { "NormalizedEmail", normalizedEmail }
        });
            var user = users.FirstOrDefault();
            // If you need to load UserRoles separately, do it here
            // For example, you might have a method to load roles by user ID
            if (user != null)
            {
                // Assuming you have a method like this, which you'll need to implement
                user.UserRoles = await LoadUserRolesAsync(user.Id);
            }
            return users.FirstOrDefault();
        }


        public async Task<User?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            //return await _userRepository.GetByIdAsync(userId);
            var user = await _userRepository.GetByIdAsync(userId);
            // If you need to load UserRoles separately, do it here
            // For example, you might have a method to load roles by user ID
            if (user != null)
            {
                // Assuming you have a method like this, which you'll need to implement
                user.UserRoles = await LoadUserRolesAsync(user.Id);
            }
            return user;
        }

        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetByAsync(new Dictionary<string, object>
        {
            { "NormalizedUserName", normalizedUserName }
        });
            var user = users.FirstOrDefault();
            // If you need to load UserRoles separately, do it here
            // For example, you might have a method to load roles by user ID
            if (user != null)
            {
                // Assuming you have a method like this, which you'll need to implement
                user.UserRoles = await LoadUserRolesAsync(user.Id);
            }
            return user;
            //return users.FirstOrDefault();
        }

        public Task<string?> GetEmailAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task<string?> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task<string?> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string?> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<string?> GetPhoneNumberAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        //public Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken)
        //{
        //    return Task.FromResult<IList<string>>(user.Roles);
        //}
        public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken)
        {

            string userTableName;
            _options.Value.TableNames.TryGetValue(typeof(User).Name, out userTableName);
            string userRoleTableName;
            _options.Value.TableNames.TryGetValue(typeof(UsersRole).Name, out userRoleTableName);
            string roleTableName;
            _options.Value.TableNames.TryGetValue(typeof(Role).Name, out roleTableName);

            string query = $@"
                SELECT r.Name
                FROM {userTableName} AS u
                INNER JOIN {userRoleTableName} AS ur ON u.User_Id = ur.User_Id
                INNER JOIN {roleTableName} AS r ON ur.Role_Id = r.Role_Id
                WHERE u.Id = @UserId
            ";

            var roleNames = await _userRepository.ExecuteQueryAsync<string>(query, new { UserId = user.Id });
            return roleNames.ToList();
        }


        public Task<bool> GetTwoFactorEnabledAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }
        private async Task<ICollection<UsersRole>> LoadUserRolesAsync(int userId)
        {
            string userRoleTableName;
            _options.Value.TableNames.TryGetValue(typeof(UsersRole).Name, out userRoleTableName);
            string roleTableName;
            _options.Value.TableNames.TryGetValue(typeof(Role).Name, out roleTableName);
            // This is a simplification. Adjust the SQL query to match your database schema.
            var query = $@"
                         SELECT ur.*
                         FROM {userRoleTableName} ur
                         JOIN {roleTableName} r ON ur.ROLE_ID = r.ROLE_ID
                         WHERE ur.USER_ID = @UserId
                             ";

            var userRoles = await _userRepository.ExecuteQueryAsync<UsersRole>(query, new { UserId = userId });
            return userRoles.ToList();
        }
        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string?> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            // Implement querying users in role using your generic repository
            // You need to have a mechanism in your user model to store roles
            throw new NotImplementedException();
        }

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            // Implement checking if the user has a password set
            throw new NotImplementedException();
        }

        public Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            // Implement checking if the user is in a specific role
            // You need to have a mechanism in your user model to store roles
            throw new NotImplementedException();
        }

        //public async Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        //{
        //    // Implement removing user from role using your generic repository
        //    // You need to have a mechanism in your user model to store roles
        //    user.Roles.Remove(roleName);
        //    await _userRepository.UpdateAsync(user);
        //}
        public async Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            var roles = await _roleRepository.GetByAsync(new Dictionary<string, object>
        {
            { "Name", roleName }
        });
            // Find the role by name
            Role role = roles.FirstOrDefault();

            if (role != null)
            {
                // Find the UserRole entry for the user and role
                UsersRole userRole = user.UserRoles.FirstOrDefault(ur => ur.RoleId == role.Id);

                if (userRole != null)
                {
                    // Remove the UserRole from the User's UserRoles collection
                    user.UserRoles.Remove(userRole);

                    // Update the user
                    await _userRepository.UpdateAsync(user);
                }
                // If the user is not assigned to the role, you can choose to handle it differently,
                // such as throwing an exception or returning a specific result.
            }
            else
            {
                // Handle the case where the role does not exist
                // This could be logging an error, throwing an exception, or returning a specific result.
            }
        }

        public Task SetEmailAsync(User user, string? email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.CompletedTask;
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.CompletedTask;
        }

        public Task SetNormalizedEmailAsync(User user, string? normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.CompletedTask;
        }

        public Task SetNormalizedUserNameAsync(User user, string? normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(User user, string? passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task SetPhoneNumberAsync(User user, string? phoneNumber, CancellationToken cancellationToken)
        {
            user.PhoneNumber = phoneNumber;
            return Task.CompletedTask;
        }

        public Task SetPhoneNumberConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        {
            user.PhoneNumberConfirmed = confirmed;
            return Task.CompletedTask;
        }

        public Task SetTwoFactorEnabledAsync(User user, bool enabled, CancellationToken cancellationToken)
        {
            user.TwoFactorEnabled = enabled;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(User user, string? userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            try
            {
                await _userRepository.UpdateAsync(user);
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                // Log the error
                return IdentityResult.Failed(new IdentityError { Description = ex.Message });
            }
        }
    }
}
