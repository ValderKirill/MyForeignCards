using MyForeignCards.Endpoints;
using MyForeignCards.Models;
using MyForeignCards.Utils;
using System.Text.RegularExpressions;

List<WordModel> words = new List<WordModel>()
{
    new WordModel("Test", "Тест")
};
words.Add(new WordModel("Test", "Тест"));

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapWordEndpoints(words);

app.Run();