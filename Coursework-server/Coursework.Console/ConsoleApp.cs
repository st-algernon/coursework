using Coursework.Core.Queries;
using MediatR;

namespace Coursework.Console;

public class ConsoleApp
{
    private readonly IMediator _mediator;
    
    public ConsoleApp(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task Main()
    {
        System.Console.WriteLine("------ Hello ------");
        
        var choice = -1;
        
        while (choice != 0)
        {
            System.Console.WriteLine("Please choose one of the following options:");
            System.Console.WriteLine("1. Get the largest collection");
            System.Console.WriteLine("2. Get the last added items");
            System.Console.WriteLine("3. Get the top tags");
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
                    var result = await _mediator.Send(new GetLargestCollectionsQuery());

                    foreach (var collection in result)
                    {
                        PrintObjectToConsole(collection);
                    }
                    break;
                case 2:
                    await _mediator.Send(new GetLastAddedItemsQuery());
                    break;
                case 3:
                    await _mediator.Send(new GetTopTagsQuery());
                    break;
                default:
                    System.Console.WriteLine("Invalid choice! Please try again.");
                    break;
            }
        }
    }

    private void PrintObjectToConsole<T>(T obj)
    {
        foreach(var prop in typeof(T).GetProperties())
        {
            System.Console.WriteLine(prop.Name, prop.GetValue(obj));
        }
    }
}