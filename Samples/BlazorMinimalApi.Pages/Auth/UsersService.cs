﻿using BlazorMinimalApis.Data;
using BlazorMinimalApis.Pages.Data;

using Microsoft.EntityFrameworkCore;

using System.Security.Cryptography;
using System.Text;

namespace BlazorMinimalApis.Pages.Auth
{
    public class UsersService
    {
        private readonly IDbContextFactory<AppDbContext> _factory;

        public UsersService(IDbContextFactory<AppDbContext> factory)
        {
            _factory = factory;
        }

        public async Task<User?> FindUserAsync(int userId)
        {
            using var context = _factory.CreateDbContext();
            return await context.Users.FindAsync(userId);
        }

        public async Task<User?> FindUserAsync(string username, string password)
        {
            using var context = _factory.CreateDbContext();
            return await context.Users.FirstOrDefaultAsync(x => x.Email == username && x.Password == password);
        }

        public async Task<User?> FindUserByEmailAsync(string email)
        {
            using var context = _factory.CreateDbContext();
            return await context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            using var context = _factory.CreateDbContext();
            var addedUser = await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            await context.UserRoles.AddAsync(new UserRole { RoleId = 1, User = user });
            await context.SaveChangesAsync();
            return addedUser.Entity;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            using var context = _factory.CreateDbContext();
            return await context.Users
                    .ToListAsync();
        }

        public string GetSha256Hash(string input)
        {
            using (var hashAlgorithm = SHA256.Create())
            {
                var byteValue = Encoding.UTF8.GetBytes(input);
                var byteHash = hashAlgorithm.ComputeHash(byteValue);
                return Convert.ToBase64String(byteHash);
            }
        }
    }

}
