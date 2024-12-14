using System.ComponentModel;
using TinyUrl.DTOs;
using TinyUrl.Services;
using TinyUrl.Exceptions;

namespace TinyUrl.Services;

public class DisplayService : IDisplayService
{
    private readonly IUrlMappingService _urlMappingService;
    private readonly IUrlGenerationService _urlGenerationService;

    public DisplayService(
        IUrlMappingService urlMappingService,
        IUrlGenerationService urlGenerationService
    )
    {
        _urlMappingService = urlMappingService;
        _urlGenerationService = urlGenerationService;
    }

    public void Start()
    {
        GetOptions();
        var option = GetUserOption();
        EvaluateDecision(option);
    }

    public void GetOptions()
    {
        foreach (Option option in Enum.GetValues(typeof(Option)))
        {
            var description = option.GetType()
                .GetField(option.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .FirstOrDefault() as DescriptionAttribute;

            string displayText = description?.Description ?? option.ToString();
            Console.WriteLine($"{(int)option}: {displayText}");
        }
    }

    public Option GetUserOption()
    {
        while (true)
        {
            Console.WriteLine("Enter your choice: ");
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int choice) && 
                Enum.IsDefined(typeof(Option), choice))
            {
                return (Option)choice;
            }

            Console.WriteLine("Invalid choice, please try again.");
        }
    }

    public void EvaluateDecision(Option choice)
    {
        switch (choice)
        {
            case Option.Create:
                PerformCreateOperation();
                break;
            case Option.Read:
                PerformReadOperation();
                break;
            case Option.Delete:
                PerformDeleteOperation();
                break;
            case Option.Statistics:
                PerformStatisticsOperation();
                break;
            default:
                Console.WriteLine("Invalid choice, please try again.");
                break;
        }
        Start();
    }

    private void PerformCreateOperation()
    {
        while (true)
        {
            try
            {
                Console.WriteLine("Enter the long URL you want to create a tiny URL for:");
                string? longUrl = Console.ReadLine();
                
                if (string.IsNullOrWhiteSpace(longUrl))
                {
                    Console.WriteLine("URL cannot be empty. Please try again.");
                    continue;
                }

                Console.WriteLine("If you want to specify a custom tiny URL, enter it here. Otherwise, press enter to generate a random one.");
                var customTinyUrl = Console.ReadLine();

                var tinyUrl = _urlMappingService.CreateUrl(longUrl, string.IsNullOrWhiteSpace(customTinyUrl) ? null : customTinyUrl);
                Console.WriteLine($"\n The tiny URL for {longUrl} is {tinyUrl} \n");
                return;
            }
            catch (UrlGenerationException e)
            {
                Console.WriteLine($"Error: {e.Message}");
                Console.WriteLine("Please try again.");
            }
        }
    }

    private void PerformReadOperation()
    {
        while (true)
        {
            try
            {
                Console.WriteLine("What tiny URL do you want to get the long URL from?");
                var tinyUrl = Console.ReadLine();
                var longUrl = _urlMappingService.GetLongUrl(tinyUrl);
                Console.WriteLine($"\n The long URL for {tinyUrl} is {longUrl} \n");
                return;
            }
            catch (UrlMappingException e)
            {
                Console.WriteLine($"Error: {e.Message}");
                Console.WriteLine("Please try again.");
            }
        }
    }

    private void PerformDeleteOperation()
    {
        while (true)
        {
            try
            {
                Console.WriteLine("What tiny URL do you want to delete?");
                var deleteTinyUrl = Console.ReadLine();
                _urlMappingService.DeleteUrl(deleteTinyUrl);
                Console.WriteLine("\n Tiny URL has been deleted successfully. \n");
                return;
            }
            catch (UrlMappingException e)
            {
                Console.WriteLine($"Error: {e.Message}");
                Console.WriteLine("Please try again.");
            }
        }
    }

    private void PerformStatisticsOperation()
    {
        while (true)
        {
            try
            {
                Console.WriteLine("What tiny URL do you want to get statistics on?");
                var statsTinyUrl = Console.ReadLine();
                var frequency = _urlMappingService.GetHitCount(statsTinyUrl);
                Console.WriteLine($"\n The tiny URL {statsTinyUrl} has been clicked {frequency} times. \n");
                return;
            }
            catch (UrlMappingException e)
            {
                Console.WriteLine($"Error: {e.Message}");
                Console.WriteLine("Please try again.");
            }
        }
    }
}