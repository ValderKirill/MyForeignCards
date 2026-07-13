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
        private readonly ILogger<WordService> _logger;

        public WordService(ILogger<WordService> logger)
        {
            _logger = logger;
        }

        public IReadOnlyCollection<WordModel> Words()
        {
            return _words.ToList();
        } 

        public void AddWord(WordModel newWord)
        {
            _words.Add(newWord);

            _logger.LogInformation("Word {WordId} added",
                newWord.Id);

            _logger.LogDebug("Word {WordId} added. Text {Word}",
                newWord.Id,
                newWord.Word);
        }

        public WordModel? GetWordById(Guid id)
        {
            var result = _words.FirstOrDefault(word => word.Id == id);

            if (result == null) 
            {
                _logger.LogDebug("Get word {WordId} failed: word was not found", id);
            }

            return result;
        }

        public bool DeleteWordById(Guid id)
        {
            var word = _words.FirstOrDefault(word => word.Id == id);
            if (word != null)
            {
                var result = _words.Remove(word);

                _logger.LogInformation("Word {WordId} deleted",
                    word.Id);

                _logger.LogDebug("Word {WordId} deleted. Text {Word}",
                    word.Id,
                    word.Word);

                return result;
            }
            else
            {
                _logger.LogWarning("Word {WordId} delete failed: word was not found", id);

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

                _logger.LogInformation("Word {WordId} updated",
                    word.Id);

                _logger.LogDebug("Word {WordId} updeted. Text {Word}",
                    word.Id,
                    word.Word);

                return true;
            }
            else
            {
                _logger.LogWarning("Word {WordId} update failed: word was not found", id);

                return false;
            }
        }
    }
}
