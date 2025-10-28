using System;

namespace CalculatorApp
{
    // ===== Базовий калькулятор =====
    public class Calculator
    {
        // Allow derived classes to update LastResult
        public double LastResult { get; protected set; }

        public double Add(double a, double b)
        {
            LastResult = a + b;
            return LastResult;
        }

        public double Subtract(double a, double b)
        {
            LastResult = a - b;
            return LastResult;
        }

        public double Multiply(double a, double b)
        {
            LastResult = a * b;
            return LastResult;
        }

        public virtual double Divide(double a, double b)
        {
            if (b == 0)
                throw new DivideByZeroException("Ділення на нуль неможливе!");
            LastResult = a / b;
            return LastResult;
        }
    }

    // ===== ScientificCalculator =====
    public class ScientificCalculator : Calculator
    {
        public double Pow(double a, double b)
        {
            LastResult = Math.Pow(a, b);
            return LastResult;
        }

        public double Sqrt(double a)
        {
            if (a < 0)
                throw new InvalidOperationException("Корінь з від’ємного числа неможливий!");
            LastResult = Math.Sqrt(a);
            return LastResult;
        }

        public double Sin(double a)
        {
            LastResult = Math.Sin(a);
            return LastResult;
        }

        public double Cos(double a)
        {
            LastResult = Math.Cos(a);
            return LastResult;
        }
    }

    // ===== ProgrammerCalculator =====
    public class ProgrammerCalculator : Calculator
    {
        // Перевизначення Divide з додатковим логуванням
        public override double Divide(double a, double b)
        {
            Console.WriteLine("[LOG] Виконується ділення у ProgrammerCalculator...");
            return base.Divide(a, b);
        }

        public string ToBinary(int number)
        {
            return Convert.ToString(number, 2);
        }

        public string ToHex(int number)
        {
            return Convert.ToString(number, 16).ToUpper();
        }

        public int And(int a, int b) => a & b;
        public int Or(int a, int b) => a | b;
        public int Xor(int a, int b) => a ^ b;
    }

    // ===== Консольний застосунок =====
    public static class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n=== Виберіть тип калькулятора ===");
                Console.WriteLine("1) Базовий");
                Console.WriteLine("2) Науковий");
                Console.WriteLine("3) Програмістський");
                Console.WriteLine("0) Вихід");
                Console.Write("Ваш вибір: ");

                string choice = Console.ReadLine() ?? string.Empty;
                if (choice == "0") break;

                try
                {
                    switch (choice)
                    {
                        case "1":
                            RunBasicCalculator();
                            break;
                        case "2":
                            RunScientificCalculator();
                            break;
                        case "3":
                            RunProgrammerCalculator();
                            break;
                        default:
                            Console.WriteLine("Невірний вибір!");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка: {ex.Message}");
                }
            }
        }

        static void RunBasicCalculator()
        {
            var calc = new Calculator();
            Console.WriteLine("\n--- Базовий калькулятор ---");
            Console.WriteLine("Операції: +, -, *, /");
            Console.Write("Введіть перше число: ");
            if (!double.TryParse(Console.ReadLine(), out double a))
            {
                Console.WriteLine("Невірне число.");
                return;
            }

            Console.Write("Введіть друге число: ");
            if (!double.TryParse(Console.ReadLine(), out double b))
            {
                Console.WriteLine("Невірне число.");
                return;
            }

            Console.Write("Операція: ");
            string op = Console.ReadLine() ?? string.Empty;

            double result = op switch
            {
                "+" => calc.Add(a, b),
                "-" => calc.Subtract(a, b),
                "*" => calc.Multiply(a, b),
                "/" => calc.Divide(a, b),
                _ => throw new InvalidOperationException("Невідома операція!")
            };

            Console.WriteLine($"Результат: {result}, LastResult = {calc.LastResult}");
        }

        static void RunScientificCalculator()
        {
            var calc = new ScientificCalculator();
            Console.WriteLine("\n--- Науковий калькулятор ---");
            Console.WriteLine("Операції: pow, sqrt, sin, cos");
            Console.Write("Введіть число (або два для pow): ");
            if (!double.TryParse(Console.ReadLine(), out double a))
            {
                Console.WriteLine("Невірне число.");
                return;
            }

            Console.Write("Операція: ");
            string op = Console.ReadLine() ?? string.Empty;

            double result = 0;
            if (op == "pow")
            {
                Console.Write("Введіть степінь: ");
                if (!double.TryParse(Console.ReadLine(), out double b))
                {
                    Console.WriteLine("Невірне число.");
                    return;
                }
                result = calc.Pow(a, b);
            }
            else if (op == "sqrt") result = calc.Sqrt(a);
            else if (op == "sin") result = calc.Sin(a);
            else if (op == "cos") result = calc.Cos(a);
            else throw new InvalidOperationException("Невідома операція!");

            Console.WriteLine($"Результат: {result}, LastResult = {calc.LastResult}");
        }

        static void RunProgrammerCalculator()
        {
            var calc = new ProgrammerCalculator();
            Console.WriteLine("\n--- Програмістський калькулятор ---");
            Console.WriteLine("Операції: bin, hex, and, or, xor, /");
            Console.Write("Введіть перше число: ");
            if (!int.TryParse(Console.ReadLine(), out int a))
            {
                Console.WriteLine("Невірне число.");
                return;
            }

            Console.Write("Операція: ");
            string op = Console.ReadLine() ?? string.Empty;

            if (op == "bin")
                Console.WriteLine($"Binary: {calc.ToBinary(a)}");
            else if (op == "hex")
                Console.WriteLine($"Hex: {calc.ToHex(a)}");
            else
            {
                Console.Write("Введіть друге число: ");
                if (!int.TryParse(Console.ReadLine(), out int b))
                {
                    Console.WriteLine("Невірне число.");
                    return;
                }

                object result = op switch
                {
                    "and" => calc.And(a, b),
                    "or" => calc.Or(a, b),
                    "xor" => calc.Xor(a, b),
                    "/" => calc.Divide(a, b), // ints will be converted to double
                    _ => throw new InvalidOperationException("Невідома операція!")
                };

                Console.WriteLine($"Результат: {result}, LastResult = {calc.LastResult}");
            }
        }
    }
}
