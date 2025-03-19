using EnglishManager.Application.DTOs;
using EnglishManager.Application.Interfaces;
using EnglishManager.Domain.Entities;
using EnglishManager.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace EnglishManager.Application.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _todoRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TodoService(ITodoRepository todoRepository, IHttpContextAccessor httpContextAccessor)
        {
            _todoRepository = todoRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User not authenticated");

            return int.Parse(userIdClaim.Value);
        }

        public async Task<IEnumerable<TodoDto>> GetAllTodosAsync()
        {
            var userId = GetCurrentUserId();
            var todos = await _todoRepository.GetAllAsync();
            return todos.Where(t => t.UserId == userId).Select(todo => MapToDto(todo));
        }

        public async Task<TodoDto> GetTodoByIdAsync(int id)
        {
            var userId = GetCurrentUserId();
            var todo = await _todoRepository.GetByIdAsync(id);

            if (todo == null || todo.UserId != userId)
                return null;

            return MapToDto(todo);
        }

        public async Task<TodoDto> CreateTodoAsync(CreateTodoDto createTodoDto)
        {
            var userId = GetCurrentUserId();
            var todo = new Todo
            {
                Title = createTodoDto.Title,
                Description = createTodoDto.Description,
                IsCompleted = false,
                CreatedDate = DateTime.UtcNow,
                UserId = userId
            };

            var createdTodo = await _todoRepository.AddAsync(todo);
            return MapToDto(createdTodo);
        }

        public async Task UpdateTodoAsync(int id, UpdateTodoDto updateTodoDto)
        {
            var userId = GetCurrentUserId();
            var todo = await _todoRepository.GetByIdAsync(id);

            if (todo == null || todo.UserId != userId)
                throw new UnauthorizedAccessException("Not authorized to update this todo");

            todo.Title = updateTodoDto.Title;
            todo.Description = updateTodoDto.Description;
            todo.IsCompleted = updateTodoDto.IsCompleted;

            if (updateTodoDto.IsCompleted && !todo.CompletedDate.HasValue)
                todo.CompletedDate = DateTime.UtcNow;
            else if (!updateTodoDto.IsCompleted)
                todo.CompletedDate = null;

            await _todoRepository.UpdateAsync(todo);
        }

        public async Task DeleteTodoAsync(int id)
        {
            var userId = GetCurrentUserId();
            var todo = await _todoRepository.GetByIdAsync(id);

            if (todo == null || todo.UserId != userId)
                throw new UnauthorizedAccessException("Not authorized to delete this todo");

            await _todoRepository.DeleteAsync(id);
        }

        private static TodoDto MapToDto(Todo todo)
        {
            return new TodoDto
            {
                Id = todo.Id,
                Title = todo.Title,
                Description = todo.Description,
                IsCompleted = todo.IsCompleted,
                CreatedDate = todo.CreatedDate,
                CompletedDate = todo.CompletedDate,
                UserId = todo.UserId
            };
        }
    }
}