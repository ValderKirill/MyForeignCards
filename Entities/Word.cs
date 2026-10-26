namespace MyForeignCards.Entities
{
    public class Word
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = null!;
        public string Translation { get; set; } = null!;

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public Guid? CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
