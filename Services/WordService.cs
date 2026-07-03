using MyForeignCards.Models;
using System.Reflection.Metadata;
using System.Transactions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyForeignCards.Services
{
    public class WordService
    {
        private readonly List<WordModel> _words = new List<WordModel>()
        {
            new WordModel("Test", "Тест"),
            new WordModel("Apple", "Яблоко")
        };

        public IReadOnlyCollection<WordModel> GetWords => _words.ToList();

        public void AddWord(WordModel newWord)
        {
            _words.Add(newWord);
        }

        public WordModel? GetWordById(Guid id)
        {
            return _words.FirstOrDefault(word => word.Id == id);
        }

        public bool DeleteWordById(Guid id)
        {
            var word = _words.First(word => word.Id == id);
            if (word != null)
            {
                return _words.Remove(word);
            }
            else
            {
                return false;
            }
        }

        public bool ChangeWord(Guid id, WordModel newWord)
        {
            var word = _words.FirstOrDefault(word => word.Id == id);

            if (word != null) 
            {
                word.Word = newWord.Word;
                word.Translation = newWord.Translation;

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
