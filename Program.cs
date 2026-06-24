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

app.Run(async (context) =>
{ 
	var request = context.Request;
	var response = context.Response;
    var expressionForGuid = @"^/word/\w{8}-\w{4}-\w{4}-\w{4}-\w{12}$";

    if (request.Path == "/words" && request.Method == "GET")
	{
		await DictionaryUtils.GetAllWords(response, words);
    }
    else if (Regex.IsMatch(request.Path, expressionForGuid))
    {
        await DictionaryUtils.GetWord(request, response, words);
    }
    else if (request.Path == "/add")
	{
        await DictionaryUtils.AddWord(request, response, words);
    }
    else if (request.Path == "/delete")
    {
        await DictionaryUtils.DeleteWord(request, response, words);
    }
    else if (request.Path == "/edit")
    {
        await DictionaryUtils.EditWord(request, response, words);
    }
});

app.Run();