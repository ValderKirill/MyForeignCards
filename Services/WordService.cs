using MyForeignCards.Models;

namespace MyForeignCards.Services
{
    public class WordService
    {
        private readonly List<WordModel> _words = new List<WordModel>()
        {
            new WordModel("Test", "Тест"),
            new WordModel("Apple", "Яблоко")
        };

        public List<WordModel> GetWords => _words;
    }
}
