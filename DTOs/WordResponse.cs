namespace MyForeignCards.DTOs
{
    public class WordResponse
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = null!;
        public string Translation { get; set; } = null!;
        public Guid? CategoryId { get; set; }
    }
}
