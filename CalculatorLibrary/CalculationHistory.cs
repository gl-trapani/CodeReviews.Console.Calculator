using Newtonsoft.Json;

namespace CalculatorLibrary;

public class CalculationHistory
{
    public List<CalculationInfo> Operations { get; set; } = new List<CalculationInfo>();

    private Calculator calculator;

    public CalculationHistory(Calculator calculator)
    {
        this.calculator = calculator;
    }

    private CalculationHistory? LoadJson()
    {
        using StreamReader r = new StreamReader("calculatorlog.json");
        var json = r.ReadToEnd();
        // close if array still open
        if (json[json.Length - 1] != ']')
            json = json + "]}";

        return JsonConvert.DeserializeObject<CalculationHistory>(json);
    }

    public bool CheckHistory()
    {
        CalculationHistory? history = LoadJson();

        if (history != null && history.Operations.Count != 0)
            return true;

        return false;
    }

    public void DisplayHistory()
    {
        CalculationHistory? history = LoadJson();

        if (history != null)
        {
            Console.WriteLine("History:\r");
            Console.WriteLine("------------------------\n");

            foreach (var op in history.Operations)
            {
                Console.WriteLine($"Id: {op.Id}, Operation: {op.Operation}, Result: {op.Result}");
                Console.WriteLine();
            }
        }
    }

    public void HistoryOptions()
    {
        Console.WriteLine("1: Clear history and return to main menu");
        Console.WriteLine("2: Return to main menu");

        string? userInput = Console.ReadLine();
        int optionSelection = 0;
        while (!int.TryParse(userInput, out optionSelection))
        {
            Console.Write("This is not valid input. Please enter a numeric value: ");
            userInput = Console.ReadLine();
        }

        switch (optionSelection)
        {
            case 1:
                ClearHistory(calculator);
                break;
            case 2:
                break;
        }
    }

    public static void ClearHistory(Calculator calculator)
    {
        Console.Write("Clear History? (Y/N)");
        ConsoleKeyInfo keyInfo = Console.ReadKey();
        if (keyInfo.KeyChar == 'y' || keyInfo.KeyChar == 'Y')
        {
            calculator.StartWriter();
            calculator.ResetIdCounter();
        }
    }

    public double GetNumberFromHistory()
    {
        double result = 0;
        CalculationHistory? history = LoadJson();
        DisplayHistory();

        Console.Write("Enter Id to use:");
        string userInput = Console.ReadLine();
        int optionSelection = 0;

        while (!int.TryParse(userInput, out optionSelection) || optionSelection < 0 ||
               optionSelection > history.Operations.Count)
        {
            Console.Write("Input not valid. Please enter correct Id: ");
            userInput = Console.ReadLine();
        }


        if (history != null)
        {
            foreach (var op in history.Operations)
            {
                if (optionSelection == op.Id)
                {
                    result = op.Result;
                }
            }
        }

        return result;
    }
}