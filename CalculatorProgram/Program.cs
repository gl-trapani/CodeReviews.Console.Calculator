using System.Text.RegularExpressions;
using CalculatorLibrary;

class Program
{
    static void Main(string[] args)
    {
        bool endApp = false;

        Calculator calculator = new Calculator();
        CalculationHistory calculatorHistory = new CalculationHistory(calculator);

        while (!endApp)
        {
            Console.Clear();
            
            // Display title as the C# console calculator app.
            Console.WriteLine("Main menu:\r");
            Console.WriteLine("-------------------------\n");
            // Declare variables and set to empty.
            // Use Nullable types (with ?) to match type of System.Console.ReadLine
            string? numInput1 = "";
            string? numInput2 = "";
            string? userInput = "";
            double result = 0;
            Console.WriteLine($"Counter: {calculator.Counter}");

            // Use Calculator or View History
            Console.WriteLine("Choose an option from the following list:");
            Console.WriteLine("1: Calculator");
            Console.WriteLine("2: History");
            Console.WriteLine("3: Exit");

            userInput = Console.ReadLine();
            int optionSelection = 0;
            while (!int.TryParse(userInput, out optionSelection))
            {
                Console.Write("This is not valid input. Please enter a numeric value: ");
                userInput = Console.ReadLine();
            }

            switch (optionSelection)
            {
                case 1:
                    break;
                case 2:
                    if (calculatorHistory.CheckHistory())
                    {
                        Console.Clear();
                        calculatorHistory.DisplayHistory();
                        calculatorHistory.HistoryOptions();
                    }
                    else
                    {
                        Console.WriteLine("History is empty.");
                        Thread.Sleep(1000);
                    }
                    continue;
                case 3:
                    endApp = true;
                    continue;
                default:
                    continue;
            }

            // Ask the user to type the first number.
            Console.WriteLine("First Number:");
            double cleanNum1 = calculator.GetNumber(calculatorHistory);
            Console.WriteLine("Second Number:");
            double cleanNum2 = calculator.GetNumber(calculatorHistory);

            // Ask the user to choose an operator.
            Console.WriteLine("Choose an operator from the following list:");
            Console.WriteLine("\ta - Add");
            Console.WriteLine("\ts - Subtract");
            Console.WriteLine("\tm - Multiply");
            Console.WriteLine("\td - Divide");
            Console.Write("Your option? ");

            string? op = Console.ReadLine();

            // Validate input is not null, and matches the pattern
            if (op == null || !Regex.IsMatch(op, "[a|s|m|d]"))
            {
                Console.WriteLine("Error: Unrecognized input.");
            }
            else
            {
                try
                {
                    result = calculator.DoOperation(cleanNum1, cleanNum2, op);
                    if (double.IsNaN(result))
                    {
                        Console.WriteLine("This operation will result in a mathematical error.\n");
                    }
                    else Console.WriteLine("Your result: {0:0.##}\n", result);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Oh no! An exception occurred trying to do the math.\n - Details: " + e.Message);
                }
            }

            Console.WriteLine("------------------------\n");

            // Wait for the user to respond before closing.
            Console.Write("Press 'n' and Enter to close the app, or press any other key and Enter to continue: ");
            if (Console.ReadLine() == "n") endApp = true;

            Console.WriteLine("\n"); // Friendly linespacing.
        }

        calculator.Finish();
        return;
    }
}