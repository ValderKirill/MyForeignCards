namespace MyForeignCards.Models
{
    public class WordModel
    {
        public Guid Id { get; set; }
        public string Word { get; set; }
        public string Translation { get; set; }

        public WordModel(string word, string translation)
        {
            Id = Guid.NewGuid();
            Word = word;
            Translation = translation;
        }
    }
}
