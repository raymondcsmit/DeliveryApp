using Core;
using DeliveryApp.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace Delivery.Services.Implementation
{
    public class RoleService : IRoleStore<Role>
    {
        private readonly IRepository<Role> _roleRepository;

        public RoleService(IRepository<Role> roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken)
        {
            await _roleRepository.AddAsync(role);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken)
        {
            await _roleRepository.DeleteAsync(role.Id);
            return IdentityResult.Success;
        }

        public void Dispose()
        {

        }

        public async Task<Role?> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            return await _roleRepository.GetByIdAsync(roleId);
        }

        public async Task<Role?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            var roles = await _roleRepository.GetByAsync(new Dictionary<string, object>
        {
            { "NormalizedName", normalizedRoleName }
        });
            return roles.FirstOrDefault();
        }

        public Task<string?> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }

        public Task<string?> GetRoleIdAsync(Role role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id.ToString());
        }

        public Task<string?> GetRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public async Task SetNormalizedRoleNameAsync(Role role, string? normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            await UpdateAsync(role, cancellationToken);
        }

        public async Task SetRoleNameAsync(Role role, string? roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            await UpdateAsync(role, cancellationToken);
        }

        public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken)
        {
            await _roleRepository.UpdateAsync(role);
            return IdentityResult.Success;
        }
    }

}
