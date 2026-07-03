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
                var word = wordService.GetWordById(id);

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

            app.MapPost("/api/words", async (HttpResponse response, WordModel newWord, WordService wordService) =>
            {
                if (newWord is not null &&
                    !string.IsNullOrWhiteSpace(newWord.Word) &&
                    !string.IsNullOrWhiteSpace(newWord.Translation))
                {
                    wordService.AddWord(newWord);
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
                var result = wordService.DeleteWordById(id);

                if (result)
                {
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
                    var result = wordService.ChangeWord(id, word);

                    if (result)
                    {
                        await response.WriteAsJsonAsync(word);
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
