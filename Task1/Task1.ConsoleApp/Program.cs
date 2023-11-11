using Task1.ConsoleApp;
using Task1.ConsoleApp.Data;

var dataProvider = new DataProvider();
var searcher = new TypedSearcher(dataProvider, 10);

Parallel.ForEach(dataProvider.UserTypedTextList, searcher.SearchMatchedWords);

//Parallel.ForEach(dataProvider.UserTypedTextList, searcher.SearchWithBinaryMatchedWords);

Console.ReadLine();
