using Microsoft.EntityFrameworkCore;
using SmolPass.Application.Interfaces;
using SmolPass.Domain.Entities;
using SmolPass.Infrastructure.Persistence;

namespace SmolPass.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddAsync(User user, CancellationToken cancellationToken)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
