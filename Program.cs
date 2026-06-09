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

app.Run(async (context) =>
{ 
	var request = context.Request;
	var responce = context.Response;
    var expressionForGuid = @"^/word/\w{8}-\w{4}-\w{4}-\w{4}-\w{12}$";

    if (request.Path == "/" && request.Method == "GET")
	{
		await DictionaryUtils.GetAllWords(responce, words);
    }
    else if (Regex.IsMatch(request.Path, expressionForGuid))
    {
        await DictionaryUtils.GetWord(request, responce, words);
    }
    else if (request.Path == "/add")
	{
        await DictionaryUtils.AddWord(request, responce, words);
    }
});

app.Run();