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
            var word = words.FirstOrDefault(word => word.Id == id);

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
            //https://localhost:7258/add?word=aboba&translation=абоба
            words.Add(new WordModel(request.Query["word"], request.Query["translation"]));
            response.Redirect("/");
        }
    }
}
