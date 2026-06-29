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
    var expressionForGuid = @"^/api/words/\w{8}-\w{4}-\w{4}-\w{4}-\w{12}$";

    if (request.Path == "/api/words" && request.Method == "GET")
	{
		await DictionaryUtils.GetAllWords(response, words);
        return;
    }
    else if (Regex.IsMatch(request.Path, expressionForGuid) && request.Method == "GET")
    {
        await DictionaryUtils.GetWord(request, response, words);
        return;
    }
    else if (request.Path == "/api/words" && request.Method == "POST")
	{
        await DictionaryUtils.AddWord(request, response, words);
        return;
    }
    else if (Regex.IsMatch(request.Path, expressionForGuid) && request.Method == "DELETE")
    {
        await DictionaryUtils.DeleteWord(request, response, words);
        return;
    }
    else if (request.Path == "/api/words" && request.Method == "PUT")
    {
        await DictionaryUtils.EditWord(request, response, words);
        return;
    }
});

app.Run();