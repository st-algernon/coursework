using Coursework.Core.Queries;
using MediatR;

namespace Coursework.Console;

public class HomePage
{
    private readonly IMediator _mediator;
    
    public HomePage(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task Main()
    {
        System.Console.WriteLine("------ Home Page ------");
        
        var choice = -1;
        
        while (choice != 0)
        {
            System.Console.WriteLine("Please choose one of the following options:");
            System.Console.WriteLine("1. Get the largest collection");
            System.Console.WriteLine("2. Get the last added items");
            System.Console.WriteLine("3. Get the top tags");
            System.Console.WriteLine("4. Search items by tag");
            System.Console.WriteLine("0. Exit");

            try
            {
                choice = int.Parse(System.Console.ReadLine() ?? throw new FormatException());
            }
            catch (FormatException)
            {
                choice = -1;
            }

            switch (choice)
            {
                case 0:
                    System.Console.WriteLine("Bye...");
                    break;
                case 1:
                    var collections = await _mediator.Send(new GetLargestCollectionsQuery());

                    PrintObjectsToConsole(collections);
                    break;
                case 2:
                    var items = await _mediator.Send(new GetLastAddedItemsQuery());
                    
                    PrintObjectsToConsole(items);
                    break;
                case 3:
                    var tags = await _mediator.Send(new GetTopTagsQuery());
                    
                    PrintObjectsToConsole(tags);
                    break;
                case 4:
                    System.Console.Write("Enter a query: ");
                    var query = System.Console.ReadLine();
                    var searchItemsQuery = new SearchItemsQuery
                    {
                        Query = query ?? string.Empty,
                        SearchBy = SearchBy.Tag
                    };
                    var foundItems = await _mediator.Send(searchItemsQuery);
                    
                    PrintObjectsToConsole(foundItems);
                    break;
                default:
                    System.Console.WriteLine("Invalid choice! Please try again.");
                    break;
            }
        }
    }

    private void PrintObjectsToConsole<T>(IEnumerable<T> objs)
    {
        foreach (var obj in objs)
        {
            PrintObjectToConsole(obj);
        }
    }

    private void PrintObjectToConsole<T>(T obj)
    {
        foreach(var prop in typeof(T).GetProperties())
        {
            System.Console.WriteLine(prop.Name + " : " + prop.GetValue(obj));
        }
        
        System.Console.WriteLine();
    }
}