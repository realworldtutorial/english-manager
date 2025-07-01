using EnglishManager.Application.DTOs;

namespace EnglishManager.Application.Interfaces
{
    public interface ITodoService
    {
        Task<IEnumerable<TodoDto>> GetAllTodosAsync();
        Task<TodoDto?> GetTodoByIdAsync(int id);
        Task<TodoDto> CreateTodoAsync(CreateTodoDto createTodoDto);
        Task UpdateTodoAsync(int id, UpdateTodoDto updateTodoDto);
        Task DeleteTodoAsync(int id);
    }
}