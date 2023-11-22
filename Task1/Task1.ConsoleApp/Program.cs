using Task1.ConsoleApp;
using Task1.ConsoleApp.Data;
using Task1.ConsoleApp.Impl;

var dataProvider = new DataProvider();
ITypedSearcher searcher = 
    //new DictionarySearcher(dataProvider);
new TreapSearcher(dataProvider);

foreach (var item in dataProvider.UserTypedTextList)
{
    searcher.Search(item);
}

Console.ReadLine();
