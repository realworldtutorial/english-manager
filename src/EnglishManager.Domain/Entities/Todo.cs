namespace EnglishManager.Domain.Entities
{
    public class Todo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }

        // Thêm liên kết với User
        public int UserId { get; set; }
        public User User { get; set; }
    }
}