using Microsoft.EntityFrameworkCore;
using MyForeignCards.Data;
using MyForeignCards.DTOs;
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

        public Task<List<WordResponse>> GetAllWordsAsync()
        {
            return _context.Words
                .AsNoTracking()
                .Select(word => new WordResponse
                {
                    Id = word.Id,
                    Text = word.Text,
                    Translation = word.Translation,
                    CategoryId = word.CategoryId
                })
                .ToListAsync();
        }

        public Task<List<WordResponse>> GetWordsByCategoryAsync(Guid categoryId)
        {
            return _context.Words
                .Where(word => word.CategoryId == categoryId)
                .AsNoTracking()
                .Select(word => new WordResponse
                {
                    Id = word.Id,
                    Text = word.Text,
                    Translation = word.Translation,
                    CategoryId = word.CategoryId
                })
                .ToListAsync();
        }

        public async Task<WordResponse> AddWordAsync(Word newWord)
        {
            _context.Words.Add(newWord);

            await _context.SaveChangesAsync();

            if (newWord.CategoryId != null)
            {
                await _context.Entry(newWord).Reference(w => w.Category).LoadAsync();
            }

            _logger.LogInformation("Word {WordId} added",
                newWord.Id);

            _logger.LogDebug("Word {WordId} added. Word: {Text}, category: {CategoryName}",
                newWord.Id,
                newWord.Text,
                newWord.Category?.Name ?? "No category");

            var wordResponse = new WordResponse
            {
                Id = newWord.Id,
                Text = newWord.Text,
                Translation = newWord.Translation,
                CategoryId = newWord.CategoryId
            };

            return wordResponse;
        }

        public async Task<WordResponse?> GetWordByIdAsync(Guid id)
        {
            var word = await _context.Words.FindAsync(id);

            if (word == null) 
            {
                _logger.LogDebug("Get word {WordId} failed: word was not found", id);
                return null;
            }

            return new WordResponse
            {
                Id = word.Id,
                Text = word.Text,
                Translation = word.Translation,
                CategoryId = word.CategoryId
            };
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
                word.CategoryId = newWord.CategoryId;

                await _context.SaveChangesAsync();

                if (newWord.CategoryId != null)
                {
                    await _context.Entry(word).Reference(w => w.Category).LoadAsync();
                }

                _logger.LogInformation("Word {WordId} updated",
                    word.Id);

                _logger.LogDebug("Word {WordId} updated. Word: {Text}, Category: {Category}",
                    word.Id,
                    word.Text,
                    word.Category?.Name ?? "No category");

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
