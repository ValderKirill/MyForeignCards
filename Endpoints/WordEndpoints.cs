using MyForeignCards.Models;
using MyForeignCards.Utils;

namespace MyForeignCards.Endpoints
{
    public static class WordEndpoints
    {
        public static void MapWordEndpoints(this WebApplication app, List<WordModel> words)
        {
            app.MapGet("/api/words", async (HttpResponse response) =>
            {
                await DictionaryUtils.GetAllWords(response, words);
            });

            app.MapGet("/api/words/{id}", async (Guid id, HttpResponse response) =>
            {
                await DictionaryUtils.GetWord(id, response, words);
            });

            app.MapPost("/api/words", async (HttpResponse response, HttpRequest request) =>
            {
                await DictionaryUtils.AddWord(request, response, words);
            });

            app.MapDelete("/api/words/{id}", async (Guid id, HttpResponse response) =>
            {
                await DictionaryUtils.DeleteWord(id, response, words);
            });

            app.MapPut("/api/words", async (HttpResponse response, HttpRequest request) =>
            {
                await DictionaryUtils.EditWord(request, response, words);
            });
        }
    }
}
