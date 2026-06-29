using MyForeignCards.Models;
using System.Transactions;

namespace MyForeignCards.Utils
{
    public static class DictionaryUtils
    {
        public static async Task GetAllWords(HttpResponse response, List<WordModel> words)
        {
            await response.WriteAsJsonAsync(words);
        }

        public static async Task GetWord(HttpRequest request, HttpResponse response, List<WordModel> words)
        {
            var wordId = request.Path.Value?.Split("/")[3];
            var word = words.FirstOrDefault(word => word.Id.ToString() == wordId);

            if (word != null)
            {
                await response.WriteAsJsonAsync(word);
            }
            else
            {
                response.StatusCode = 404;
                await response.WriteAsync("Не нашли искомое слово");
            }
        }

        public static async Task AddWord(HttpRequest request, HttpResponse response, List<WordModel> words)
        {
            //var word = request.Query["word"];
            //var translation = request.Query["translation"];
            //https://localhost:7258/add?word=aboba&translation=абоба

            var newWord = await request.ReadFromJsonAsync<WordModel>();

            if (newWord is not null &&
                !string.IsNullOrWhiteSpace(newWord.Word) &&
                !string.IsNullOrWhiteSpace(newWord.Translation))
            {
                words.Add(newWord);
                await response.WriteAsJsonAsync(newWord);
            }
            else
            {
                response.StatusCode = 400;
                await response.WriteAsJsonAsync(new { message = "Не смогли добавить пустое слово!" });
            }
        }

        public static async Task DeleteWord(HttpRequest request, HttpResponse response, List<WordModel> words)
        {
            var wordId = request.Path.Value?.Split("/")[3];
            var word = words.FirstOrDefault(word => word.Id.ToString() == wordId);

            if (!string.IsNullOrWhiteSpace(wordId) &&
                word != null)
            {
                words.Remove(word);
                response.StatusCode = 204;
            }
            else
            {
                response.StatusCode = 404;
                await response.WriteAsync("Не нашли слово с нужным ID!");
            }
        }

        public static async Task EditWord(HttpRequest request, HttpResponse response, List<WordModel> words)
        {
            WordModel? reqWord = await request.ReadFromJsonAsync<WordModel>();

            if (reqWord != null)
            {
                var neededWord = words.FirstOrDefault(word => word.Id == reqWord.Id);

                if (neededWord != null)
                {
                    neededWord.Word = reqWord.Word;
                    neededWord.Translation = reqWord.Translation;
                    await response.WriteAsJsonAsync(neededWord);
                }
                else
                {
                    response.StatusCode = 404;
                    await response.WriteAsync("Не смогли добавить пустое слово!");
                }
            }
            else
            {
                response.StatusCode = 400;
                await response.WriteAsync("Пустые входные данные");
            }
        }
    }
}
