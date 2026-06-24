using MyForeignCards.Models;

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
            var id = request.Path.Value?.Split("/")[2];
            var word = words.FirstOrDefault(word => word.Id .ToString() == id);

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
            var word = request.Query["word"];
            var translation = request.Query["translation"];
            //https://localhost:7258/add?word=aboba&translation=абоба
            if (!string.IsNullOrWhiteSpace(word) &&
                !string.IsNullOrWhiteSpace(translation))
            {
                words.Add(new WordModel(word, translation));
                response.Redirect("/");
            }
            else
            {
                await response.WriteAsync("Не смогли добавить пустое слово!");
            }
        }

        public static async Task DeleteWord(HttpRequest request, HttpResponse response, List<WordModel> words)
        {
            var wordId = request.Query["id"];
            var word = words.FirstOrDefault(w => w.Id.ToString() == wordId);

            if (!string.IsNullOrWhiteSpace(wordId) &&
                word != null)
            {
                words.Remove(word);
                response.Redirect("/");
            }
            else
            {
                response.StatusCode = 404;
                await response.WriteAsync("Не нашли слово с нужным ID!");
            }
        }

        public static async Task EditWord(HttpRequest request, HttpResponse response, List<WordModel> words)
        {
            var wordId = request.Query["id"];
            var neededWord = words.FirstOrDefault(w => w.Id.ToString() == wordId);

            if (neededWord == null)
            {
                response.StatusCode = 404;
                await response.WriteAsync("Не нашли изменяемое слово!");
            }

            var word = request.Query["word"];
            var translation = request.Query["translation"];

            if (!string.IsNullOrWhiteSpace(word) &&
                !string.IsNullOrWhiteSpace(translation))
            {
                neededWord.Word = word;
                neededWord.Translation = translation;
                response.Redirect("/");
            }
            else
            {
                await response.WriteAsync("Не смогли добавить пустое слово!");
            }
        }
    }
}
