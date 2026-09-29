namespace MyForeignCards.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public List<Word> Words { get; set; } = [];
    }
}
