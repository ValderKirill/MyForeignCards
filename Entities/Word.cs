namespace MyForeignCards.Entities
{
    public class Word
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = null!;
        public string Translation { get; set; } = null!;
    }
}
