using Microsoft.EntityFrameworkCore;
using MyForeignCards.Data;
using MyForeignCards.Entities;

namespace MyForeignCards.Services
{
    public class WordService
    {
        private readonly ILogger<WordService> _logger;
        private readonly ApplicationContext _context;

        public WordService(ILogger<WordService> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        public Task<List<Word>> GetAllWords()
        {
            return _context.Words.ToListAsync();
        } 

        public async void AddWordAsync(Word newWord)
        {
            await _context.AddAsync(newWord);

            _context.SaveChanges();

            _logger.LogInformation("Text {WordId} added",
                newWord.Id);

            _logger.LogDebug("Text {TextId} added. Text: {Text}",
                newWord.Id,
                newWord.Text);
        }

        public async Task<Word?> GetWordByIdAsync(Guid id)
        {
            var result = await _context.Words.FirstOrDefaultAsync(word => word.Id == id);

            if (result == null) 
            {
                _logger.LogDebug("Get text {TextId} failed: text was not found", id);
            }

            return result;
        }

        public bool DeleteWordById(Guid id)
        {
            var word = _context.Words.FirstOrDefaultAsync(word => word.Id == id).Result;
            if (word != null)
            {
                var result = _context.Words.Remove(word);

                _context.SaveChanges();

                _logger.LogInformation("Text {TextId} deleted",
                    word.Id);

                _logger.LogDebug("Text {TextId} deleted. Text: {Text}",
                    word.Id,
                    word.Text);

                return true;
            }
            else
            {
                _logger.LogWarning("Text {TextId} delete failed: text was not found", id);

                return false;
            }
        }

        public bool ChangeWord(Guid id, Word newWord)
        {
            var word = _context.Words.FirstOrDefaultAsync(word => word.Id == id).Result;

            if (word != null)
            {
                word.Text = newWord.Text;
                word.Translation = newWord.Translation;

                _context.SaveChanges();

                _logger.LogInformation("Text {TextId} updated",
                    word.Id);

                _logger.LogDebug("Text {TextId} updated. Text: {Text}",
                    word.Id,
                    word.Text);

                return true;
            }
            else
            {
                _logger.LogWarning("Text {TextId} update failed: text was not found", id);

                return false;
            }
        }
    }
}
