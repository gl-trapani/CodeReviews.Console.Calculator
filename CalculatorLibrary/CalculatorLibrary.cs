// CalculatorLibrary.cs

using System.Text.Json.Nodes;

namespace CalculatorLibrary;

using System.Diagnostics;
using Newtonsoft.Json;

public class Calculator
{
    private JsonWriter Writer { get; set; }

    public int Counter = 0;
    public int Id { get; set; } = 0;

    public Calculator()
    {
        StartWriter();
    }

    public void StartWriter()
    {
        StreamWriter logFile = File.CreateText("calculatorlog.json");
        logFile.AutoFlush = true;
        Writer = new JsonTextWriter(logFile);
        Writer.Formatting = Formatting.Indented;
        Writer.WriteStartObject();
        Writer.WritePropertyName("Operations");
        Writer.WriteStartArray();
    }

    public void ResetIdCounter()
    {
        Id = 0;
    }

    // CalculatorLibrary.cs
    public double DoOperation(double num1, double num2, string op)
    {
        Counter++;
        Id++;

        double
            result = double
                .NaN; // Default value is "not-a-number" if an operation, such as division, could result in an error.
        Writer.WriteStartObject();
        Writer.WritePropertyName("Id");
        Writer.WriteValue(Id.ToString());
        Writer.WritePropertyName("Operand1");
        Writer.WriteValue(num1);
        Writer.WritePropertyName("Operand2");
        Writer.WriteValue(num2);
        Writer.WritePropertyName("Operation");
        // Use a switch statement to do the math.
        switch (op)
        {
            case "a":
                result = num1 + num2;
                Writer.WriteValue("Add");
                break;
            case "s":
                result = num1 - num2;
                Writer.WriteValue("Subtract");
                break;
            case "m":
                result = num1 * num2;
                Writer.WriteValue("Multiply");
                break;
            case "d":
                // Ask the user to enter a non-zero divisor.
                if (num2 != 0)
                {
                    result = num1 / num2;
                }

                Writer.WriteValue("Divide");
                break;
            // Return text for an incorrect option entry.
            default:
                break;
        }

        Writer.WritePropertyName("Result");
        Writer.WriteValue(result);
        Writer.WriteEndObject();

        return result;
    }

    public void Finish()
    {
        Writer.WriteEndArray();
        Writer.WriteEndObject();
        Writer.Close();
    }

    public double GetNumber(CalculationHistory calculationHistory)
    {
        double cleanNum = 0;

        if (calculationHistory.CheckHistory())
        {
            Console.Write("Use past result? (Y/N)");
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            if (keyInfo.KeyChar == 'y' || keyInfo.KeyChar == 'Y')
            {
                cleanNum = calculationHistory.GetNumberFromHistory();
                return cleanNum;
            }
        }

        Console.Write("Type a number, and then press Enter: ");
        string? numInput1 = Console.ReadLine();

        while (!double.TryParse(numInput1, out cleanNum))
        {
            Console.Write("\nThis is not valid input. Please enter a numeric value: ");
            numInput1 = Console.ReadLine();
        }

        return cleanNum;
    }
}