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

        public Task<List<Word>> GetAllWordsAsync()
        {
            return _context.Words.ToListAsync();
        } 

        public async Task<Word> AddWordAsync(Word newWord)
        {
            var word = _context.Words.Add(newWord);

            await _context.SaveChangesAsync();

            _logger.LogInformation("Word {WordId} added",
                newWord.Id);

            _logger.LogDebug("Word {WordId} added. Word: {Text}",
                newWord.Id,
                newWord.Text);

            return word.Entity;
        }

        public async Task<Word?> GetWordByIdAsync(Guid id)
        {
            var result = await _context.Words.FindAsync(id);

            if (result == null) 
            {
                _logger.LogDebug("Get word {WordId} failed: word was not found", id);
            }

            return result;
        }

        public async Task<bool> DeleteWordByIdAsync(Guid id)
        {
            var word = await _context.Words.FindAsync(id);

            if (word != null)
            {
                _context.Words.Remove(word);

                await _context.SaveChangesAsync();

                _logger.LogInformation("Word {WordId} deleted",
                    word.Id);

                _logger.LogDebug("Word {WordId} deleted. Word: {Text}",
                    word.Id,
                    word.Text);

                return true;
            }
            else
            {
                _logger.LogWarning("Word {WordId} delete failed: word was not found", id);

                return false;
            }
        }

        public async Task<bool> ChangeWordAsync(Guid id, Word newWord)
        {
            var word = await _context.Words.FindAsync(id);

            if (word != null)
            {
                word.Text = newWord.Text;
                word.Translation = newWord.Translation;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Word {WordId} updated",
                    word.Id);

                _logger.LogDebug("Word {WordId} updated. Word: {Text}",
                    word.Id,
                    word.Text);

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
