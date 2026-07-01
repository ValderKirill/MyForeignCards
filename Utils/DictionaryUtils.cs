using MyForeignCards.Models;

namespace MyForeignCards.Utils
{
    public static class DictionaryUtils
    {
        public static async Task GetAllWords(HttpResponse response, List<WordModel> words)
        {
            await response.WriteAsJsonAsync(words);
        }

        public static async Task GetWord(Guid id, HttpResponse response, List<WordModel> words)
        {
            var word = words.FirstOrDefault(word => word.Id == id);

            if (word != null)
            {
                await response.WriteAsJsonAsync(word);
            }
            else
            {
                response.StatusCode = 404;
                await response.WriteAsJsonAsync(new { message = "Не нашли искомое слово" });
            }
        }

        public static async Task AddWord(HttpRequest request, HttpResponse response, List<WordModel> words)
        {
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

        public static async Task DeleteWord(Guid id, HttpResponse response, List<WordModel> words)
        {
            var word = words.FirstOrDefault(word => word.Id == id);

            if (word != null)
            {
                words.Remove(word);
                response.StatusCode = 204;
            }
            else
            {
                response.StatusCode = 404;
                await response.WriteAsJsonAsync(new { message = "Не нашли слово с нужным ID!" });
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
                    await response.WriteAsJsonAsync(new { message = "Не смогли добавить пустое слово!" });
                }
            }
            else
            {
                response.StatusCode = 400;
                await response.WriteAsJsonAsync(new { message = "Пустые входные данные" });
            }
        }
    }
}