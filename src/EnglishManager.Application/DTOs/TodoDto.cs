namespace EnglishManager.Application.DTOs
{
    public class TodoDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int UserId { get; set; }
    }

    public class CreateTodoDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class UpdateTodoDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}