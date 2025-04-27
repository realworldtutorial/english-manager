using EnglishManager.Domain.Entities;

namespace EnglishManager.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task<User> GetByEmailAsync(string email);
        Task<User> CreateAsync(User user);
        Task UpdateAsync(User user);
        Task<bool> EmailExistsAsync(string email);
        Task<Dictionary<int, string>> GetUserNamesAsync();
    }
}