using BlazorMinimalApis.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorMinimalApis.Auth
{
    public class RolesService
    {
        private readonly AppDbContext _context;

        public RolesService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Role>> FindUserRolesAsync(int userId)
        {
            var roles = await _context.Roles.Where(role => role.UserRoles.Any(x => x.UserId == userId)).ToListAsync();
            return roles;
        }

        public async Task<bool> IsUserInRole(int userId, string roleName)
        {
            var userRolesQuery = from role in _context.Roles
                                 where role.Name == roleName
                                 from user in role.UserRoles
                                 where user.UserId == userId
                                 select role;
            var userRole = await userRolesQuery.FirstOrDefaultAsync();
            return userRole != null;
        }

        public async Task<List<User>> FindUsersInRoleAsync(string roleName)
        {
            var roleUserIdsQuery = from role in _context.Roles
                                   where role.Name == roleName
                                   from user in role.UserRoles
                                   select user.UserId;
            return await _context.Users.Where(user => roleUserIdsQuery.Contains(user.Id))
                .ToListAsync();
        }
    }

}
