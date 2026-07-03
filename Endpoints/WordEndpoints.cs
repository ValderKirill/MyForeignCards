using MyForeignCards.Models;
using MyForeignCards.Services;

namespace MyForeignCards.Endpoints
{
    public static class WordEndpoints
    {
        public static void MapWordEndpoints(this WebApplication app)
        {
            app.MapGet("/api/words", async (HttpResponse response, WordService wordService) =>
            {
                await response.WriteAsJsonAsync(wordService.GetWords);
            });

            app.MapGet("/api/words/{id:guid}", async (Guid id, HttpResponse response, WordService wordService) =>
            {
                var word = wordService.GetWords.FirstOrDefault(word => word.Id == id);

                if (word != null)
                {
                    await response.WriteAsJsonAsync(word);
                }
                else
                {
                    response.StatusCode = 404;
                    await response.WriteAsJsonAsync(new { message = "Не нашли искомое слово" });
                }
            });

            app.MapPost("/api/words", async (HttpResponse response, HttpRequest request, WordService wordService) =>
            {
                var newWord = await request.ReadFromJsonAsync<WordModel>();

                if (newWord is not null &&
                    !string.IsNullOrWhiteSpace(newWord.Word) &&
                    !string.IsNullOrWhiteSpace(newWord.Translation))
                {
                    wordService.GetWords.Add(newWord);
                    await response.WriteAsJsonAsync(newWord);
                }
                else
                {
                    response.StatusCode = 400;
                    await response.WriteAsJsonAsync(new { message = "Не смогли добавить пустое слово!" });
                }
            });

            app.MapDelete("/api/words/{id:guid}", async (Guid id, HttpResponse response, WordService wordService) =>
            {
                var word = wordService.GetWords.FirstOrDefault(word => word.Id == id);

                if (word != null)
                {
                    wordService.GetWords.Remove(word);
                    response.StatusCode = 204;
                }
                else
                {
                    response.StatusCode = 404;
                    await response.WriteAsJsonAsync(new { message = "Не нашли слово с нужным ID!" });
                }
            });

            app.MapPut("/api/words/{id:guid}", async (Guid id, WordModel word, HttpResponse response, WordService wordService) =>
            {
                if (word != null)
                {
                    var neededWord = wordService.GetWords.FirstOrDefault(word => word.Id == id);

                    if (neededWord != null)
                    {
                        neededWord.Word = word.Word;
                        neededWord.Translation = word.Translation;
                        await response.WriteAsJsonAsync(neededWord);
                    }
                    else
                    {
                        response.StatusCode = 404;
                        await response.WriteAsJsonAsync(new { message = "Не найдено слово с указанным id" });
                    }
                }
                else
                {
                    response.StatusCode = 400;
                    await response.WriteAsJsonAsync(new { message = "Пустые входные данные" });
                }
            });
        }
    }
}
